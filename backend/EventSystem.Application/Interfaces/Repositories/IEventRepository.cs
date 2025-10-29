using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Repositories
{
    public interface IEventRepository:IGenericRepository<Event>
    {
        Task JoinEventAsync(Participant participant, CancellationToken token);

        Task<Participant?> GetParticipantAsync(Guid eventId, Guid userId, CancellationToken token);
        Task LeaveEventAsync(Participant participant, CancellationToken token);
        Task<IEnumerable<Event>> FetchUserEventsAsync(Guid userId, CancellationToken token);
        Task<IEnumerable<Event>> GetEventsByTagsAsync(IEnumerable<string> tags, CancellationToken token);
    }
}
