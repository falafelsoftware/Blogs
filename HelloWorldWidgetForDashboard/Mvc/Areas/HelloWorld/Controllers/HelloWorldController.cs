using System;
using System.Linq;
using System.Web.Mvc;
using FalafelDashboard.Common;
using FalafelDashboard.Configuration;
using SitefinityWebApp.Mvc.Areas.HelloWorld.Models;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;

namespace SitefinityWebApp.Mvc.Areas.HelloWorld.Controllers
{
    /// <summary>
    /// Controller that provides functionality
    /// </summary>
    public class HelloWorldController : Controller
    {
        #region Constants and Fields

        #endregion

        #region Properties

        /// <summary>
        /// The message to display in the hello world view
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            try
            {
                if (Utils.IsWidgetAllowed("HelloWorld"))
                {
                    //INITIALIZE VIEW MODEL
                    var model = new HelloWorldViewModel { Message = "Hello world dashboard widget"};

                    //TODO: POPULATE VIEW MODEL WITH DATA

                    //RETURN AREA VIEW WITH VIEW MODEL
                    return View("../../Areas/HelloWorld/Views/HelloWorld/Index", model);
                }

                return View("../../Areas/HelloWorld/Views/HelloWorld/Unauthorized");
            }
            catch (Exception ex)
            {
                return View("../../Areas/HelloWorld/Views/HelloWorld/Error", ex);
            }
        }

        #endregion
    }
}