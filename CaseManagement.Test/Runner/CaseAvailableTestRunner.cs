using System;
using UseCaseDrivenDevelopment.CaseManagement.Compiler;
using UseCaseDrivenDevelopment.CaseManagement.Runtime;
using UseCaseDrivenDevelopment.CaseManagement.Service;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Runner;

public class CaseAvailableTestRunner : CaseTestRunner
{
    public CaseAvailableTestRunner(CaseService caseService, RuntimeService runtimeService) :
        base(caseService, runtimeService)
    {
    }

    public Result? Run(CaseAvailableTest test)
    {
        if (test == null)
        {
            throw new ArgumentNullException(nameof(test));
        }

        var @case = GetCase(test.CaseName);
        if (@case == null)
        {
            return null;
        }

        var context = new CaseRuntimeContext
        {
            Case = @case,
            EvaluationDate = test.EvaluationDate
        };

        var availableCase = RuntimeService.GetAvailableCase(context);

        // test result
        var validTest = availableCase == test.ExpectedAvailable;
        if (!validTest)
        {
            return new Result
            {
                Executed = DateTime.Now,
                Valid = validTest,
                Source = "Available",
                Expected = test.ExpectedAvailable.ToString(),
                Actual = availableCase.ToString()
            };
        }

        // test ok
        return new()
        {
            Executed = DateTime.Now,
            Valid = true
        };
    }
}