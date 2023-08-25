using System;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Model;

public class CaseValue
{
    /// <summary>The creation date</summary>
    public DateTime Created { get; set; }

    /// <summary>The case value period</summary>
    public ValuePeriod Period { get; set; } = new();

    /// <summary>The case field name</summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>The case value as JSON</summary>
    public string? Value { get; set; }

    public override string ToString() => $"{Field}={Value}";
}