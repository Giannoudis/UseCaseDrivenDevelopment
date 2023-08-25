using System;
using System.Linq;
using UseCaseDrivenDevelopment.CaseManagement.Compiler;
using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Runtime;
using UseCaseDrivenDevelopment.CaseManagement.Service;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Runner;

public abstract class CaseChangeTestRunner : CaseTestRunner
{
    public CaseFieldService CaseFieldService { get; }

    protected CaseChangeTestRunner(CaseService caseService, CaseFieldService caseFieldService,
        RuntimeService runtimeService) :
        base(caseService, runtimeService)
    {
        CaseFieldService = caseFieldService ?? throw new ArgumentNullException(nameof(caseFieldService));
    }

    protected Result? TestResult(CaseChangeRuntimeContext expected, CaseChangeRuntimeContext actual)
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw new ArgumentNullException(nameof(actual));
        }

        // case fields
        if (expected.CaseFields != null)
        {
            foreach (var expectedField in expected.CaseFields)
            {
                var actualField = actual.CaseFields?.FirstOrDefault(x => string.Equals(x.Name, expectedField.Name));
                if (actualField == null)
                {
                    // missing field
                    return new()
                    {
                        Executed = DateTime.Now,
                        Valid = false,
                        Source = $"Missing field {expectedField.Name}"
                    };
                }

                var result = TestCaseField(expectedField, actualField);
                if (result != null)
                {
                    return result;
                }
            }
        }

        // case values
        if (expected.CaseValues != null)
        {
            foreach (var expectedValue in expected.CaseValues)
            {
                var actualValue = actual.CaseValues?.FirstOrDefault(x => string.Equals(x.Field, expectedValue.Field));
                if (actualValue == null)
                {
                    // missing value
                    return new()
                    {
                        Executed = DateTime.Now,
                        Valid = false,
                        Source = $"Missing value {expectedValue.Field}"
                    };
                }

                var caseField = CaseFieldService.GetCaseField(actualValue.Field);
                if (caseField == null)
                {
                    throw new ScriptException($"Missing case field {actualValue.Field}");
                }

                var result = TestCaseValue(caseField, expectedValue, actualValue);
                if (result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }

    protected Result? TestCaseField(CaseField expected, CaseField actual)
    {
        // case field required
        if (expected.Required != actual.Required)
        {
            return new()
            {
                Executed = DateTime.Now,
                Valid = false,
                Source = $"{expected.Name}.Required",
                Expected = expected.Required.ToString(),
                Actual = actual.Required.ToString()
            };
        }

        // case field hidden
        if (expected.Hidden != actual.Hidden)
        {
            return new()
            {
                Executed = DateTime.Now,
                Valid = false,
                Source = $"{expected.Name}.Hidden",
                Expected = expected.Hidden.ToString(),
                Actual = actual.Hidden.ToString()
            };
        }

        return null;
    }

    protected Result? TestCaseValue(CaseField caseField, CaseValue expected, CaseValue actual)
    {
        // case value created
        if (expected.Created != actual.Created)
        {
            return new()
            {
                Executed = DateTime.Now,
                Valid = false,
                Source = $"{expected.Field}.Created",
                Expected = expected.Created.ToCompactString(),
                Actual = actual.Created.ToCompactString()
            };
        }

        // case value start
        if (expected.Period.Start != actual.Period.Start)
        {
            return new()
            {
                Executed = DateTime.Now,
                Valid = false,
                Source = $"{expected.Field}.Start",
                Expected = expected.Period.Start.ToCompactString(),
                Actual = actual.Period.Start.ToCompactString()
            };
        }

        // case value end
        if (expected.Period.End != actual.Period.End)
        {
            return new()
            {
                Executed = DateTime.Now,
                Valid = false,
                Source = $"{expected.Field}.End",
                Expected = expected.Period.End?.ToCompactString(),
                Actual = actual.Period.End?.ToCompactString()
            };
        }

        // case value
        var expectedValue = expected.GetCaseValue(caseField.ValueType);
        var actualValue = actual.GetCaseValue(caseField.ValueType);
        if (!Equals(expectedValue, actualValue))
        {
            return new()
            {
                Executed = DateTime.Now,
                Valid = false,
                Source = $"{expected.Field}.Value",
                Expected = expected.GetCaseValueText(caseField.ValueType),
                Actual = actual.GetCaseValueText(caseField.ValueType)
            };
        }

        return null;
    }
}