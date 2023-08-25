using System;
using UseCaseDrivenDevelopment.CaseManagement.Service;

namespace UseCaseDrivenDevelopment.CaseManagement.Runtime;

public abstract class CaseRuntime
{
    private readonly CaseValueService valueService = new();
    protected CaseRuntimeContext Context { get; }

    protected CaseRuntime(CaseRuntimeContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Case Values

    /// <summary>Test for case value</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>True if the case value is present</returns>
    public bool HasCaseValue(string caseFieldName, DateTime? evaluationDate = null) =>
        valueService.GetCaseValue(caseFieldName, evaluationDate ?? Context.EvaluationDate) != null;

    /// <summary>Get case value as JSON</summary>
    /// <param name="caseFieldName">The case field name</param>
    /// <param name="evaluationDate">The evaluation date (default: UTC now)</param>
    /// <returns>The case value as JSON</returns>
    public string? GetCaseValue(string caseFieldName, DateTime? evaluationDate = null)
    {
        var caseValue = valueService.GetCaseValue(caseFieldName, evaluationDate ?? Context.EvaluationDate);
        return caseValue?.Value;
    }

    #endregion
}