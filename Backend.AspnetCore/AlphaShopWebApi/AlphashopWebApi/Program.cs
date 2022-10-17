var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

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
