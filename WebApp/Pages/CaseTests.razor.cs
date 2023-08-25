using Microsoft.AspNetCore.Components;
using UseCaseDrivenDevelopment.CaseManagement.Compiler;
using UseCaseDrivenDevelopment.CaseManagement.Service;
using UseCaseDrivenDevelopment.CaseManagement.Test;
using UseCaseDrivenDevelopment.CaseManagement.Test.Runner;
using UseCaseDrivenDevelopment.CaseManagement.Test.Service;
using UseCaseDrivenDevelopment.WebApp.Shared;

namespace UseCaseDrivenDevelopment.WebApp.Pages;

public partial class CaseTests
{
    private List<CaseTestItem>? caseTests;
    private string? ErrorMessage { get; set; }
    private bool TestRunning;

    [Inject] private CaseService? CaseService { get; set; }
    [Inject] private CaseFieldService? CaseFieldService { get; set; }
    [Inject] private RuntimeService? RuntimeService { get; set; }
    [Inject] private CaseAvailableTestService? CaseAvailableTestService { get; set; }
    [Inject] private CaseBuildTestService? CaseBuildTestService { get; set; }
    [Inject] private CaseValidateTestService? CaseValidateTestService { get; set; }

    private async Task StartTest(CaseTestItem testItem, bool indicator = true)
    {
        if (CaseService == null || CaseFieldService == null || RuntimeService == null)
        {
            return;
        }

        try
        {
            if (indicator)
            {
                await ChangeRunningState(true);
            }

            Result? testResult = null;
            switch (testItem.TestType.Name)
            {
                case nameof(CaseAvailableTest):
                    testResult = new CaseAvailableTestRunner(
                        CaseService, RuntimeService).Run((CaseAvailableTest)testItem.Test);
                    break;
                case nameof(CaseBuildTest):
                    testResult = new CaseBuildTestRunner(
                        CaseService, CaseFieldService, RuntimeService).Run((CaseBuildTest)testItem.Test);
                    break;
                case nameof(CaseValidateTest):
                    testResult = new CaseValidateTestRunner(
                        CaseService, CaseFieldService, RuntimeService).Run((CaseValidateTest)testItem.Test);
                    break;
            }

            if (testResult != null)
            {
                testItem.Result.Apply(testResult);
            }
            else
            {
                testItem.Result.Reset();
            }
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
        }
        finally
        {
            if (indicator)
            {
                await ChangeRunningState(false);
            }
        }
    }

    private async Task StartAllTests()
    {
        if (caseTests == null)
        {
            return;
        }

        try
        {
            await ChangeRunningState(true);
            foreach (var caseTest in caseTests)
            {
                await StartTest(caseTest, false);
            }
        }
        finally
        {
            await ChangeRunningState(false);
        }
    }

    private async Task ChangeRunningState(bool running)
    {
        TestRunning = running;
        StateHasChanged();
        await Task.Delay(10);
    }

    private void ResetMessages()
    {
        ErrorMessage = null;
    }

    private void SetupData()
    {
        try
        {
            ResetMessages();

            caseTests ??= new();
            caseTests.Clear();
            if (CaseAvailableTestService != null)
            {
                AddTests(CaseAvailableTestService.GetCaseTests(), caseTests);
            }

            if (CaseBuildTestService != null)
            {
                AddTests(CaseBuildTestService.GetCaseTests(), caseTests);
            }

            if (CaseValidateTestService != null)
            {
                AddTests(CaseValidateTestService.GetCaseTests(), caseTests);
            }
        }
        catch (Exception exception)
        {
            ErrorMessage = exception.GetBaseException().Message;
        }
    }

    private static void AddTests(IEnumerable<CaseTest> tests, List<CaseTestItem> target)
    {
        foreach (var test in tests)
        {
            target.Add(new(test));
        }
    }

    protected override void OnInitialized()
    {
        SetupData();
        base.OnInitialized();
    }
}