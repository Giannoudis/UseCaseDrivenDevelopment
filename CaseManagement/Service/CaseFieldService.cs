using System;
using System.Collections.Generic;
using System.Linq;
using UseCaseDrivenDevelopment.CaseManagement.Model;

namespace UseCaseDrivenDevelopment.CaseManagement.Service;

public class CaseFieldService : JsonFileService
{
    public CaseFieldService() :
        base("Data\\CaseFields.json")
    {
    }

    /// <summary>Get all case fields</summary>
    public List<CaseField> GetCaseFields() =>
        ReadCaseFields();

    /// <summary>Get case field by name</summary>
    public CaseField? GetCaseField(string name)
    {
        var caseFields = ReadCaseFields();
        var caseField = caseFields.FirstOrDefault(x => string.Equals(x.Name, name));
        return caseField;
    }

    /// <summary>Get new case field</summary>
    public void AddCaseField(CaseField caseField)
    {
        if (caseField == null)
        {
            throw new ArgumentNullException(nameof(caseField));
        }

        if (string.IsNullOrWhiteSpace(caseField.Name))
        {
            throw new ArgumentException(nameof(caseField.Name));
        }

        var caseFields = GetCaseFields();
        var existing = caseFields.FirstOrDefault(x => string.Equals(x.Name, caseField.Name));
        if (existing != null)
        {
            caseFields.Remove(existing);
        }

        caseFields.Add(caseField);
        WriteCaseFields(caseFields);
    }

    /// <summary>Update case field</summary>
    public bool UpdateCaseField(CaseField caseField)
    {
        if (caseField == null)
        {
            throw new ArgumentNullException(nameof(caseField));
        }

        if (string.IsNullOrWhiteSpace(caseField.Name))
        {
            throw new ArgumentException(nameof(caseField.Name));
        }

        var caseFields = GetCaseFields();
        var existing = caseFields.FirstOrDefault(x => string.Equals(x.Name, caseField.Name));
        if (existing == null)
        {
            return false;
        }

        var index = caseFields.IndexOf(caseField);
        caseFields.RemoveAt(index);
        caseFields.Insert(index, caseField);
        WriteCaseFields(caseFields);
        return true;
    }

    /// <summary>Delete case field</summary>
    public bool DeleteCaseField(CaseField caseField)
    {
        if (caseField == null)
        {
            throw new ArgumentNullException(nameof(caseField));
        }

        if (string.IsNullOrWhiteSpace(caseField.Name))
        {
            throw new ArgumentException(nameof(caseField.Name));
        }

        var caseFields = GetCaseFields();
        var existing = caseFields.FirstOrDefault(x => string.Equals(x.Name, caseField.Name));
        if (existing == null)
        {
            return false;
        }

        caseFields.Remove(existing);
        WriteCaseFields(caseFields);
        return true;
    }

    private List<CaseField> ReadCaseFields() =>
        ReadFromFile<CaseField>();

    private void WriteCaseFields(List<CaseField> caseFields) =>
        WriteToFile(caseFields);
}