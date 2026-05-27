using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw8_s30628.DTOs;
using PJATK_APBD_Cw8_s30628.Exceptions;
using PJATK_APBD_Cw8_s30628.FIlters;
using PJATK_APBD_Cw8_s30628.Infrastructure;
using PJATK_APBD_Cw8_s30628.Models;

namespace PJATK_APBD_Cw8_s30628.Services;

public class PatientService(HospitalDbContext ctx) : IPatientService
{
    public async Task<IEnumerable<PatientResponseDTO>> GetAllPatientsAsync(PatientFIlters filters, CancellationToken cancellationToken)
    {
        return await ctx.Patients
            .Where(p => 
                (filters.Search == null || p.FirstName.Contains(filters.Search) || p.LastName.Contains(filters.Search)))
            .Select(p => new PatientResponseDTO
            {
                Pesel = p.Pesel,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                Sex = p.Sex == true ? "M" : "F",
                Admissions = p.Admissions.Select(a => new AdmissionDTO
                {
                    Id = a.Id,
                    AdmissionDate = a.AdmissionDate,
                    DischargeDate = a.DischargeDate,
                    Ward = new WardDto
                    {
                        Id = a.Ward.Id,
                        Name = a.Ward.Name,
                        Description = a.Ward.Description
                    }
                }).ToList(),
                BedAssignments = p.BedAssignments.Select(ba => new BedAssignmentDTO
                {
                    Id = ba.Id,
                    From = ba.From,
                    To = ba.To,
                    Bed = new BedDTO
                    {
                        Id = ba.Bed.Id,
                        BedType = new BedTypeDTO
                        {
                            Id = ba.Bed.BedType.Id,
                            Name = ba.Bed.BedType.Name,
                            Description = ba.Bed.BedType.Description
                        },
                        Room = new RoomDTO
                        {
                            Id = ba.Bed.Room.Id,
                            HasTv = ba.Bed.Room.HasTv,
                            Ward = new WardDto
                            {
                                Id = ba.Bed.Room.Ward.Id,
                                Name = ba.Bed.Room.Ward.Name,
                                Description = ba.Bed.Room.Ward.Description
                            }
                        }
                    }
                }).ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CreateBedAssignment(string pesel, CreateBedAssignmentDTO dto, CancellationToken cancellationToken)
    {
        var patient = await ctx.Patients.FirstOrDefaultAsync(p => p.Pesel == pesel, cancellationToken) 
                      ?? throw new NotFoundException($"Pacjent z peselem '{pesel}' nie istnieje");
        
        var ward = await ctx.Wards.FirstOrDefaultAsync(w => w.Name == dto.Ward, cancellationToken)
                   ?? throw new NotFoundException($"Oddzial o nazwie '{dto.Ward}' nie istnieje");
        
        var bedType = await ctx.BedTypes.FirstOrDefaultAsync(bt => bt.Name == dto.BedType, cancellationToken)
                      ?? throw new NotFoundException($"Lozko o typie '{dto.BedType}' nie istnieje");

        if (dto.To.HasValue && dto.To.Value < dto.From) throw new BadRequestException("Odpis musi byc pozniejszy niz zapis");
        
        var availableBed = await ctx.Beds
                               .Where(b => b.BedTypeId == bedType.Id)
                               .Where(b => b.Room.WardId == ward.Id)
                               .Where(b => !b.BedAssignments.Any(ba =>
                                   (dto.To == null || ba.From < dto.To) &&
                                   (ba.To == null || ba.To > dto.From)))
                               .FirstOrDefaultAsync(cancellationToken)
                           ?? throw new NotFoundException($"Nie ma dostepnego lozka '{dto.BedType}' na oddziale '{dto.Ward}' w podanym przedziale czasowym ({dto.From} : {dto.To})");

        var assignment = new BedAssignment
        {
            PatientPesel = patient.Pesel,
            BedId = availableBed.Id,
            From = dto.From,
            To = dto.To
        };

        ctx.BedAssignments.Add(assignment);
        await ctx.SaveChangesAsync(cancellationToken);

        return assignment.Id;
    }
}