using AutoMapper;
using CWheelsAPI.Controllers.Resources;
using CWheelsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWheelsAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //Domain to API Resources
            CreateMap<Category, CategoryResource>();
            CreateMap<User, UserResource>();
            CreateMap<User, ContactResource>();
            CreateMap<Photo, PhotoResource>();
            CreateMap<Thumbnail, ThumbnailResource>();
            CreateMap<Vehicle, SearchVehicleResource>();
            CreateMap<Vehicle, VehicleByCategoryResource>()
                .ForMember(vr => vr.ThumbnailUrl, opt => opt.MapFrom(vf => vf.Thumbnails.Select(vr => vr.ThumbnailUrl).FirstOrDefault()));
            CreateMap<Vehicle, HotAndNewVehicleResource>()
                .ForMember(vr => vr.ThumbnailUrl, opt => opt.MapFrom(vf => vf.Thumbnails.Select(vr => vr.ThumbnailUrl).FirstOrDefault()));
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr => vr.Contact, opt => opt.MapFrom(vf => new ContactResource { Email = vf.User.Email, Phone = vf.User.Phone,Name=vf.User.Name }))
                .ForMember(vr => vr.Category, opt => opt.MapFrom(vf => new KeyValuePairResource { Id = vf.Category.Id, Type = vf.Category.Type }));
            CreateMap<Vehicle, MyAdsResource>()
                .ForMember(vr => vr.ThumbnailUrl, opt => opt.MapFrom(vf => vf.Thumbnails.Select(vr => vr.ThumbnailUrl).FirstOrDefault()));


            //API Resources to Domain
            CreateMap<LoginUserResource, User>();
            CreateMap<RegisterUserResource, User>();
            CreateMap<SaveVehicleResource, Vehicle>()
                 .ForMember(v => v.Id, opt => opt.Ignore());

        }
    }
}
