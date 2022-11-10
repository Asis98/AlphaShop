using AlphashopWebApi.Services;
using ArticoliWebService.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors();
builder.Services.AddDbContext<AlphaShopDbContext>();

builder.Services.AddScoped<IArticoliRepository, ArticoliRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();
app.UseCors(
    options =>
        options
            //.WithOrigins("http://localhost:4500")
            //.WithMethods("POST", "PUT", "DELETE", "GET")
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
