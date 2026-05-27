namespace PJATK_APBD_Cw8_s30628.DTOs;

public record BedTypeDTO
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
