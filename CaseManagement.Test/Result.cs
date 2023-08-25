using System;

namespace UseCaseDrivenDevelopment.CaseManagement.Test;

public class Result
{
    public DateTime? Executed { get; set; }
    public bool? Valid { get; set; }
    public string? Source { get; set; }
    public string? Expected { get; set; }
    public string? Actual { get; set; }

    public void Apply(Result source)
    {
        Executed = source.Executed;
        Valid = source.Valid;
        Source = source.Source;
        Expected = source.Expected;
        Actual = source.Actual;
    }

    public void Reset()
    {
        Executed = null;
        Valid = null;
        Source = null;
        Expected = null;
        Actual = null;
    }
}