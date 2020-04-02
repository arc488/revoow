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

namespace Revoow.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageRepository pageRepository;
        private readonly ITestimonialRepository testimonialRepository;
        private readonly VideoUploadService uploadService;
        private readonly IMapper mapper;

        public PageController(IPageRepository pageRepository, ITestimonialRepository testimonialRepository, VideoUploadService uploadService, IMapper mapper)
        {
            this.pageRepository = pageRepository;
            this.testimonialRepository = testimonialRepository;
            this.uploadService = uploadService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var viewModel = new CreateViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel viewModel, IFormFile file)
        {
            var page = this.mapper.Map<CreateViewModel, Page>(viewModel);

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                page.Logo = ms.ToArray();
            };

            this.pageRepository.Add(page);

            return View();
        }

        public IActionResult Detail(string companyName)
        {
            Debug.WriteLine("Company name string is " + companyName);
            var page = this.pageRepository.GetByName(companyName);
            if (page == null) return NotFound();
            var viewModel = this.mapper.Map<Page, DetailViewModel>(page);
            return View(viewModel);
        }

        public IActionResult Upload(string companyName)
        {
            var page = this.pageRepository.GetByName(companyName);
            if (page == null) return NotFound();
            var viewModel = this.mapper.Map<Page, UploadViewModel>(page);
            return View(viewModel);
        }

        [HttpPost("/page/upload")]
        public IActionResult UploadVideo()
        {
            //filename is current time stripped of all non numbers
            var fileName = Regex.Replace(DateTime.Now.ToString(), "[^0-9]", "") + ".webm";
            var file = Request.Form.Files[0];
            var ratingValue = Request.Form["ratingValue"];
            var firstName = Request.Form["firstName"];
            var pageId = Request.Form["pageId"];

            var filePath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);

            }

            var testimonial = new Testimonial()
            {
                PageId = Int32.Parse(pageId),
                Rating = Int32.Parse(ratingValue),
                VideoName = fileName,
                ReviewerName = firstName
            };

            testimonialRepository.Add(testimonial);
             
            UploadToYoutube(fileName);
            return View("Upload");
        }

        public void UploadToYoutube(string fileName)
        {
            this.uploadService.Run(fileName).Wait();
            
        }

    }
}
