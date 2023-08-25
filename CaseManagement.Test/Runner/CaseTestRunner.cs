using System;
using UseCaseDrivenDevelopment.CaseManagement.Compiler;
using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Service;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Runner;

public abstract class CaseTestRunner
{
    public CaseService CaseService { get; }
    public RuntimeService RuntimeService { get; }

    protected CaseTestRunner(CaseService caseService, RuntimeService runtimeService)
    {
        CaseService = caseService ?? throw new ArgumentNullException(nameof(caseService));
        RuntimeService = runtimeService ?? throw new ArgumentNullException(nameof(runtimeService));
    }

    public Case? GetCase(string caseName)
    {
        var @case = CaseService.GetCase(caseName);
        return @case;
    }
}