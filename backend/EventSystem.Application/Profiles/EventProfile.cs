using EventSystem.Application.DTOs.Event;
using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Profiles
{
    public class EventProfile:AutoMapper.Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventDto>()
            .ForMember(dest => dest.CurrentParticipantsCount,
                       opt => opt.MapFrom(src => src.Participants.Count));

            CreateMap<CreateEventDto, Event>()
            .ForMember(dest => dest.Participants, opt => opt.Ignore())
            .ForMember(dest => dest.AdminId, opt => opt.Ignore()) 
            .ForMember(dest => dest.Admin, opt => opt.Ignore());

            CreateMap<PatchEventDto, Event>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}