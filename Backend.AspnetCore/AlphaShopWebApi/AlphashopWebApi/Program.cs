using AlphashopWebApi.Services;
using ArticoliWebService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddDbContext<AlphaShopDbContext>();

builder.Services.AddScoped<IArticoliRepository, ArticoliRepository>();

var app = builder.Build();

app.UseRouting();
app.UseCors(options =>
    options
    //.WithOrigins("http://localhost:4500")
    //.WithMethods("POST", "PUT", "DELETE", "GET")
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.MapControllers(); 

app.Run();
