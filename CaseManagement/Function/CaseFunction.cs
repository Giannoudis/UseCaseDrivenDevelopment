/* CaseFunction */

using System;
using System.Text.Json;

namespace UseCaseDrivenDevelopment.CaseManagement.Function;

public abstract class CaseFunction
{
    /// <summary>The function runtime</summary>
    protected dynamic Runtime { get; }

    protected CaseFunction(object runtime)
    {
        Runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
    }

    #region Case Values

    /// <summary>Test for case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>True if the case value is present</returns>
    protected bool HasCaseValue(string caseFieldName, DateTime? evaluationDate = null) =>
        Runtime.HasCaseValue(caseFieldName, evaluationDate);

    /// <summary>Get case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case value</returns>
    protected T GetCaseValue<T>(string caseFieldName, DateTime? evaluationDate = null) =>
        GetCaseValue(caseFieldName, default(T)!, evaluationDate);

    /// <summary>Get case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case value</returns>
    protected T GetCaseValue<T>(string caseFieldName, T defaultValue, DateTime? evaluationDate = null)
    {
        var jsonValue = Runtime.GetCaseValue(caseFieldName, evaluationDate) as string;
        if (string.IsNullOrWhiteSpace(jsonValue))
        {
            return defaultValue;
        }

        var value = JsonSerializer.Deserialize<T>(jsonValue);
        return value ?? defaultValue;
    }

    /// <summary>Get case string value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case string value</returns>
    protected string GetCaseStringValue(string caseFieldName, string? defaultValue = null,
        DateTime? evaluationDate = null) =>
        defaultValue != null
            ? GetCaseValue(caseFieldName, defaultValue, evaluationDate)
            : GetCaseValue<string>(caseFieldName, evaluationDate);

    /// <summary>Get case date time value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case string value</returns>
    protected DateTime? GetCaseDateTimeValue(string caseFieldName, DateTime? defaultValue = null,
        DateTime? evaluationDate = null) =>
        defaultValue != null
            ? GetCaseValue(caseFieldName, defaultValue, evaluationDate)
            : GetCaseValue<DateTime>(caseFieldName, evaluationDate);

    /// <summary>Get case integer value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case integer value</returns>
    protected int? GetCaseIntegerValue(string caseFieldName, int? defaultValue = null,
        DateTime? evaluationDate = null) =>
        defaultValue != null
            ? GetCaseValue(caseFieldName, defaultValue, evaluationDate)
            : GetCaseValue<int>(caseFieldName, evaluationDate);

    /// <summary>Get case boolean value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case boolean value</returns>
    protected bool? GetCaseBooleanValue(string caseFieldName, bool? defaultValue = null,
        DateTime? evaluationDate = null) =>
        defaultValue != null
            ? GetCaseValue(caseFieldName, defaultValue, evaluationDate)
            : GetCaseValue<bool>(caseFieldName, evaluationDate);

    /// <summary>Get case decimal value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case decimal value</returns>
    protected decimal? GetCaseDecimalValue(string caseFieldName, decimal? defaultValue = null,
        DateTime? evaluationDate = null) =>
        defaultValue != null
            ? GetCaseValue(caseFieldName, defaultValue, evaluationDate)
            : GetCaseValue<decimal>(caseFieldName, evaluationDate);

    #endregion
}