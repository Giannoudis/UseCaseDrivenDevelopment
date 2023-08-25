using System;

namespace UseCaseDrivenDevelopment.CaseManagement.Shared;

/// <summary>Case value period</summary>
public class ValuePeriod
{
    private DateTime start = DateTime.UtcNow.Date;

    /// <summary>Period start date</summary>
    public DateTime Start
    {
        get => start;
        set
        {
            if (End.HasValue && value > End.Value)
            {
                value = End.Value;
            }

            start = value;
        }
    }

    private DateTime? end;

    /// <summary>Period end date, open period if not present</summary>
    public DateTime? End
    {
        get => end;
        set
        {
            if (value.HasValue && value.Value < Start)
            {
                value = Start;
            }

            end = value;
        }
    }

    public ValuePeriod()
    {
    }

    public ValuePeriod(DateTime start) :
        this(start, DateTime.MaxValue)
    {
    }

    public ValuePeriod(DateTime start, DateTime end)
    {
        Start = start < end ? start : end;
        End = start < end ? end : start;
    }

    public override string ToString() =>
        End.HasValue ? $"{Start.ToCompactString()}-{End.Value.ToCompactString()}" : $"{Start.ToCompactString()}-open";
}