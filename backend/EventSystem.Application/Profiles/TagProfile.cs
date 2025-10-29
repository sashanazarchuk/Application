using EventSystem.Application.DTOs.Tag;
using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Profiles
{
    public class TagProfile : AutoMapper.Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}