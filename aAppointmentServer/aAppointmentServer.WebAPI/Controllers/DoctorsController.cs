using aAppointmentServer.Application.Features.Doctors.CreateDoctor;
using aAppointmentServer.Application.Features.Doctors.DeleteDoctorById;
using aAppointmentServer.Application.Features.Doctors.GetAllDoctor;
using aAppointmentServer.Application.Features.Doctors.UpdateDoctor;
using aAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TS.Result;

namespace aAppointmentServer.WebAPI.Controllers
{
    public sealed class DoctorsController : ApiController
    {
        public DoctorsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllDoctorsQuery request, CancellationToken cancellationToken)
        {
             var response = await _mediator.Send(request, cancellationToken);
           
            return StatusCode((int)response.StatusCode, response);

            //Result<string> response = (HttpStatusCode.InternalServerError, "Example error");
            //return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }


        [HttpPost]
        public async Task<IActionResult> DeleteById(DeleteDoctorByIdCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }

    }
}
