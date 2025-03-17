using DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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


//Services -------------------------------------------------------------------------
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
