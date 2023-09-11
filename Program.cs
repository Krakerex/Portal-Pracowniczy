using krzysztofb.Configuration;
using krzysztofb.Email;
using krzysztofb.Models;
using krzysztofb.Services;
using krzysztofb.Services.Request.Exceptions;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WnioskiContext>();
builder.Services.AddScoped<UzytkownikService>();
builder.Services.AddScoped<WniosekService>();
builder.Services.AddScoped<MemoryStream>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Hour)
        .WriteTo.Console();
});
//add serilog logger

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
