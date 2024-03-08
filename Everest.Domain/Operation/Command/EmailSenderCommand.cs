using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;
public class EmailSenderCommand : CommandBase
{
    private readonly IEmailService _emailService;

    public EmailSenderCommand(IEmailService emailService)
    {
        _emailService = emailService;
    }
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    protected override async void Execute()
    {
        // Отправка письма
        await _emailService.SendEmailAsync(ToEmail, Subject, Body);
    }
}
