using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WindowsPrinterService;

var builder = Host.CreateApplicationBuilder(args);

// Configure the service to run as a Windows Service
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "WindowsPrinterService"; // Change this to your service name
});

// Configure Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();

    // Add EventLog to log to the Windows Event Viewer
    logging.AddEventLog(settings =>
    {
        settings.SourceName = "WindowsPrinterService"; // Change the source name if needed
    });

    // If running in development or in debug mode, log to console
    if (builder.Environment.IsDevelopment())
    {
        logging.AddConsole(); // This will log to the console only in development mode
    }
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
