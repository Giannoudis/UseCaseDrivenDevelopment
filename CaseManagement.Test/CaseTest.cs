using System;

namespace UseCaseDrivenDevelopment.CaseManagement.Test;

public abstract class CaseTest
{
    /// <summary>The test name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The case test name</summary>
    public string CaseName { get; set; } = string.Empty;

    /// <summary>Evaluation date (default: UTC now)</summary>
    public DateTime EvaluationDate { get; set; } = DateTime.Now;
}