using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"[DB PATH] {Path.GetFullPath(connection)}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
