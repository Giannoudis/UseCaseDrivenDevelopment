using System.Collections.Generic;

namespace UseCaseDrivenDevelopment.CaseManagement.Model;

public class Case
{
    /// <summary>The case name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The case label, fallback is the name</summary>
    public string? Label { get; set; }

    /// <summary>The case field names</summary>
    public List<string> Fields { get; set; } = new();

    /// <summary>The case available expression (C#)</summary>
    public string? AvailableExpression { get; set; }

    /// <summary>The case build expression (C#)</summary>
    public string? BuildExpression { get; set; }

    /// <summary>The case validate expression (C#)</summary>
    public string? ValidateExpression { get; set; }

    public override string ToString() => Name;
}