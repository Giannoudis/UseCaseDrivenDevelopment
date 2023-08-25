using System;

namespace UseCaseDrivenDevelopment.CaseManagement.Shared;

/// <summary>Payroll script exception</summary>
public class ScriptException : Exception
{
    /// <summary>Initializes a new instance of the exception</summary>
    public ScriptException(string message) :
        base(message)
    {
    }

    /// <summary>Initializes a new instance of the exception</summary>
    public ScriptException(string message, Exception innerException) :
        base(message, innerException)
    {
    }
}