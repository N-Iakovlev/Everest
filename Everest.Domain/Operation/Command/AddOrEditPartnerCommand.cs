using Antlr.Runtime;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;
public class DeletePartnerCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<Content>(Id));
    }
}

public class AddOrEditPartnerCommand : CommandBase
{
    public int? Id { get; set; }
    public IFormFile Label { get; set; }
    public string Site { get; set; }
    public string CompanyName { get; set; }

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Partner partner = isNew ? new Partner() : Repository.GetById<Partner>(Id.GetValueOrDefault());
        partner.Site = Site;
        partner.CompanyName = CompanyName;
        if (Label != null && Label.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                Label.CopyTo(memoryStream);
                partner.Label = memoryStream.ToArray();
            }
        }

        Repository.SaveOrUpdate(partner);
    }

    public class Validator : AbstractValidator<AddOrEditPartnerCommand>
    {
        public Validator()
        {
            RuleFor(partner => partner.CompanyName).NotEmpty();
            RuleFor(partner => partner.Site).NotEmpty();
        }
    }

    public class AsQuery : QueryBase<AddOrEditPartnerCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditPartnerCommand ExecuteResult()
        {
            var partner = Repository.GetById<Partner>(Id);
            if (partner == null)
                return new AddOrEditPartnerCommand();

            return new AddOrEditPartnerCommand()
            {
                Id = partner.Id,
                Site = partner.Site,
                CompanyName = partner.CompanyName,
            };
        }
    }
}

