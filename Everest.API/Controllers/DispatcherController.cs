using Everest.Domain;
using Incoding.Core.CQRS.Core;

namespace Everest.API.Controllers
{
    #region << Using >>

    using Incoding.Web.MvcContrib;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static NHibernate.Engine.Query.CallableParser;
    using System.Net.Mail;

    #endregion

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public HomeController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        public IActionResult Admin()
        {
            return View("~/Views/Admin/Index.cshtml");
        }

        public IActionResult Main()
        {
            return View("~/Views/App/Index.cshtml");
        }
        public IActionResult Cart()
        {
            return View("~/Views/App/Cart/Index.cshtml");
        }
    }

    [AllowAnonymous]
    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController(IServiceProvider serviceProvider)
                : base(serviceProvider) { }
    }

}