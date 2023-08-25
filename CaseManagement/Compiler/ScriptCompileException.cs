using System;
using System.Collections.Generic;
using System.Text;

namespace UseCaseDrivenDevelopment.CaseManagement.Compiler;

/// <summary>
/// Payroll script compile exception
/// </summary>
public class ScriptCompileException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="T:UseCaseDrivenDevelopment.CaseManagement.Compiler.ScriptCompileException"></see> class.</summary>
    /// <param name="failures">The diagnostic results</param>
    internal ScriptCompileException(IList<string> failures) :
        base(GetMessage(failures))
    {
    }

    private static string GetMessage(IList<string> failures)
    {
        if (failures.Count == 1)
        {
            return failures[0];
        }

        var buffer = new StringBuilder();
        buffer.AppendLine($"{failures.Count} compile errors:");
        foreach (var failure in failures)
        {
            buffer.AppendLine(failure);
        }

        return buffer.ToString();
    }
}