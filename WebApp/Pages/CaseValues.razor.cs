using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Service;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.WebApp.Pages;

public partial class CaseValues
{
    private List<CaseField>? caseFields;
    private List<CaseValue>? caseValues;
    private string? ErrorMessage { get; set; }

    [Inject] private CaseFieldService? CaseFieldService { get; set; }
    [Inject] private CaseValueService? CaseValueService { get; set; }
    [Inject] protected NavigationManager? NavigationManager { get; set; }
    [Inject] protected IJSRuntime? JsRuntime { get; set; }

    private string GetCaseFieldText(string caseFieldName)
    {
        var caseField = GetCaseField(caseFieldName);
        return caseField != null ? caseField.Label ?? caseField.Name : string.Empty;
    }

    private string GetCaseValueCreatedText(CaseValue caseValue)
    {
        var caseField = GetCaseField(caseValue.Field);
        return caseField != null ? caseValue.Created.ToCompactString() : string.Empty;
    }

    private string GetCaseValueStartText(CaseValue caseValue)
    {
        var caseField = GetCaseField(caseValue.Field);
        return caseField != null ? caseValue.Period.Start.ToCompactString() : string.Empty;
    }

    private string GetCaseValueEndText(CaseValue caseValue)
    {
        var caseField = GetCaseField(caseValue.Field);
        return caseField != null && caseValue.Period.End.HasValue
            ? caseValue.Period.End.Value.ToCompactString()
            : string.Empty;
    }

    private string? GetCaseValueText(CaseValue caseValue)
    {
        var caseField = GetCaseField(caseValue.Field);
        if (caseField == null || string.IsNullOrWhiteSpace(caseValue.Value))
        {
            return null;
        }

        switch (caseField.ValueType)
        {
            case CaseFieldValueType.String:
                return JsonSerializer.Deserialize<string>(caseValue.Value);
            case CaseFieldValueType.DateTime:
                return JsonSerializer.Deserialize<DateTime>(caseValue.Value).ToCompactString();
            case CaseFieldValueType.Integer:
                return JsonSerializer.Deserialize<int>(caseValue.Value).ToString();
            case CaseFieldValueType.Boolean:
                return JsonSerializer.Deserialize<bool>(caseValue.Value).ToString();
            case CaseFieldValueType.Decimal:
                return JsonSerializer.Deserialize<decimal>(caseValue.Value).ToString(SystemSpecification.DecimalFormat,
                    CultureInfo.CurrentUICulture);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private CaseField? GetCaseField(string caseFieldName)
    {
        if (caseFields == null)
        {
            return null;
        }

        return caseFields.FirstOrDefault(x => string.Equals(x.Name, caseFieldName));
    }

    private async Task ClearCaseValuesAsync()
    {
        if (CaseValueService == null)
        {
            return;
        }

        if (JsRuntime != null)
        {
            var confirmed = await JsRuntime.InvokeAsync<bool>("confirm", "Delete all case values?");
            if (!confirmed)
            {
                return;
            }
        }

        ResetMessages();

        try
        {
            // clear all case values
            CaseValueService.ClearCaseValues();
            ReloadPage();
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
        }
    }

    private void ResetMessages()
    {
        ErrorMessage = null;
    }

    private void ReloadPage()
    {
        if (NavigationManager != null)
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }

    private void SetupData()
    {
        try
        {
            ResetMessages();

            if (CaseFieldService != null)
            {
                caseFields = CaseFieldService.GetCaseFields();
            }

            if (CaseValueService != null)
            {
                caseValues = CaseValueService.GetCaseValues();
            }
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
        }
    }

    protected override void OnInitialized()
    {
        SetupData();
        base.OnInitialized();
    }
}