using EventSystem.Application.DTOs.Auth;
using EventSystem.Domain.Entities;
using EventSystem.Infrastructure.Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Infrastructure.Profiles
{
    public class AppUserProfile : AutoMapper.Profile
    {
        public AppUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap();
            
            CreateMap<User, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore())  
            .ForMember(dest => dest.Email, opt => opt.Ignore())     
            .ForMember(dest => dest.Id, opt => opt.Ignore())        
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore()).ReverseMap();
        }
    }
}