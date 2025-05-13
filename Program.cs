using Hangfire;
using Serilog;
using Survey_Basket;
using Survey_Basket.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDependencies(builder.Configuration);

builder.Host.UseSerilog((context,configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json" , "v1"));
    app.UseHangfireDashboard("/Jobs");
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.UseExceptionHandler();

app.Run();
