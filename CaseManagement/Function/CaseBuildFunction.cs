/* CaseBuildFunction */

// ReSharper disable RedundantUsingDirective

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

// ReSharper restore RedundantUsingDirective

namespace UseCaseDrivenDevelopment.CaseManagement.Function;

public class CaseBuildFunction : CaseChangeFunction
{
    public CaseBuildFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public void Build()
    {
        // ReSharper disable EmptyRegion

        #region Expression

        #endregion

        // ReSharper restore EmptyRegion
    }
}