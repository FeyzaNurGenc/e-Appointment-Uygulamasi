using aAppointmentServer.Application.Features.Auth.Login;
using aAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aAppointmentServer.WebAPI.Controllers
{
    [AllowAnonymous]
 
    public sealed class AuthController : ApiController
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand request,CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request , cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
