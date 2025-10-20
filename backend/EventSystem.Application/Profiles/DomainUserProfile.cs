using EventSystem.Application.DTOs.Auth;
using EventSystem.Application.DTOs.Users;
using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Profiles
{
    public class DomainUserProfile: AutoMapper.Profile
    {
        public DomainUserProfile()
        {
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AdminEvents, opt=> opt.Ignore())
                .ForMember(dest => dest.Participations, opt=> opt.Ignore());

            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}