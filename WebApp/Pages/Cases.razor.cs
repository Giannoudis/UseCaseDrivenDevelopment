using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using UseCaseDrivenDevelopment.CaseManagement.Compiler;
using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Service;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.WebApp.Pages;

public partial class Cases
{
    private MudForm? caseForm;
    private List<Case>? availableCases;
    private List<CaseField>? caseFields;
    private List<CaseValue>? caseValues;
    private Case? EditCase { get; set; }
    private Dictionary<CaseField, CaseValue> EditFields { get; } = new();
    private string? ErrorMessage { get; set; }
    private bool ShowValidation { get; set; } = true;
    private DateTime EvaluationDate { get; set; } = DateTime.Today;

    private DateTime? PickerEvaluationDate
    {
        get => EvaluationDate;
        set
        {
            EvaluationDate = value ?? DateTime.UtcNow;
            SetupData();
        }
    }

    [Inject] private CaseService? CaseService { get; set; }
    [Inject] private CaseFieldService? CaseFieldService { get; set; }
    [Inject] private CaseValueService? CaseValueService { get; set; }
    [Inject] private RuntimeService? RuntimeService { get; set; }
    [Inject] protected NavigationManager? NavigationManager { get; set; }
    [Inject] protected IJSRuntime? JsRuntime { get; set; }

    private string GetEvaluationDay()
    {
        var days = (EvaluationDate.Date - DateTime.Now.Date).Days;
        switch (days)
        {
            case 0:
                return string.Empty;
            case -1:
                return $" ({days} Day)";
            case 1:
                return $" (+{days} Day)";
            default:
                return days > 0 ? $" (+{days} Days)" : $" ({days} Days)";
        }
    }

    private CaseValue? GetCaseValue(CaseField caseField)
    {
        if (CaseValueService == null)
        {
            return null;
        }

        var value = CaseValueService.GetCaseValue(caseField.Name, EvaluationDate);
        return value;
    }

    private string GetCaseValueCreatedText(CaseField caseField)
    {
        var caseValue = GetCaseValue(caseField);
        return caseValue != null ? caseValue.Created.ToCompactString() : string.Empty;
    }

    private string GetCaseValueStartText(CaseField caseField)
    {
        var caseValue = GetCaseValue(caseField);
        return caseValue != null ? caseValue.Period.Start.ToCompactString() : string.Empty;
    }

    private string GetCaseValueEndText(CaseField caseField)
    {
        var caseValue = GetCaseValue(caseField);
        return caseValue != null && caseValue.Period.End.HasValue
            ? caseValue.Period.End.Value.ToCompactString()
            : string.Empty;
    }

    private string? GetCaseValueText(CaseField caseField)
    {
        var caseValue = GetCaseValue(caseField);
        if (caseValue == null || string.IsNullOrWhiteSpace(caseValue.Value))
        {
            return null;
        }

        return caseValue.GetCaseValueText(caseField.ValueType);
    }

    private void StartCase(Case @case)
    {
        if (EditCase != null || RuntimeService == null ||
            CaseFieldService == null || CaseValueService == null ||
            !@case.Fields.Any())
        {
            return;
        }

        ResetMessages();

        try
        {
            EditFields.Clear();
            foreach (var field in @case.Fields)
            {
                var caseField = CaseFieldService.GetCaseField(field);
                if (caseField == null)
                {
                    ErrorMessage = $"Unknown case field {field}.";
                    continue;
                }

                // set the current value
                var caseValue = CaseValueService.GetCaseValue(caseField.Name);
                EditFields.Add(caseField, new CaseValue
                {
                    Created = EvaluationDate,
                    Field = caseField.Name,
                    Value = caseValue?.Value
                });
            }

            // missing edit fields
            if (!EditFields.Any())
            {
                ErrorMessage = $"Case {@case.Label ?? @case.Name} without fields.";
                return;
            }

            // edit fields
            RuntimeService.BuildCase(new(
                caseFields: EditFields.Keys.ToList(),
                caseValues: EditFields.Values.ToList(),
                @case: @case,
                evaluationDate: EvaluationDate));

            // activate case editor
            EditCase = @case;
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
        }
    }

    private async Task SubmitCaseEditAsync()
    {
        if (RuntimeService == null || CaseValueService == null ||
            EditCase == null || caseForm == null)
        {
            return;
        }

        // form validation
        caseForm.ResetValidation();
        await caseForm.Validate();
        if (!caseForm.IsValid)
        {
            return;
        }

        ResetMessages();

        try
        {
            var valid = RuntimeService.ValidateCase(new(
                @case: EditCase,
                caseFields: EditFields.Keys.ToList(),
                caseValues: EditFields.Values.ToList(),
                evaluationDate: EvaluationDate));
            if (!valid)
            {
                ErrorMessage = $"Invalid case {EditCase.Label}";
                return;
            }

            // create case values
            var values = new List<CaseValue>();
            foreach (var editField in EditFields)
            {
                // ignore empty and hidden field
                if (editField.Key.Hidden && string.IsNullOrWhiteSpace(editField.Value.Value))
                {
                    continue;
                }

                values.Add(editField.Value);
            }

            if (!values.Any())
            {
                ErrorMessage = $"Case {EditCase.Label} without values";
                return;
            }

            CaseValueService.AddCaseValues(values);

            // reset editor
            EditCase = null;
            ReloadPage();
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
        }
    }

    private void CancelCaseEdit()
    {
        ResetMessages();
        EditCase = null;
    }

    private async Task CaseValueDataChanged()
    {
        if (EditCase == null || RuntimeService == null)
        {
            return;
        }

        // rebuild case fields with values
        RuntimeService.BuildCase(new(
            caseFields: EditFields.Keys.ToList(),
            caseValues: EditFields.Values.ToList(),
            @case: EditCase,
            evaluationDate: EvaluationDate));

        // invoke state change async to updated dependent case values!
        await InvokeAsync(StateHasChanged);
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

            if (CaseService != null && RuntimeService != null)
            {
                availableCases = RuntimeService.GetAvailableCases(CaseService.GetCases(), EvaluationDate);
            }

            if (CaseFieldService != null)
            {
                caseFields = CaseFieldService.GetCaseFields();
            }

            if (CaseValueService != null)
            {
                caseValues = CaseValueService.GetCaseValues(EvaluationDate);
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