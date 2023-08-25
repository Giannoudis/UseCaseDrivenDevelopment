﻿/* CaseValidateFunction */

// ReSharper disable RedundantUsingDirective

using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

// ReSharper restore RedundantUsingDirective

namespace UseCaseDrivenDevelopment.CaseManagement.Function;

public class CaseValidateFunction : CaseChangeFunction
{
    public CaseValidateFunction(object runtime) :
        base(runtime)
    {
    }

    /// <summary>Entry point for the runtime</summary>
    /// <remarks>Internal usage only, do not call this method</remarks>
    public bool? Validate()
    {
        // ReSharper disable EmptyRegion

        #region Expression

        #endregion

        // ReSharper restore EmptyRegion

        // compiler will optimize this out if the code provides a return
        return default;
    }
}