using EventSystem.Application.DTOs.Tag;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Queries.Tags.GetAllTags
{
    public record GetAllTagsQuery:IRequest<IEnumerable<TagDto>>;
    

}
