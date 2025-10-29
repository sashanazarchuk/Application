using EventSystem.Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Interfaces.Services
{
    public interface IUserSnapshotService
    {
        Task<UserSnapshotDto> GetUserSnapshotAsync(CancellationToken cancellationToken);
    }
}
