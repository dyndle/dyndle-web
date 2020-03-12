﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trivident.Modules.Core.DebugInfo
{
    public interface IDebugInfoProvider
    {
        MvcHtmlString ShowButton(int position);
        string Name { get; }
        string IconBase64 { get; }
        string OverrideLocation { get; }
        void OnActionExecuting(ActionExecutingContext filterContext);
        void OnActionExecuted(ActionExecutedContext filterContext);
        void OnResultExecuting(ResultExecutingContext filterContext);
        void OnResultExecuted(ResultExecutedContext filterContext);
    }
}