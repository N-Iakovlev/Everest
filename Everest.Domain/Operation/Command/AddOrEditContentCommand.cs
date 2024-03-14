namespace Everest.Domain;

#region << Using >>
using FluentValidation;
using HandlebarsDotNet;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;
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
    public IFormFile ContentImage { get; set; }
    public string ShortDescription { get; set; }
    public string LongDescription { get; set; }

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Content content = isNew ? new Content() : Repository.GetById<Content>(Id.GetValueOrDefault());
        content.ShortDescription = ShortDescription;
        content.LongDescription = LongDescription;
        if (ContentImage != null && ContentImage.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                ContentImage.CopyTo(memoryStream);
                content.ContentImage = memoryStream.ToArray(); 
            }
        }

        Repository.SaveOrUpdate(content);
    }

    public class Validator : AbstractValidator<AddOrEditContentCommand>
    {
        public Validator()
        {
            RuleFor(content => content.ShortDescription).NotEmpty();
            


        }
    }

    public class AsQuery : QueryBase<AddOrEditContentCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditContentCommand ExecuteResult()
        {
            var content = Repository.GetById<Content>(Id);
            if (content == null)
                return new AddOrEditContentCommand();

            return new AddOrEditContentCommand()
                   {
                           Id = content.Id,
                           ShortDescription = content.ShortDescription,
                           LongDescription = content.LongDescription,
                    };
        }
    }
}