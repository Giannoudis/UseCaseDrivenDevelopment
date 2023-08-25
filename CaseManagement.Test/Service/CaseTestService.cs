using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Service;

public abstract class CaseTestService
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private string TestDirectory { get; }

    protected CaseTestService(string baseDirectory)
    {
        // test folder
        var assembly = Assembly.GetEntryAssembly();
        var assemblyLocation = assembly != null ? Path.GetDirectoryName(assembly.Location) : null;
        TestDirectory = assemblyLocation != null ? Path.Combine(assemblyLocation, baseDirectory) : baseDirectory;
    }

    /// <summary>Get all case tests from the test directory</summary>
    protected List<T> GetCaseTests<T>() where T : CaseTest
    {
        var caseTests = new List<T>();

        if (Directory.Exists(TestDirectory))
        {
            var testFiles = Directory.GetFiles(TestDirectory);
            foreach (var testFile in testFiles)
            {
                caseTests.AddRange(ReadFromFile<T>(testFile));
            }
        }

        return caseTests;
    }

    /// <summary>Read test from file</summary>
    private List<T> ReadFromFile<T>(string fileName) where T : CaseTest
    {
        if (!File.Exists(fileName))
        {
            return new();
        }

        var caseTests = JsonSerializer.Deserialize<List<T>>(
            File.ReadAllText(fileName),
            serializerOptions);
        return caseTests ?? new();
    }
}