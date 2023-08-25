/* CaseChangeFunction */

using System;
using System.Text.Json;

namespace UseCaseDrivenDevelopment.CaseManagement.Function;

public abstract class CaseChangeFunction : CaseFunction
{
    protected CaseChangeFunction(object runtime) :
        base(runtime)
    {
    }

    #region Field

    /// <summary>Change the field visibility</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="visible">The visibility state</param>
    public void SetVisibility(string caseFieldName, bool visible) =>
        Runtime.SetVisibility(caseFieldName, visible);

    #endregion

    #region Edit

    /// <summary>Get the case value start date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value start date</returns>
    protected DateTime? GetEditStart(string caseFieldName) =>
        Runtime.GetEditStart(caseFieldName);

    /// <summary>Set the case value start date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="start">The case value start date</param>
    protected void SetEditStart(string caseFieldName, DateTime start) =>
        Runtime.SetEditStart(caseFieldName, start);

    /// <summary>Get the case value end date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value end date</returns>
    protected DateTime? GetEditEnd(string caseFieldName) =>
        Runtime.GetEditEnd(caseFieldName);

    /// <summary>Set the case value end date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="end">The case value end date</param>
    protected void SetEditEnd(string caseFieldName, DateTime? end) =>
        Runtime.SetEditEnd(caseFieldName, end);

    /// <summary>Test for case edit value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>True if the edit value exists</returns>
    protected bool HasEditValue(string caseFieldName) =>
        Runtime.HasEditValue(caseFieldName);

    /// <summary>Get case value edit value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The edit value</returns>
    protected T GetEditValue<T>(string caseFieldName) =>
        GetEditValue<T>(caseFieldName, default!);

    /// <summary>Get case value edit value with a default value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The edit value</returns>
    protected T GetEditValue<T>(string caseFieldName, T defaultValue)
    {
        var jsonValue = Runtime.GetEditValue(caseFieldName) as string;
        if (string.IsNullOrWhiteSpace(jsonValue))
        {
            return defaultValue;
        }

        var value = JsonSerializer.Deserialize<T>(jsonValue);
        return value ?? defaultValue;
    }

    /// <summary>Get case value string value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The edit string value</returns>
    protected string GetStringValue(string caseFieldName, string? defaultValue = null) =>
        defaultValue != null ? GetEditValue(caseFieldName, defaultValue) : GetEditValue<string>(caseFieldName);

    /// <summary>Get case value date time value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The edit date time value</returns>
    protected DateTime? GetDateTimeValue(string caseFieldName, DateTime? defaultValue = null) =>
        defaultValue != null ? GetEditValue(caseFieldName, defaultValue) : GetEditValue<DateTime>(caseFieldName);

    /// <summary>Get case value integer value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The edit integer value</returns>
    protected int? GetIntegerValue(string caseFieldName, int? defaultValue = null) =>
        defaultValue != null ? GetEditValue(caseFieldName, defaultValue) : GetEditValue<int>(caseFieldName);

    /// <summary>Get case value boolean value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The edit boolean value</returns>
    protected bool? GetBooleanValue(string caseFieldName, bool? defaultValue = null) =>
        defaultValue != null ? GetEditValue(caseFieldName, defaultValue) : GetEditValue<bool>(caseFieldName);

    /// <summary>Get case value decimal value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The edit decimal value</returns>
    protected decimal? GetDecimalValue(string caseFieldName, decimal? defaultValue = null) =>
        defaultValue != null ? GetEditValue(caseFieldName, defaultValue) : GetEditValue<decimal>(caseFieldName);

    /// <summary>Test decimal value range</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="minValue">The minimum value</param>
    /// <param name="maxValue">The maximum value</param>
    /// <returns>True is the value is between the minimum and maximum value</returns>
    protected bool EditValueBetween(string caseFieldName, decimal minValue, decimal maxValue)
    {
        var value = GetEditValue<decimal>(caseFieldName);
        return value >= minValue && value <= maxValue;
    }

    /// <summary>Test integer value range</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="minValue">The minimum value</param>
    /// <param name="maxValue">The maximum value</param>
    /// <returns>True is the value is between the minimum and maximum value</returns>
    protected bool EditValueBetween(string caseFieldName, int minValue, int maxValue)
    {
        var value = GetEditValue<int>(caseFieldName);
        return value >= minValue && value <= maxValue;
    }

    /// <summary>Set case value edit value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="value">The case value</param>
    protected void SetEditValue<T>(string caseFieldName, T value)
    {
        var jsonValue = JsonSerializer.Serialize(value);
        Runtime.SetEditValue(caseFieldName, jsonValue);
    }

    /// <summary>Set case value start date as value</summary>
    /// <param name="caseFieldName">The case field name</param>
    protected void SetEditValueStart(string caseFieldName)
    {
        var start = GetEditStart(caseFieldName);
        SetEditValue(caseFieldName, start);
    }

    /// <summary>Set case value end date as value</summary>
    /// <param name="caseFieldName">The case field name</param>
    protected void SetEditValueEnd(string caseFieldName)
    {
        var end = GetEditEnd(caseFieldName);
        SetEditValue(caseFieldName, end);
    }

    #endregion
}