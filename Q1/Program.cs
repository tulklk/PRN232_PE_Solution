using Microsoft.EntityFrameworkCore;
using Q1.Data;

var builder = WebApplication.CreateBuilder(args);

// Required root URL for the exam
builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddControllers();

builder.Services.AddDbContext<PE_PRN_Fall22B1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// When Visual Studio opens the root URL, redirect it to Swagger UI.
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});

app.MapControllers();

app.Run();
