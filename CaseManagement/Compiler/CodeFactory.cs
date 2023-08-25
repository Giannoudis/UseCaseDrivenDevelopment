using System;
using System.Collections.Generic;
using System.Reflection;
using UseCaseDrivenDevelopment.CaseManagement.Function;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Compiler;

/// <summary>
/// Script files loaded from the scripting core assembly
/// </summary>
internal static class CodeFactory
{
    private static Assembly Assembly { get; }
    private static Dictionary<string, string> CodeFiles { get; } = new();

    static CodeFactory()
    {
        Assembly = typeof(CodeFactory).Assembly;
    }

    /// <summary>
    /// The embedded source files
    /// </summary>
    internal static readonly string[] SourceFiles =
    {
        // function
        $"{nameof(Function)}\\{nameof(CaseFunction)}.cs",
        $"{nameof(Function)}\\{nameof(CaseChangeFunction)}.cs"
    };

    /// <summary>
    /// The case available embedded template file
    /// </summary>
    internal static readonly string CaseAvailableTemplate = $"{nameof(Function)}\\{nameof(CaseAvailableFunction)}.cs";

    /// <summary>
    /// The case build embedded template file
    /// </summary>
    internal static readonly string CaseBuildTemplate = $"{nameof(Function)}\\{nameof(CaseBuildFunction)}.cs";

    /// <summary>
    /// The case validate embedded template file
    /// </summary>
    internal static readonly string CaseValidateTemplate = $"{nameof(Function)}\\{nameof(CaseValidateFunction)}.cs";

    /// <summary>
    /// Gets embedded source code
    /// </summary>
    /// <param name="resourceName">Name of the resource</param>
    /// <returns>The source code</returns>
    internal static string GetEmbeddedCodeFile(string resourceName)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
        {
            throw new ArgumentException(nameof(resourceName));
        }

        resourceName = resourceName.EnsureEnd(".cs");

        string codeFile;
        if (CodeFiles.TryGetValue(resourceName, out var file))
        {
            codeFile = file;
        }
        else
        {
            codeFile = Assembly.GetEmbeddedFile(resourceName);
            CodeFiles.Add(resourceName, codeFile);
        }

        return codeFile;
    }
}