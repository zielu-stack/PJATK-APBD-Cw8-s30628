namespace PJATK_APBD_Cw8_s30628.DTOs;

public record BedDTO
{
    public int Id { get; init; }
    public BedTypeDTO BedType { get; init; } = new BedTypeDTO();
    public RoomDTO Room { get; init; } = new RoomDTO();
}
