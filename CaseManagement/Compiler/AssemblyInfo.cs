using System;

namespace UseCaseDrivenDevelopment.CaseManagement.Compiler;

/// <summary>
/// Assembly information
/// </summary>
public class AssemblyInfo
{
    /// <summary>
    /// Assembly version
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// Assembly title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Assembly product
    /// </summary>
    public string Product { get; set; }

    public AssemblyInfo(Version version, string title, string product)
    {
        Version = version;
        Title = title;
        Product = product;
    }
}