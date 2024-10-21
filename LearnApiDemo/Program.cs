using AutoMapper;
using LearnApiDemo.Data;
using LearnApiDemo.Helper;
using LearnApiDemo.Repositories;
using LearnApiDemo.RepositoryImplementation;
using LearnApiDemo.ServiceImplementation;
using LearnApiDemo.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registering Services to IoC Container
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<ICustomerService, CustomerService>();

//Register Connection String
builder.Services.AddDbContext<LearnApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("apiCon")));

//Crceating Mapper Configs & Registering Automapper
var autoMapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperHandler()));
IMapper mapper = autoMapper.CreateMapper();

builder.Services.AddSingleton(mapper);

//Adding CORS
builder.Services.AddCors(policy => policy.AddPolicy("corsPolicy", build =>
{
    //API can be accessed for any Domain using "*"
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddCors(policy => policy.AddPolicy("corsPolicy1", build =>
{
    build.WithOrigins("https://domain3.com").AllowAnyMethod().AllowAnyHeader();
}));

//Enabling Default policy
builder.Services.AddCors(policy => policy.AddDefaultPolicy(build =>
{
    //API can be accessed for any Domain using "*"
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//Adding Serilog Configs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//Use Serilog for Logging
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS middleware
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
