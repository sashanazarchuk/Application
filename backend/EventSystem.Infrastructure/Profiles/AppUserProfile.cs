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
            CreateMap<ApplicationUser, User>().ReverseMap();
        }
    }
}