using Domain;
using Persistence;
using VoucherApi.Mapping;

var builder = WebApplication.CreateBuilder(args);
var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var migrationManager = new MigrationManager(loggerFactory.CreateLogger<MigrationManager>(), builder.Configuration);
migrationManager.Migrate();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDomainService();
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
