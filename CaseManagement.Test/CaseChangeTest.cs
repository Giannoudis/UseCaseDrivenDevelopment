using System.Collections.Generic;
using UseCaseDrivenDevelopment.CaseManagement.Model;

namespace UseCaseDrivenDevelopment.CaseManagement.Test;

public abstract class CaseChangeTest : CaseTest
{
    /// <summary>Working case fields</summary>
    public List<CaseField>? CaseFields { get; set; }

    /// <summary>Working case values </summary>
    public List<CaseValue>? CaseValues { get; set; }

    /// <summary>Expected case fields</summary>
    public List<CaseField>? ExpectedCaseFields { get; set; }

    /// <summary>Expected case values </summary>
    public List<CaseValue>? ExpectedCaseValues { get; set; }
}