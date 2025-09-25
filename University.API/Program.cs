using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using University.Core.Services;
using University.Data.Contexts;
using University.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Swagger configuration 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Serilog configuration 
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();


// Dependency Injection with Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<UniversityDbContext>().AsSelf().InstancePerLifetimeScope();  
    container.RegisterType<StudentRepositories>().As<IStudentRepository>().InstancePerLifetimeScope();
    container.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
});

// Use Serilog for logging 
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
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
