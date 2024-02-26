using Microsoft.AspNetCore.Http;

namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using Incoding.Core.CQRS.Core;

#endregion


public class AddOrEditShoppingCartCommand : CommandBase
{
    public int? Id { get; set; }

    public int UserId { get; set; }

    public List<CartLine> CartLines { get; set; }

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        ShoppingCart q = isNew ? new ShoppingCart() : Repository.GetById<ShoppingCart>(Id.GetValueOrDefault());

        q.UserId = UserId;
        q.CartLines = CartLines;

        Repository.SaveOrUpdate(q);
    }

    public class Validator : AbstractValidator<AddOrEditShoppingCartCommand>
    {
        public Validator()
        {

        }
    }

    public class AsQuery : QueryBase<AddOrEditShoppingCartCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditShoppingCartCommand ExecuteResult()
        {
            var q = Repository.GetById<ShoppingCart>(Id);
            if (q == null)
                return new AddOrEditShoppingCartCommand();

            return new AddOrEditShoppingCartCommand()
            {
                Id = q.Id,
                UserId = q.UserId,
                CartLines = q.CartLines

            };

        }
    }

    public class DeleteShoppingCartCommand : CommandBase
    {
        public int Id { get; set; }

        protected override void Execute()
        {
            Repository.Delete(Repository.GetById<ShoppingCart>(Id));
        }
    }
}
  
