using AutoMapper;
using ITRoots.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITRoots
{
    public class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }
        public static void Init()
        {
            var config = new MapperConfiguration(cfg => {

                cfg.CreateMap<RegisterVM, Users>();
                cfg.CreateMap<LoginUser_Result, UserVM>();

                cfg.CreateMap<SelectUserList_Result, UserVM>().ReverseMap();
                cfg.CreateMap<Users, UserVM>().ReverseMap();

                cfg.CreateMap<Invoices, InvoiceVM>()
                .ForMember( dest => dest.User,
                     opt => opt.MapFrom(src => src.Users))
                .ForMember(dest => dest.Products,
                     opt => opt.MapFrom(src => src.Products));

                cfg.CreateMap<InvoiceVM, Invoices>();
                cfg.CreateMap<Products, ProductVM>().ReverseMap();
            });
            Mapper = config.CreateMapper();
        }
    }
}