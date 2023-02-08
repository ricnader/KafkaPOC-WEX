using KafkaConsumerPOC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHostedService, KafkaConsumerService>();


// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
