using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Revoow.Data.IRepositories;
using Revoow.Services;
using Revoow.Models;
using Revoow.ViewModels.Revoow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Revoow.Areas.Identity;

namespace Revoow.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        private readonly IPageRepository pageRepository;
        private readonly ITestimonialRepository testimonialRepository;
        private readonly VideoService videoService;
        private readonly UserManager<RevoowUser> userManager;
        private readonly IMapper mapper;

        public PageController(IPageRepository pageRepository,
                              ITestimonialRepository testimonialRepository,
                              VideoService videoService,
                              UserManager<RevoowUser> userManager,
                              IMapper mapper)
        {
            this.pageRepository = pageRepository;
            this.testimonialRepository = testimonialRepository;
            this.videoService = videoService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(string pageUrl)
        {
            var page = this.pageRepository.GetByUrl(pageUrl);
            if (page == null)
            {
                return NotFound();
            }
            var viewModel = this.mapper.Map<Page, DetailViewModel>(page);
            Debug.WriteLine(page.CreatedBy.MaxVideos);
            viewModel.IsVideoLimitReached = (int)page.Testimonials.Count >= (int)page.CreatedBy.MaxVideos;
            return View(viewModel);
        }

        public IActionResult Upload(string pageUrl)
        {
            var page = this.pageRepository.GetByUrl(pageUrl);
            if (page == null) return NotFound();
            var viewModel = this.mapper.Map<Page, UploadViewModel>(page);
            return View(viewModel);
        }

        [HttpPost("/page/upload")]
        public IActionResult Upload()
        {
            string redirectUrl;

            var user = userManager.GetUserAsync(HttpContext.User).Result;

            var file = Request.Form.Files[0];
            var ratingValue = Int32.Parse(Request.Form["ratingValue"]);
            var reviewerName = Request.Form["reviewerName"].ToString();
            var pageId = Int32.Parse(Request.Form["pageId"]);
            var companyName = Request.Form["companyName"];

            var page = pageRepository.GetById(pageId);


            if (!(page.Testimonials == null) && (page.Testimonials.Count >= user.MaxVideos))
            {

                redirectUrl = Request.Host + "/" + page.PageURL;
                return Json(new { url = redirectUrl });
            }

            if (page.Testimonials.Count >= user.MaxVideos)
            {
                videoService.SaveVideo(file);
                var thumbnail = videoService.GenerateThumbnail();
                var testimonial = new Testimonial()
                {
                    PageId = pageId,
                    Rating = ratingValue,
                    VideoName = videoService.fileName,
                    VideoPath = videoService.videoPath,
                    VideoThumbnail = thumbnail,
                    ReviewerName = reviewerName,
                };
                testimonialRepository.Add(testimonial);
                page.Testimonials.Add(testimonial);
                pageRepository.Update(page);
            }
            redirectUrl = Request.Host + "/" + page.PageURL;

            return Json(new { url = redirectUrl });

        }

    }
}
