using System;
using System.IO;

public class SimpleLogger
{
    private readonly string logFilePath;

    public SimpleLogger(string filePath)
    {
        logFilePath = "./logs/";

        if(!Directory.Exists(logFilePath)) Directory.CreateDirectory(logFilePath);

        logFilePath += filePath;

        // Pokud soubor neexistuje, vytvoří ho
        if (!File.Exists(logFilePath))
        {
            using (File.Create(logFilePath)) { }
        }
    }

    public void Log(string message)
    {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        try
        {
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    public void LogError(string message)
    {
        Log($"ERROR: {message}");
    }

    public void LogInfo(string message)
    {
        Log($"INFO: {message}");
    }

    public void LogWarning(string message)
    {
        Log($"WARNING: {message}");
    }
}