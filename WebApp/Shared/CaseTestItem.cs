using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Test;

namespace UseCaseDrivenDevelopment.WebApp.Shared;

internal sealed class CaseTestItem
{
    internal CaseTest Test { get; }

    internal CaseTestItem(CaseTest test)
    {
        Test = test;
        TestType = test.GetType();
        var typeName = test.GetType().Name;
        if (typeName.StartsWith(nameof(Case)))
        {
            typeName = typeName.Substring(nameof(Case).Length);
        }
        if (typeName.EndsWith(nameof(Test)))
        {
            typeName = typeName.Substring(0, typeName.Length - nameof(Test).Length);
        }
        TypeName = typeName;
    }

    internal Type TestType { get; }
    internal string TypeName { get; }
    internal string Name => Test.Name;
    internal string CaseName => Test.CaseName;

    internal Result Result { get; } = new();
}