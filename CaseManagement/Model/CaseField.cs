using System.Collections.Generic;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Model;

public class CaseField
{
    /// <summary>The case field name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The case field label, fallback is the name</summary>
    public string? Label { get; set; }

    /// <summary>The case field value type</summary>
    public CaseFieldValueType ValueType { get; set; }

    /// <summary>Field visibility</summary>
    public bool Hidden { get; set; }

    /// <summary>Required field</summary>
    public bool Required { get; set; }

    /// <summary>Field only with period start (without period end)</summary>
    public bool Moment { get; set; }

    /// <summary>Custom field attributes</summary>
    public Dictionary<string, string>? Attributes { get; set; }

    public override string ToString() => Name;
}