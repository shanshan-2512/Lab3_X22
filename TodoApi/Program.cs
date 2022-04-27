using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var dbmsVersion = new MariaDbServerVersion(builder.Configuration.GetValue<string>("DBMSVersion"));
var connString = builder.Configuration.GetConnectionString("TodoDbContext");


builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseMySql(connString, dbmsVersion));

builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
	opt.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
