using System.Collections.Generic;

namespace UseCaseDrivenDevelopment.CaseManagement.Test.Service;

public class CaseValidateTestService : CaseTestService
{
    public CaseValidateTestService() :
        base("Tests\\Validate")
    {
    }

    /// <summary>Get all case validate tests</summary>
    public List<CaseValidateTest> GetCaseTests() =>
        GetCaseTests<CaseValidateTest>();
}