using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw8_s30628.DTOs;
using PJATK_APBD_Cw8_s30628.Exceptions;
using PJATK_APBD_Cw8_s30628.FIlters;
using PJATK_APBD_Cw8_s30628.Services;

namespace PJATK_APBD_Cw8_s30628.Controlers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] PatientFIlters filters, CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllPatientsAsync(filters, cancellationToken));
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AddBedAssignment(string pesel, [FromBody] CreateBedAssignmentDTO dto, CancellationToken cancellationToken)
    {
        try
        {
            var newId = await service.CreateBedAssignment(pesel, dto, cancellationToken);
            return Created($"/api/patients/{pesel}/bedassignments/{newId}", new { id = newId });
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
}