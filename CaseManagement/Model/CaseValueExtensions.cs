using System;
using System.Globalization;
using System.Text.Json;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Model;

public static class CaseValueExtensions
{
    /// <summary>Test if moment is within the date period</summary>
    /// <returns>True if the evaluation date is within the period</returns>
    public static bool WithinPeriod(this CaseValue caseValue, DateTime test)
    {
        var end = caseValue.Period.End ?? DateTime.MaxValue;
        return test >= caseValue.Period.Start && test <= end;
    }

    /// <summary>Test for existing case value key using the field and created date</summary>
    /// <returns>True for identical key</returns>
    public static bool EqualKey(this CaseValue caseValue, CaseValue compare) =>
        string.Equals(caseValue.Field, compare.Field) && caseValue.Created == compare.Created;

    /// <summary>Get the formatted case value</summary>
    /// <param name="caseValue">The case value</param>
    /// <param name="valueType">The value type</param>
    /// <returns>The formatted case value</returns>
    public static object? GetCaseValue(this CaseValue caseValue, CaseFieldValueType valueType)
    {
        if (string.IsNullOrWhiteSpace(caseValue.Value))
        {
            return null;
        }

        switch (valueType)
        {
            case CaseFieldValueType.String:
                return JsonSerializer.Deserialize<string>(caseValue.Value);
            case CaseFieldValueType.DateTime:
                return JsonSerializer.Deserialize<DateTime>(caseValue.Value);
            case CaseFieldValueType.Integer:
                return JsonSerializer.Deserialize<int>(caseValue.Value);
            case CaseFieldValueType.Boolean:
                return JsonSerializer.Deserialize<bool>(caseValue.Value);
            case CaseFieldValueType.Decimal:
                return JsonSerializer.Deserialize<decimal>(caseValue.Value);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>Get the formatted case value</summary>
    /// <param name="caseValue">The case value</param>
    /// <param name="valueType">The value type</param>
    /// <returns>The formatted case value</returns>
    public static string? GetCaseValueText(this CaseValue caseValue, CaseFieldValueType valueType)
    {
        if (string.IsNullOrWhiteSpace(caseValue.Value))
        {
            return null;
        }

        switch (valueType)
        {
            case CaseFieldValueType.String:
                return JsonSerializer.Deserialize<string>(caseValue.Value);
            case CaseFieldValueType.DateTime:
                return JsonSerializer.Deserialize<DateTime>(caseValue.Value).ToCompactString();
            case CaseFieldValueType.Integer:
                return JsonSerializer.Deserialize<int>(caseValue.Value).ToString();
            case CaseFieldValueType.Boolean:
                return JsonSerializer.Deserialize<bool>(caseValue.Value).ToString();
            case CaseFieldValueType.Decimal:
                return JsonSerializer.Deserialize<decimal>(caseValue.Value).ToString(SystemSpecification.DecimalFormat,
                    CultureInfo.CurrentUICulture);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}