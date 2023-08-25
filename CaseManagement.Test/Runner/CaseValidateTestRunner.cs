using System;
using System.Collections.Generic;
using UseCaseDrivenDevelopment.CaseManagement.Compiler;
using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Runtime;
using UseCaseDrivenDevelopment.CaseManagement.Service;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Runner;

public class CaseValidateTestRunner : CaseChangeTestRunner
{
    public CaseValidateTestRunner(CaseService caseService, CaseFieldService caseFieldService,
        RuntimeService runtimeService) :
        base(caseService, caseFieldService, runtimeService)
    {
    }

    public Result? Run(CaseValidateTest test)
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

        var context = new CaseChangeRuntimeContext
        {
            Case = @case,
            EvaluationDate = test.EvaluationDate,
            // copy fields
            CaseFields = test.CaseFields != null ? new List<CaseField>(test.CaseFields) : null,
            // copy values
            CaseValues = test.CaseValues != null ? new List<CaseValue>(test.CaseValues) : null,
        };

        var validCase = RuntimeService.ValidateCase(context);

        // validation result
        var validTest = validCase == test.ExpectedValid;
        if (!validTest)
        {
            return new()
            {
                Executed = DateTime.Now,
                Valid = validTest,
                Source = "Validate",
                Expected = test.ExpectedValid.ToString(),
                Actual = validCase.ToString()
            };
        }

        // test fields and values
        var expectedContext = new CaseChangeRuntimeContext
        {
            Case = @case,
            EvaluationDate = test.EvaluationDate,
            // copy fields
            CaseFields = test.ExpectedCaseFields != null ? new List<CaseField>(test.ExpectedCaseFields) :
                test.CaseFields != null ? new List<CaseField>(test.CaseFields) : null,
            // copy values
            CaseValues = test.ExpectedCaseValues != null ? new List<CaseValue>(test.ExpectedCaseValues) :
                test.CaseValues != null ? new List<CaseValue>(test.CaseValues) : null,
        };
        var result = TestResult(expectedContext, context);
        if (result != null)
        {
            return result;
        }

        // test ok
        return new()
        {
            Executed = DateTime.Now,
            Valid = true
        };
    }
}