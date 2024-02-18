using Microsoft.AspNetCore.Http;

namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using HandlebarsDotNet;
using Incoding.Core.CQRS.Core;

#endregion

public class DeleteContentCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<Content>(Id));
    }
}

public class AddOrEditContentCommand : CommandBase
{
    public int? Id { get; set; }
    public string ContentName { get; set; }
   
    

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Content pr = isNew ? new Content() : Repository.GetById<Content>(Id.GetValueOrDefault());
        pr.ContentName = ContentName;
        
        Repository.SaveOrUpdate(pr);
    }

    public class Validator : AbstractValidator<AddOrEditContentCommand>
    {
        public Validator()
        {
            RuleFor(pr => pr.ContentName).NotEmpty();

            
        }
    }

    public class AsQuery : QueryBase<AddOrEditContentCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditContentCommand ExecuteResult()
        {
            var pr = Repository.GetById<Content>(Id);
            if (pr == null)
                return new AddOrEditContentCommand();

            return new AddOrEditContentCommand()
                   {
                           Id = pr.Id,
                           ContentName = pr.ContentName,
                          
                   };
        }
    }
}