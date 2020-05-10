using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Revoow.Data.IRepositories;
using Revoow.Models;
using Revoow.ViewModels.Revoow;

namespace Revoow.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly IPageRepository pageRepository;
        private readonly IMapper mapper;

        public CompanyController(IPageRepository pageRepository, IMapper mapper)
        {
            this.pageRepository = pageRepository;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var model = CreateViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel viewModel, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                var model = CreateViewModel();
                return View(model);
            };

            var page = this.mapper.Map<CreateViewModel, Page>(viewModel);

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                page.Logo = ms.ToArray();
            };

            this.pageRepository.Add(page);
            return Redirect("~/" + page.PageURL);
        }

        public CreateViewModel CreateViewModel()
        {
            var model = new CreateViewModel
            {
                URLsInUse = this.pageRepository.GetAll().Select(p => p.PageURL).ToList(),
                NamesInUse = this.pageRepository.GetAll().Select(p => p.CompanyName).ToList()

            };

            return model;
        }

    }
}