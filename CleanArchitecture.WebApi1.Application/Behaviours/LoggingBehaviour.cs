using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.WebApi1.Application.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.WebApi1.Application.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly IAuthenticatedUserService _currentUserService;
        private readonly IAccountService _identityService;
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<TRequest> logger, IAuthenticatedUserService currentUserService,
            IAccountService identityService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? string.Empty;
            var userName = string.Empty;

            if (!string.IsNullOrEmpty(userId)) userName = await _identityService.GetUserNameAsync(userId);

            _logger.LogInformation("CleanArchitecture Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }
    }
}