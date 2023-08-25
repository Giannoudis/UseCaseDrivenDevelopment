using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UseCaseDrivenDevelopment.CaseManagement.Shared;

namespace UseCaseDrivenDevelopment.CaseManagement.Compiler;

/// <summary>
/// Script template
/// </summary>
internal sealed class ScriptCompiler
{
    /// <summary>
    /// Script type
    /// </summary>
    private Type ScriptType { get; }

    /// <summary>
    /// Source codes
    /// </summary>
    private IReadOnlyDictionary<string, string> SourceCodes { get; }

    private const string ExpressionRegion = "Expression";
    private const string ReturnStatement = "return";

    /// <summary>
    /// Initializes a new instance of the <see cref="ScriptCompiler"/> class
    /// </summary>
    internal ScriptCompiler(Type scriptType, string embeddedTemplate, string expression,
        IEnumerable<string> embeddedSourceFiles, bool returnValue)
    {
        if (scriptType == null)
        {
            throw new ArgumentNullException(nameof(scriptType));
        }

        if (string.IsNullOrWhiteSpace(embeddedTemplate))
        {
            throw new ArgumentException(nameof(embeddedTemplate));
        }

        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException(nameof(expression));
        }

        if (embeddedSourceFiles == null)
        {
            throw new ArgumentNullException(nameof(embeddedSourceFiles));
        }

        ScriptType = scriptType ?? throw new ArgumentNullException(nameof(scriptType));

        // source codes
        var sourceCodes = new Dictionary<string, string>();

        // template
        var codeName = embeddedTemplate.EnsureEnd(".cs");
        var template = CodeFactory.GetEmbeddedCodeFile(codeName);
        sourceCodes.Add(codeName, GetTemplateCode(template, expression, returnValue));

        // embedded files
        foreach (var scriptName in embeddedSourceFiles)
        {
            codeName = scriptName.EnsureEnd(".cs");
            sourceCodes.Add(codeName, CodeFactory.GetEmbeddedCodeFile(codeName));
        }

        SourceCodes = new ReadOnlyDictionary<string, string>(sourceCodes);
    }

    /// <summary>
    /// Compile the assembly
    /// </summary>
    /// <returns>The compile result</returns>
    internal ScriptCompileResult Compile()
    {
        if (string.IsNullOrWhiteSpace(ScriptType.FullName))
        {
            throw new ScriptException($"Type {ScriptType} without assembly");
        }

        // collect code to compile
        var codes = new List<string>();
        codes.AddRange(SourceCodes.Values);

        // compile code
        var compiler = new CSharpCompiler(ScriptType.FullName);
        return compiler.CompileAssembly(new List<string>(codes));
    }

    /// <summary>
    /// Apply template code to a source code region
    /// </summary>
    /// <param name="template">The template code</param>
    /// <param name="expression">The expression code</param>
    /// <param name="returnValue">Expression with return value</param>
    private string GetTemplateCode(string template, string expression, bool returnValue)
    {
        var templateCode = SetupRegion(template, ExpressionRegion, GetExpressionCode(expression, returnValue));
        return templateCode;
    }

    private string SetupRegion(string template, string regionName, string expression)
    {
        // start
        var startMarker = $"#region {regionName}";
        var startIndex = template.IndexOf(startMarker, StringComparison.InvariantCulture);
        if (startIndex < 0)
        {
            throw new ScriptException($"Missing start region with name {regionName} in script type {ScriptType}");
        }

        // end
        var endMarker = "#endregion";
        var endIndex = template.IndexOf(endMarker, startIndex + startMarker.Length, StringComparison.InvariantCulture);
        if (endIndex < 0)
        {
            throw new ScriptException($"Missing end region with name {regionName} in script type {ScriptType}");
        }

        // token replacement
        var placeholder = template.Substring(startIndex, endIndex - startIndex + endMarker.Length);
        return template.Replace(placeholder, expression);
    }

    private static string GetExpressionCode(string expression, bool returnValue)
    {
        if (returnValue && !HasReturnStatement(expression))
        {
            expression = $"return {expression}";
        }

        return expression.EnsureEnd(";");
    }

    // ignore multi line statement
    private static bool HasReturnStatement(string code) =>
        code.Contains(';') || code.StartsWith(ReturnStatement);
}