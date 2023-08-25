using System;
using System.Linq;

namespace UseCaseDrivenDevelopment.CaseManagement.Runtime;

public abstract class CaseChangeRuntime : CaseRuntime
{
    protected new CaseChangeRuntimeContext Context { get; }

    protected CaseChangeRuntime(CaseChangeRuntimeContext context) :
        base(context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Field

    /// <summary>Change the field visibility</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="visible">The visibility state</param>
    public void SetVisibility(string caseFieldName, bool visible)
    {
        var caseField = Context.CaseFields?.FirstOrDefault(x => string.Equals(caseFieldName, x.Name));
        if (caseField != null)
        {
            caseField.Hidden = !visible;
        }
    }

    #endregion

    #region Edit

    /// <summary>Get the case value start date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value start date</returns>
    public DateTime? GetEditStart(string caseFieldName)
    {
        var caseValue = Context.CaseValues?.FirstOrDefault(x => string.Equals(caseFieldName, x.Field));
        return caseValue?.Period.Start;
    }

    /// <summary>Set the case value start date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="start">The case value start date</param>
    public void SetEditStart(string caseFieldName, DateTime start)
    {
        var caseValue = Context.CaseValues?.FirstOrDefault(x => string.Equals(caseFieldName, x.Field));
        if (caseValue != null)
        {
            caseValue.Period.Start = start;
        }
    }

    /// <summary>Get the case value end date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The case value end date</returns>
    public DateTime? GetEditEnd(string caseFieldName)
    {
        var caseValue = Context.CaseValues?.FirstOrDefault(x => string.Equals(caseFieldName, x.Field));
        return caseValue?.Period.End;
    }

    /// <summary>Set the case value end date</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="end">The case value end date</param>
    public void SetEditEnd(string caseFieldName, DateTime? end)
    {
        var caseValue = Context.CaseValues?.FirstOrDefault(x => string.Equals(caseFieldName, x.Field));
        if (caseValue != null)
        {
            caseValue.Period.End = end;
        }
    }

    /// <summary>Test for case edit value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>True if the edit value exists</returns>
    public bool HasEditValue(string caseFieldName) =>
        Context.CaseValues != null &&
        Context.CaseValues.Any(x => string.Equals(caseFieldName, x.Field));

    /// <summary>Get case edit value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <returns>The edit value</returns>
    public string? GetEditValue(string caseFieldName)
    {
        var caseValue = Context.CaseValues?.FirstOrDefault(x => string.Equals(caseFieldName, x.Field));
        return caseValue?.Value;
    }

    /// <summary>Set case edit value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="value">The case value</param>
    public void SetEditValue(string caseFieldName, string value)
    {
        var caseValue = Context.CaseValues?.FirstOrDefault(x => string.Equals(caseFieldName, x.Field));
        if (caseValue != null)
        {
            caseValue.Value = value;
        }
    }

    #endregion
}