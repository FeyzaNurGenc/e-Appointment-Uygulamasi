using aAppointmentServer.Application.Features.Roles.RoleSync;
using aAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aAppointmentServer.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class RolesController : ApiController
    {
        public RolesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Sync(RoleSyncCommand request,CancellationToken CancellationToken)
        {
            var response = await _mediator.Send(request, CancellationToken);
            return StatusCode((int)response.StatusCode ,response);
        } 
    }

}