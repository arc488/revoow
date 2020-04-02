using AutoMapper;
using Revoow.Models;
using Revoow.ViewModels.Revoow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revoow.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Page, CreateViewModel>().ReverseMap();
            CreateMap<Page, DetailViewModel>().ReverseMap();
            CreateMap<Page, UploadViewModel>().ReverseMap();
        }

    }
}
