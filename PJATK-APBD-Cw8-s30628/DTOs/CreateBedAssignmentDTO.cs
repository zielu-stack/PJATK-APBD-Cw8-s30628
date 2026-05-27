namespace PJATK_APBD_Cw8_s30628.DTOs;

public record CreateBedAssignmentDTO
{
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
    public string BedType { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;

}