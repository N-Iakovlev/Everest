using FluentValidation;
using Incoding.Core.CQRS.Core;
using static Everest.Domain.Category;

namespace Everest.Domain
{
    public class DeleteCategoryCommand : CommandBase
    {
        public int Id { get; set; }

        protected override void Execute()
        {
            Repository.Delete(Repository.GetById<Category>(Id));
        }
    }

    public class AddOrEditCategoryCommand : CommandBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OfType Type { get; set; }

        protected override void Execute()
        {
            var isNew = Id == 0;
            Category ca = isNew ? new Category() : Repository.GetById<Category>(Id);
            ca.Name = Name;
            ca.Type = Type;

            Repository.SaveOrUpdate(ca);
        }

        public class Validator : AbstractValidator<AddOrEditCategoryCommand>
        {
            public Validator()
            {
                RuleFor(ca => ca.Name).NotEmpty();
            }
        }

        public class AsQuery : QueryBase<AddOrEditCategoryCommand>
        {
            public int Id { get; set; }

            protected override AddOrEditCategoryCommand ExecuteResult()
            {
                var ca = Repository.GetById<Category>(Id);
                if (ca == null)
                    return new AddOrEditCategoryCommand();

                return new AddOrEditCategoryCommand()
                {
                    Id = ca.Id,
                    Name = ca.Name,
                    Type = ca.Type,
                };
            }
        }
    }
}