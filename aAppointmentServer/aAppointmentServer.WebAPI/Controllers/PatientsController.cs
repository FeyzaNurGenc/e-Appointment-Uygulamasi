using aAppointmentServer.Application.Features.Patients.CreatePatient;
using aAppointmentServer.Application.Features.Patients.DeletePatientById;
using aAppointmentServer.Application.Features.Patients.GetAllPatien;
using aAppointmentServer.Application.Features.Patients.UpdatePatient;
using aAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace aAppointmentServer.WebAPI.Controllers
{
    public sealed class PatientsController : ApiController
    {
        public PatientsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);

            //Result<string> response = (HttpStatusCode.InternalServerError, "Example error");
            //return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }


        [HttpPost]
        public async Task<IActionResult> DeleteById(DeletePatientByIdCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)response.StatusCode, response);

        }
    }
}
