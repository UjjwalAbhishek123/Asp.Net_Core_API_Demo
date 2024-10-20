using AutoMapper;
using LearnApiDemo.Data;
using LearnApiDemo.Helper;
using LearnApiDemo.Repositories;
using LearnApiDemo.RepositoryImplementation;
using LearnApiDemo.ServiceImplementation;
using LearnApiDemo.Services;
using Microsoft.EntityFrameworkCore;

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
