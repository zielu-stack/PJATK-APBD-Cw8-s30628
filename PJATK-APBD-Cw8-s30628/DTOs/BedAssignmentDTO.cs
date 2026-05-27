namespace PJATK_APBD_Cw8_s30628.DTOs;

public record BedAssignmentDTO
{
    public int Id { get; init; }
    public DateTime From { get; init; }
    public DateTime? To { get; init; }
    public BedDTO Bed { get; init; } = new BedDTO();
}
