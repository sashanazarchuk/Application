using EventSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Repositories
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task AddRangeAsync(IEnumerable<Tag> entities, CancellationToken token);  
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Tag>> GetByNamesAsync(IEnumerable<string> names, CancellationToken token);

    }
}