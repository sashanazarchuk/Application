using EventSystem.Application.DTOs.Users;
using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Profiles
{
    public class ParticipantProfile:AutoMapper.Profile
    {
        public ParticipantProfile()
        {
            CreateMap<Participant, ParticipantDto>()
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
           .ForMember(dest => dest.JoinedAt, opt => opt.MapFrom(src => src.JoinedAt));
        }
    }
}
