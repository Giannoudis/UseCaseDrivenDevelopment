using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UseCaseDrivenDevelopment.CaseManagement.Function;
using UseCaseDrivenDevelopment.CaseManagement.Model;
using UseCaseDrivenDevelopment.CaseManagement.Runtime;
using UseCaseDrivenDevelopment.CaseManagement.Service;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Compiler;

public class RuntimeService
{
    /// <summary>
    /// Get all available cases
    /// </summary>
    /// <param name="cases"></param>
    /// <param name="evaluationDate">The value evaluation date</param>
    /// <returns></returns>
    public List<Case> GetAvailableCases(IEnumerable<Case> cases, DateTime evaluationDate)
    {
        var availableCases = cases.Where(@case => GetAvailableCase(new(@case, evaluationDate))).ToList();
        return availableCases;
    }

    /// <summary>
    /// Test if case is available
    /// </summary>
    /// <param name="context">The runtime context</param>
    /// <returns>True for an available case</returns>
    public bool GetAvailableCase(CaseRuntimeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // available without expression
        if (string.IsNullOrWhiteSpace(context.Case?.AvailableExpression))
        {
            return true;
        }

        // available script
        var runtime = new CaseAvailableRuntime(context);
        var script = CreateScript<CaseAvailableFunction>(
            runtime: runtime,
            embeddedTemplate: CodeFactory.CaseAvailableTemplate,
            expression: context.Case.AvailableExpression,
            returnValue: true);
        if (script == null)
        {
            return true;
        }

        // available test
        var available = script.Available() as bool?;
        return available ?? true;
    }

    /// <summary>
    /// Build a case
    /// </summary>
    /// <param name="context">The runtime context</param>
    /// <returns>The available case fields</returns>
    public void BuildCase(CaseChangeRuntimeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (string.IsNullOrWhiteSpace(context.Case?.BuildExpression))
        {
            return;
        }

        // build script
        var runtime = new CaseBuildRuntime(context);
        var script = CreateScript<CaseBuildFunction>(
            runtime: runtime,
            embeddedTemplate: CodeFactory.CaseBuildTemplate,
            expression: context.Case.BuildExpression,
            returnValue: false);
        if (script == null)
        {
            return;
        }

        // build fields
        script.Build();
    }

    /// <summary>
    /// Validate a case
    /// </summary>
    /// <param name="context">The runtime context</param>
    /// <returns>True for a valid case</returns>
    public bool ValidateCase(CaseChangeRuntimeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Case == null || context.CaseValues == null)
        {
            return false;
        }

        // mandatory fields
        foreach (var caseValue in context.CaseValues)
        {
            if (!ValidateCaseValue(caseValue))
            {
                return false;
            }
        }

        // valid without expression
        if (string.IsNullOrWhiteSpace(context.Case.ValidateExpression))
        {
            return true;
        }

        // validate script
        var runtime = new CaseValidateRuntime(context);
        var script = CreateScript<CaseValidateFunction>(
            runtime: runtime,
            embeddedTemplate: CodeFactory.CaseValidateTemplate,
            expression: context.Case.ValidateExpression,
            returnValue: true);
        if (script == null)
        {
            return true;
        }

        var valid = script.Validate() as bool?;
        return valid ?? true;
    }

    private static bool ValidateCaseValue(CaseValue caseValue)
    {
        if (string.IsNullOrWhiteSpace(caseValue.Field))
        {
            return false;
        }

        var caseField = new CaseFieldService().GetCaseField(caseValue.Field);
        if (caseField == null)
        {
            return false;
        }

        // mandatory field
        if (caseField.Required && string.IsNullOrWhiteSpace(caseValue.Value))
        {
            return false;
        }

        return true;
    }

    #region Compiler

    private static dynamic? CreateScript<T>(object runtime, string embeddedTemplate,
        string expression, bool returnValue)
    {
        // c# compilation
        var compiler = new ScriptCompiler(
            scriptType: typeof(CaseAvailableFunction),
            expression: expression,
            embeddedTemplate: embeddedTemplate,
            embeddedSourceFiles: CodeFactory.SourceFiles,
            returnValue: returnValue);

        var compileResult = compiler.Compile();

        // load assembly from binary
        using var loadContext = new CollectibleAssemblyLoadContext();
        var assembly = loadContext.LoadFromBinary(compileResult.Binary);

        // build type from assembly
        var script = CreateScript<T>(assembly, runtime);
        return script;
    }

    /// <summary>
    /// Create a new script instance
    /// </summary>
    /// <param name="assembly">The script assembly</param>
    /// <param name="runtime">The script runtime</param>
    /// <returns>New instance of the scripting type</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static dynamic? CreateScript<T>(Assembly assembly, object runtime)
    {
        var scriptType = typeof(T);
        var assemblyScriptType = assembly.GetType(scriptType.FullName ?? throw new InvalidOperationException());
        if (assemblyScriptType == null)
        {
            throw new ScriptException($"Unknown script type {scriptType}");
        }

        // create script instance
        return Activator.CreateInstance(assemblyScriptType, runtime);
    }

    #endregion
}