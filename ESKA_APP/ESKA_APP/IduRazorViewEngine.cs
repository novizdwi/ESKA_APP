using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DevExpress.Web.Mvc;

public class IduRazorViewEngine : RazorViewEngine
{
    public IduRazorViewEngine()
        : base()
    {
       

        ViewLocationFormats = new[] {
            "~/Views/%1{1}/{0}.cshtml", 
            "~/Views/Shared/{0}.cshtml", 
        };

        PartialViewLocationFormats = new[] {
            "~/Views/%1{1}/{0}.cshtml", 
            "~/Views/Shared/{0}.cshtml", 
        };

        
    }

    protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
    {
        var nameSpace = controllerContext.Controller.GetType().Namespace;

        nameSpace = nameSpace.Replace("Controllers", "");
        nameSpace = nameSpace.Replace(".", "/");
        if (nameSpace != "")
        {
            nameSpace = nameSpace + "/";
        }
        return base.CreatePartialView(controllerContext, partialPath.Replace("%1", nameSpace));
    }

    protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
    {
        var nameSpace = controllerContext.Controller.GetType().Namespace;
        nameSpace = nameSpace.Replace("Controllers", "");
        nameSpace = nameSpace.Replace(".", "/");
        if (nameSpace != "")
        {
            nameSpace = nameSpace + "/";
        }

        return base.CreateView(controllerContext, viewPath.Replace("%1", nameSpace), masterPath.Replace("%1", nameSpace));
    }

    protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
    {
        var nameSpace = controllerContext.Controller.GetType().Namespace;
        nameSpace = nameSpace.Replace("Controllers", "");
        nameSpace = nameSpace.Replace(".", "/");
        if (nameSpace != "")
        {
            nameSpace = nameSpace + "/";
        }
        return base.FileExists(controllerContext, virtualPath.Replace("%1", nameSpace));
    }

}



