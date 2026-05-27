using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw8_s30628.Infrastructure;
using PJATK_APBD_Cw8_s30628.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddDbContext<HospitalDbContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("HospitalDb")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/openapi/v1.json", "Api"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// + komenda na scaffold, ktorej uzylem do wygenerowania bazy
// dotnet ef dbcontext scaffold "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master" Microsoft.EntityFrameworkCore.SqlServer --context HospitalDbContext --context-dir Infrastructure --output-dir Models --no-onconfiguring --table Patients --table Admissions --table Wards --table Beds --table BedTypes --table Rooms --table BedAssignments --force
