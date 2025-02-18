using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsPrinterService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly string filePath = @"C:\PrintQueue\printjob.txt";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Printer Service Started...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        _logger.LogInformation("Print job file detected. Reading content...");

                        // Read print job details
                        string printJob = File.ReadAllText(filePath, Encoding.UTF8);
                        _logger.LogInformation($"Print job content: {printJob}");

                        string[] parts = printJob.Split('|');
                        if (parts.Length == 2)
                        {
                            string printerName = parts[0];
                            string printContent = parts[1];

                            _logger.LogInformation($"Printing to: {printerName}");
                            _logger.LogInformation($"Print Content: {printContent}");

                            
                            string escpText = printContent;

                            byte[] bytes = Encoding.ASCII.GetBytes(escpText);
                            bool result = RawPrinterHelper.SendBytesToPrinter(printerName, bytes);

                            if (result)
                            {
                                _logger.LogInformation("Print successful.");
                                File.Delete(filePath); // Delete after successful printing
                            }
                            else
                            {
                                _logger.LogError("Print failed. Check printer connection.");
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Invalid print job format. Expected 'PrinterName|PrintContent'.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during printing: {ex.Message}");
                }

                await Task.Delay(5000, stoppingToken); // Check every 5 seconds
            }
        }
    }
}
