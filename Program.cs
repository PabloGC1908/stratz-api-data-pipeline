using Microsoft.EntityFrameworkCore;
using StratzAPI.Data;
using StratzAPI.Models;
using StratzAPI.Repositories;
using StratzAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<GraphQLService>();
builder.Services.AddScoped<TeamRepository>();
builder.Services.AddScoped<LeagueRepository>();
builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<MatchRepository>();
builder.Services.AddScoped<MatchStatsPickBansRepository>();
builder.Services.AddScoped<MatchPlayerRepository>();
builder.Services.AddScoped<SerieRepository>();
builder.Services.AddScoped<PlaybackDataRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("SQL"))
        .LogTo(Console.WriteLine, LogLevel.Information);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
