using System.Collections.Generic;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Service;

public class CaseAvailableTestService : CaseTestService
{
    public CaseAvailableTestService() :
        base("Tests\\Available")
    {
    }

    /// <summary>Get all case available tests</summary>
    public List<CaseAvailableTest> GetCaseTests() =>
        GetCaseTests<CaseAvailableTest>();
}