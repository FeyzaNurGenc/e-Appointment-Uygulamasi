using aAppointmentServer.Application.Features.Users.CreateUser;
using aAppointmentServer.Application.Features.Users.DeleteUserById;
using aAppointmentServer.Application.Features.Users.GetAllRolesForUsers;
using aAppointmentServer.Application.Features.Users.GetAllUsers;
using aAppointmentServer.Application.Features.Users.UpdateUser;
using aAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace aAppointmentServer.WebAPI.Controllers
{
    public sealed class UsersController : ApiController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);


        }
        [HttpPost]
        public async Task<IActionResult> GetAllRoles(GetAllRolesForUsersQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }


        [HttpPost]
        public async Task<IActionResult> DeleteById(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }
    }
}
