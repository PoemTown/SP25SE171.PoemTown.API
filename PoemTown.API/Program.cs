using PoemTown.API;
using PoemTown.API.CustomMiddleware;
using PoemTown.Repository;
using PoemTown.Service;
using PoemTown.Service.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddConfigureServiceRepository(builder.Configuration);
builder.Services.AddConfigureServiceService(builder.Configuration, builder.Environment);
builder.Services.AddConfigureServiceApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
await app.UseInitializeDatabaseAsync();

app.AddApplicationApi();
app.MapControllers();
app.Run();