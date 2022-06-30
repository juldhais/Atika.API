using Atika.API;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlite("Data Source=atika.db"));
builder.Services.AddScoped<QuizService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
    o.AddDefaultPolicy(b =>
        b.AllowAnyHeader()
         .AllowAnyMethod()
         .AllowAnyOrigin()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
using var db = serviceScope?.ServiceProvider.GetRequiredService<DataContext>();
db?.Database.EnsureCreated();

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.Run();
