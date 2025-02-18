# Windows Printer Service

## Overview
This project is a Windows Service built with **.NET Core 8** that monitors a specified file for print job instructions and sends the print data to a specified printer using **ESC/P commands**.

## Features
- Runs as a background service.
- Monitors a print job file (`printjob.txt`) for print commands.
- Reads print job details and sends data to the specified printer.
- Supports **ESC/P (Epson Standard Code for Printers) commands**.
- Logs print job status and errors.
- Deletes the print job file after successful printing.

## How It Works
1. The service continuously checks for the existence of a print job file (`C:\PrintQueue\printjob.txt`).
2. If a file is detected, it reads its content, which should be in the format:
   ```
   PrinterName|PrintContent
   ```
3. The content is parsed, and the service sends the data to the specified printer using ESC/P commands.
4. If printing is successful, the file is deleted; otherwise, an error is logged.
5. The service repeats the check every **5 seconds**.

## Prerequisites
- .NET Core 8 SDK installed.
- A configured ESC/P-compatible printer.
- The `C:\PrintQueue\` directory must exist.
- Proper permissions for the service to access files and communicate with printers.

## Installation & Deployment
1. **Build the project**
   ```sh
   dotnet publish -c Release -r win-x64
   ```
2. **Install the service** (Run as Administrator)
   ```sh
   sc create WindowsPrinterService binPath="C:\path\to\WindowsPrinterService.exe" start=auto
   ```
3. **Start the service**
   ```sh
   sc start WindowsPrinterService
   ```

## Configuration
- The `filePath` variable in `Worker.cs` specifies the monitored file location.
- Modify the logic in `RawPrinterHelper.SendBytesToPrinter()` if additional printer handling is needed.

## Logging
- Logs are recorded using **Microsoft.Extensions.Logging**.
- The logs help debug issues like file format errors or printer connectivity problems.

## Troubleshooting
- Ensure the printer is correctly installed and accessible.
- Check log messages for errors.
- Verify that the print job file format follows `PrinterName|PrintContent`.
- Restart the service if needed:
  ```sh
  sc stop WindowsPrinterService
  sc start WindowsPrinterService
  ```

## License
This project is open-source and available for modification and distribution.

