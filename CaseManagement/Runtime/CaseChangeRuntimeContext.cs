using System;
using System.Collections.Generic;
using UseCaseDrivenDevelopment.CaseManagement.Model;

namespace UseCaseDrivenDevelopment.CaseManagement.Runtime;

/// <summary>Context for the case runtime</summary>
public class CaseChangeRuntimeContext : CaseRuntimeContext
{
    /// <summary>Working case fields</summary>
    public List<CaseField>? CaseFields { get; set; }

    /// <summary>Working case values </summary>
    public List<CaseValue>? CaseValues { get; set; }

    public CaseChangeRuntimeContext()
    {
    }

    public CaseChangeRuntimeContext(List<CaseField> caseFields,
        List<CaseValue> caseValues, Case @case, DateTime evaluationDate) :
        base(@case, evaluationDate)
    {
        CaseFields = caseFields ?? throw new ArgumentNullException(nameof(caseFields));
        CaseValues = caseValues ?? throw new ArgumentNullException(nameof(caseValues));
    }
}