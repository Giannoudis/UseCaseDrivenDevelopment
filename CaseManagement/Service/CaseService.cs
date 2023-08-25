using System;
using System.Collections.Generic;
using System.Linq;
using UseCaseDrivenDevelopment.CaseManagement.Model;

namespace UseCaseDrivenDevelopment.CaseManagement.Service;

public class CaseService : JsonFileService
{
    public CaseService() :
        base("Data\\Cases.json")
    {
    }

    /// <summary>Get all cases</summary>
    public List<Case> GetCases() =>
        ReadCases();

    /// <summary>Get case by name</summary>
    public Case? GetCase(string name)
    {
        var @case = ReadCases().FirstOrDefault(x => string.Equals(x.Name, name));
        return @case;
    }

    /// <summary>Add new case</summary>
    public void AddCase(Case @case)
    {
        if (@case == null)
        {
            throw new ArgumentNullException(nameof(@case));
        }

        if (string.IsNullOrWhiteSpace(@case.Name))
        {
            throw new ArgumentException(nameof(@case.Name));
        }

        var cases = GetCases();
        var existing = cases.FirstOrDefault(x => string.Equals(x.Name, @case.Name));
        if (existing != null)
        {
            cases.Remove(existing);
        }

        cases.Add(@case);
        WriteCases(cases);
    }

    /// <summary>Update case</summary>
    public bool UpdateCase(Case @case)
    {
        if (@case == null)
        {
            throw new ArgumentNullException(nameof(@case));
        }

        if (string.IsNullOrWhiteSpace(@case.Name))
        {
            throw new ArgumentException(nameof(@case.Name));
        }

        var cases = GetCases();
        var existing = cases.FirstOrDefault(x => string.Equals(x.Name, @case.Name));
        if (existing == null)
        {
            return false;
        }

        var index = cases.IndexOf(@case);
        cases.RemoveAt(index);
        cases.Insert(index, @case);
        WriteCases(cases);
        return true;
    }

    /// <summary>Delete case</summary>
    public bool DeleteCase(Case @case)
    {
        if (@case == null)
        {
            throw new ArgumentNullException(nameof(@case));
        }

        if (string.IsNullOrWhiteSpace(@case.Name))
        {
            throw new ArgumentException(nameof(@case.Name));
        }

        var cases = GetCases();
        var existing = cases.FirstOrDefault(x => string.Equals(x.Name, @case.Name));
        if (existing == null)
        {
            return false;
        }

        cases.Remove(existing);
        WriteCases(cases);
        return true;
    }

    private List<Case> ReadCases() =>
        ReadFromFile<Case>();

    private void WriteCases(List<Case> cases) =>
        WriteToFile(cases);
}