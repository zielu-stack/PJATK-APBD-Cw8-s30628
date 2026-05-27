using PJATK_APBD_Cw8_s30628.DTOs;
using PJATK_APBD_Cw8_s30628.FIlters;
using PJATK_APBD_Cw8_s30628.Models;

namespace PJATK_APBD_Cw8_s30628.Services;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDTO>> GetAllPatientsAsync(PatientFIlters filters, CancellationToken cancellationToken);
    Task<int> CreateBedAssignment(string pesel, CreateBedAssignmentDTO patient,  CancellationToken cancellationToken);
}