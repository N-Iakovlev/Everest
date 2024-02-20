using Everest.Domain;
using Incoding.Core.CQRS.Core;

namespace Everest.API.Controllers
{
    #region << Using >>

    using Incoding.Web.MvcContrib;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public HomeController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        [Route("/Admin")]
        public IActionResult Admin()
        {
            return View("~/Views/Admin/Index.cshtml");
        }

        public ActionResult Index()
        {
            return View("~/Views/Application/Index.cshtml");
        }
    }

    [AllowAnonymous]
    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController(IServiceProvider serviceProvider)
                : base(serviceProvider) { }
    }

}