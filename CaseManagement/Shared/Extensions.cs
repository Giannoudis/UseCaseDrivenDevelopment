using System;

namespace UseCaseDrivenDevelopment.CaseManagement.Shared;

public static class Extensions
{
    #region String

    /// <summary>Ensures an ending suffix</summary>
    /// <param name="source">The source value</param>
    /// <param name="suffix">The suffix to add</param>
    /// <returns>The string with suffix</returns>
    public static string EnsureEnd(this string source, string suffix)
    {
        if (!string.IsNullOrWhiteSpace(suffix))
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                source = suffix;
            }
            else if (!source.EndsWith(suffix))
            {
                source = $"{source}{suffix}";
            }
        }

        return source;
    }

    #endregion

    #region DateTime

    /// <summary>
    /// Get compact date time string
    /// </summary>
    /// <param name="value">The date value</param>
    /// <returns>Compact date string</returns>
    public static string ToCompactString(this DateTime value) =>
        // test fro midnight
        value.TimeOfDay.Ticks == 0 ? $"{value:d}" : $"{value:g}";

    #endregion
}