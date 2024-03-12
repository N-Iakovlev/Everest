using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incoding.Core.Block.IoC;

namespace Everest.Domain;

public class SendEmailCommand : CommandBase
{
   

    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

 

    protected override void Execute()
    {
        // Для примера, здесь можно добавить логику для получения адресата, темы и тела письма,
        
        try
        {
            var _emailService = IoCFactory.Instance.TryResolve<EmailService>();
            _emailService.SendEmailDefault(To, Subject, Body);
        }
        catch (Exception ex)
        {
            
            throw new Exception("Ошибка при отправке электронного письма", ex);
        }
    }
}

