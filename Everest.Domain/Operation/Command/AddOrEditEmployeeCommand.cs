using Microsoft.AspNetCore.Http;

namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using Incoding.Core.CQRS.Core;

#endregion

public class DeleteEmployeeCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<Employee>(Id));
    }
}

public class AddOrEditEmployeeCommand : CommandBase
{
    public string LastName { get; set; }

    public string FirstName { get; set; }

    public int? Id { get; set; }
    
    public IFormFile Avatar { get; set; }

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Employee em = isNew ? new Employee() : Repository.GetById<Employee>(Id.GetValueOrDefault());
        em.FirstName = FirstName;
        em.LastName = LastName;
        if (Avatar != null && Avatar.Length > 0)
        {
            
            using (var memoryStream = new MemoryStream())
            {
                Avatar.CopyTo(memoryStream);
                em.Avatar = memoryStream.ToArray(); // Сохраняем массив байтов в свойство Avatar
            }
        }
        Repository.SaveOrUpdate(em);
    }

    public class Validator : AbstractValidator<AddOrEditEmployeeCommand>
    {
        public Validator()
        {
            RuleFor(q => q.FirstName).NotEmpty();

            RuleFor(q => q.LastName).NotEmpty();
        }
    }

    public class AsQuery : QueryBase<AddOrEditEmployeeCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditEmployeeCommand ExecuteResult()
        {
            var em = Repository.GetById<Employee>(Id);
            if (em == null)
                return new AddOrEditEmployeeCommand();

            return new AddOrEditEmployeeCommand()
                   {
                           Id = em.Id,
                           FirstName = em.FirstName,
                           LastName = em.LastName,
                   };
        }
    }
}