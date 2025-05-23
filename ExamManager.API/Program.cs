using DatabaseContext;
using ExamManager.API.Services;
using ExamManager.Configuration;
using ExamManager.Extensions;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Services.AdminActions;
using Services.Authentication;
using Services.EmailSender;
using Services.Exam;
using Services.ProfessorExam;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; //Api request doesn't lowercase object atrribute
}); ;
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

//Connection to database -------------------------------------------------------------------------
builder.Services.AddDbContext<ExamManagerContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")));

//Configuration -------------------------------------------------------------------------

var jwtConfig = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();

builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));


builder.Services.AddLogging();
builder.Services.AddTransient<Middleware>();

//Services -------------------------------------------------------------------------
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IExamService, ExamService>();
builder.Services.AddTransient<IProfessorExamService, ProfessorExamService>();
builder.Services.AddTransient<IAdminActionsService, AdminActionsService>();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();


builder.Services.AddHostedService<SendEmailTimer>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Use CORS before controllers
app.UseCors("AllowReactApp");

app.UseMiddleware<Middleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
