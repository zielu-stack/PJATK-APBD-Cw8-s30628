namespace PJATK_APBD_Cw8_s30628.DTOs;

public record PatientResponseDTO
{
    public string Pesel { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public int Age { get; init; }
    public string Sex { get; init; } = string.Empty;
    public List<AdmissionDTO> Admissions { get; init; } = [];
    public List<BedAssignmentDTO> BedAssignments { get; init; } = [];
}
