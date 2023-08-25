using System;
using System.Collections.Generic;
using System.Linq;
using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Service;

public class CaseValueService : JsonFileService
{
    public CaseValueService() :
        base("Data\\CaseValues.json")
    {
    }

    /// <summary>Get all case values</summary>
    public List<CaseValue> GetCaseValues(DateTime? evaluationDate = null)
    {
        var caseValues = ReadCaseValues();
        if (evaluationDate.HasValue)
        {
            caseValues = caseValues.
                // remove values created after the evaluation date
                Where(x => x.Created <= evaluationDate &&
                           // remove outside periods
                           x.WithinPeriod(evaluationDate.Value)).ToList();
        }

        return caseValues;
    }

    /// <summary>Get case field values</summary>
    public List<CaseValue> GetCaseValues(string caseFieldName, DateTime? evaluationDate = null) =>
        GetCaseValues(evaluationDate).Where(x => string.Equals(x.Field, caseFieldName)).ToList();

    /// <summary>Get case value</summary>
    public CaseValue? GetCaseValue(string caseFieldName, DateTime? evaluationDate = null)
    {
        if (string.IsNullOrWhiteSpace(caseFieldName))
        {
            throw new ArgumentException(nameof(caseFieldName));
        }

        evaluationDate ??= DateTime.UtcNow;

        var caseValues = GetCaseValues(caseFieldName, evaluationDate);
        if (!caseValues.Any())
        {
            return null;
        }

        var timeValues = caseValues.
            // remove values created after the evaluation date
            Where(x => x.Created <= evaluationDate &&
                       // remove outside periods
                       x.WithinPeriod(evaluationDate.Value)).ToList();
        if (!timeValues.Any())
        {
            return default;
        }

        // select the evaluated value (last created)
        var timeValue = timeValues.OrderByDescending(x => x.Created).First();
        return timeValue;
    }

    /// <summary>Add case values</summary>
    public void AddCaseValues(IEnumerable<CaseValue> caseValues)
    {
        if (caseValues == null)
        {
            throw new ArgumentNullException(nameof(caseValues));
        }

        var existingCaseValues = ReadCaseValues();
        var caseValuesList = caseValues as CaseValue[] ?? caseValues.ToArray();
        foreach (var caseValue in caseValuesList)
        {
            var existing = existingCaseValues.FirstOrDefault(x => x.EqualKey(caseValue));
            if (existing != null)
            {
                throw new ScriptException("Invalid case value");
            }

            existingCaseValues.Add(caseValue);
        }

        WriteCaseValues(existingCaseValues);
    }

    /// <summary>Clear all case values</summary>
    public void ClearCaseValues() =>
        WriteCaseValues(new());

    private List<CaseValue> ReadCaseValues() =>
        ReadFromFile<CaseValue>();

    private void WriteCaseValues(List<CaseValue> caseValues)
    {
        var groupedCaseValues = caseValues.GroupBy(x => new { x.Field, x.Created });
        var multiGroups = groupedCaseValues.Where(x => x.Count() > 1).ToList();
        if (multiGroups.Any())
        {
            var first = multiGroups.First();
            throw new InvalidOperationException("Multiple case value with same creation date: " +
                                                $"{first.Key.Field} with data {first.Key.Created}");
        }

        WriteToFile(caseValues);
    }
}