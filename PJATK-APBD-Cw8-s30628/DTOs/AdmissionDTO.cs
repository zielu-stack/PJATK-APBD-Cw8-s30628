namespace PJATK_APBD_Cw8_s30628.DTOs;

public record AdmissionDTO
{
    public int Id { get; init; }
    public DateTime AdmissionDate { get; init; }
    public DateTime? DischargeDate { get; init; }
    public WardDto Ward { get; init; } = new WardDto();
}
