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
        private readonly EmailService _emailService;

        public HomeController(IDispatcher dispatcher, EmailService emailService)
        {
            _dispatcher = dispatcher;
            _emailService = emailService;
        }
        public IActionResult Admin()
        {
            return View("~/Views/Admin/Index.cshtml");
        }

        public IActionResult Main()
        {
            return View("~/Views/App/Index.cshtml");
        }

        public async Task<IActionResult> SendEmailDefault(string to, string subject, string body)
        {
           
            await _emailService.SendEmailDefault(to, subject, body);
            return Ok();
        }


        public IActionResult Index()
        {
            return View();
        }
    }

    [AllowAnonymous]
    public class DispatcherController : DispatcherControllerBase
    {
        public DispatcherController(IServiceProvider serviceProvider)
                : base(serviceProvider) { }
    }

}