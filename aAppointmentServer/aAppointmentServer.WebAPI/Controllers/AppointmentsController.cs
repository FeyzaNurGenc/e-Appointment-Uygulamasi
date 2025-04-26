using aAppointmentServer.Application.Features.Appointments.CreateAppointment;
using aAppointmentServer.Application.Features.Appointments.DeleteAppointmentById;
using aAppointmentServer.Application.Features.Appointments.GetAllAppointments;
using aAppointmentServer.Application.Features.Appointments.GetAllDoctorByDepartment;
using aAppointmentServer.Application.Features.Appointments.GetPatientByIdentityNumber;
using aAppointmentServer.Application.Features.Appointments.UpdateAppointment;
using aAppointmentServer.Application.Features.Doctors.CreateDoctor;
using aAppointmentServer.Application.Features.Doctors.DeleteDoctorById;
using aAppointmentServer.Application.Features.Doctors.GetAllDoctor;
using aAppointmentServer.Application.Features.Doctors.UpdateDoctor;
using aAppointmentServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace aAppointmentServer.WebAPI.Controllers
{
    public sealed class AppointmentsController : ApiController
    {
        public AppointmentsController(IMediator mediator) : base(mediator) 
        { 
        }

        [HttpPost]
        public async Task<IActionResult> GetAllDoctorByDepartment(GetAllDoctorsByDepartmentQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);

            //Result<string> response = (HttpStatusCode.InternalServerError, "Example error");
            //return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost]
        public async Task<IActionResult> GetAllByDoctorId(GetAllAppointmentsByDoctorIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);

    
        }

        [HttpPost]
        public async Task<IActionResult> GetPatientByIdentityNumber(GetPatientByIdentityNumberQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);



        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);



        }
        [HttpPost]
        public async Task<IActionResult> DeleteById(DeleteAppointmentByIdCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)response.StatusCode, response);

        }


    }
}
