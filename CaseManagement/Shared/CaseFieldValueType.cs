using System.Text.Json.Serialization;

namespace UseCaseDrivenDevelopment.CaseManagement.Shared;

/// <summary>The case field value type</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CaseFieldValueType
{
    /// <summary>Value type string</summary>
    String,

    /// <summary>Value type date time</summary>
    DateTime,

    /// <summary>Value type integer</summary>
    Integer,

    /// <summary>Value type boolean</summary>
    Boolean,

    /// <summary>Value type decimal</summary>
    Decimal
}