using System.Text.Json;

namespace UseCaseDrivenDevelopment.CaseManagement.Model;

public static class CaseFieldExtensions
{
    /// <summary>Get case field attribute</summary>
    /// <returns>The attribute value</returns>
    public static T? GetAttribute<T>(this CaseField caseField, string attributeName) =>
        GetAttribute<T>(caseField, attributeName, default!);

    /// <summary>Get case field attribute</summary>
    /// <returns>The attribute value</returns>
    public static T? GetAttribute<T>(this CaseField caseField, string attributeName, T defaultValue)
    {
        if (caseField.Attributes == null || !caseField.Attributes.ContainsKey(attributeName))
        {
            return defaultValue;
        }

        return JsonSerializer.Deserialize<T>(caseField.Attributes[attributeName]);
    }
}