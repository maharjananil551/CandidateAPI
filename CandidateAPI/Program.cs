using AutoMapper;
using CandidateAPI.Mapping;
using CandidateAPI.Data;
using Microsoft.EntityFrameworkCore;
using CandidateAPI.Repositories;
using CandidateAPI.Services.CoreServices;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddSingleton<ICacheService, HashTableCacheService>();

// Register ApplicationDbContext with the appropriate database provider
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Adjust to your connection string

builder.Services.AddSwaggerGen();


// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
