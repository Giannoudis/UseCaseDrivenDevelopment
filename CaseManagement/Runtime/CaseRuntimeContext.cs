using System;
using UseCaseDrivenDevelopment.CaseManagement.Model;

namespace UseCaseDrivenDevelopment.CaseManagement.Runtime;

/// <summary>Context for the case runtime</summary>
public class CaseRuntimeContext
{
    /// <summary>Working case</summary>
    public Case? Case { get; set; }

    /// <summary>Evaluation date (default: UTC now)</summary>
    public DateTime EvaluationDate { get; set; }

    public CaseRuntimeContext()
    {
    }

    public CaseRuntimeContext(Case @case, DateTime evaluationDate)
    {
        Case = @case ?? throw new ArgumentNullException(nameof(@case));
        EvaluationDate = evaluationDate;
    }
}