using EventSystem.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem.Application.Commands.Account.Logout
{
    internal class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        private readonly IUserService _userService;
        public LogoutUserCommandHandler(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.LogoutAsync(request.UserId);
        }
    }
}