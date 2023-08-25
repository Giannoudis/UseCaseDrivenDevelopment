using System.Collections.Generic;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Service;

public class CaseBuildTestService : CaseTestService
{
    public CaseBuildTestService() :
        base("Tests\\Build")
    {
    }

    /// <summary>Get all case build tests</summary>
    public List<CaseBuildTest> GetCaseTests() =>
        GetCaseTests<CaseBuildTest>();
}