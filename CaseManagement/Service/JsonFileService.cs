using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace UseCaseDrivenDevelopment.CaseManagement.Service;

public abstract class JsonFileService
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    protected string FileName { get; }

    protected JsonFileService(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(nameof(fileName));
        }

        // file is located on the executable folder
        var assembly = Assembly.GetEntryAssembly();
        var directory = assembly != null ? Path.GetDirectoryName(assembly.Location) : Path.GetDirectoryName(fileName);
        FileName = directory != null ? Path.Combine(directory, fileName) : fileName;
    }

    protected List<T> ReadFromFile<T>()
    {
        if (!File.Exists(FileName))
        {
            return new();
        }

        var caseFields = JsonSerializer.Deserialize<List<T>>(
            File.ReadAllText(FileName),
            serializerOptions);
        return caseFields ?? new();
    }

    protected void WriteToFile<T>(List<T> data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        // demo only: delete existing file
        if (File.Exists(FileName))
        {
            File.Delete(FileName);
        }

        // ensure directory
        var directory = new FileInfo(FileName).DirectoryName;
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var jsonString = JsonSerializer.Serialize(data, serializerOptions);
        File.WriteAllText(FileName, jsonString);
    }
}