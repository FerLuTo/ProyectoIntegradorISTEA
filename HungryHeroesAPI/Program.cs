using System.Text.Json.Serialization;
using AccessData;
using Business.Interfaces;
using Business.Services;
using Common;
using Common.Helper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//Add Services to DI Container
{
    var services = builder.Services;
    var env = builder.Environment;

    // Add services to the container.
    services.AddDbContext<AppDBContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("HungryHeroes"));
    });
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        //Serializa los enums como string en la respuesta de la api (Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddSwaggerGen();

    //Configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    //Configure DI for application services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IBusinessService, BusinessService>();
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<ISaleService, SaleService>();
    services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();

}

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    app.UseMiddleware<ErrorHandlerMiddleware>();

    //custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();

}

app.UseAuthorization();

app.Run();
