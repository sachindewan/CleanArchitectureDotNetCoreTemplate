using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.WebApi1.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.WebApi1.Application.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IAuthenticatedUserService _currentUserService;
        private readonly IAccountService _identityService;
        private readonly ILogger<TRequest> _logger;
        private readonly Stopwatch _timer;

        public PerformanceBehaviour(
            ILogger<TRequest> logger,
            IAuthenticatedUserService currentUserService,
            IAccountService identityService)
        {
            _timer = new Stopwatch();

            _logger = logger;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var userId = _currentUserService.UserId ?? string.Empty;
                var userName = string.Empty;

                if (!string.IsNullOrEmpty(userId)) userName = await _identityService.GetUserNameAsync(userId);

                _logger.LogWarning(
                    "CleanArchitecture Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userId, userName, request);
            }

            return response;
        }
    }
}