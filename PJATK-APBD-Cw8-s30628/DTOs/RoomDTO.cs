namespace PJATK_APBD_Cw8_s30628.DTOs;

public record RoomDTO
{
    public string Id { get; init; } = string.Empty;
    public bool HasTv { get; init; }
    public WardDto Ward { get; init; } = new WardDto();
}
