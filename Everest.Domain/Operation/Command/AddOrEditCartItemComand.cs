using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentValidation;
using Incoding.Core.CQRS.Core;

namespace Everest.Domain;
public class DeleteCartLineommand : CommandBase
{
    public int Id { get; set; }
    
    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<CartLine>(Id));
    }
}

public class AddOrEditCartLineCommand : CommandBase

{
    public int? Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }


    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        CartLine q = isNew ? new CartLine() : Repository.GetById<CartLine>(Id.GetValueOrDefault());
        if (q == null)
        {
            q = new CartLine(); // Создаем новый объект, если не найден существующий
            q.Product = Product; // Устанавливаем продукт
            q.Quantity = Quantity; // Устанавливаем количество
            Repository.SaveOrUpdate(q); // Сохраняем новый элемент корзины
        }
        else
        {
            q.Quantity += Quantity; // Увеличиваем количество товара в корзине
            Repository.SaveOrUpdate(q); // Сохраняем обновленный элемент корзины
        }
    }

    public class Validator : AbstractValidator<AddOrEditCartLineCommand>
    {
        public Validator()
        {
           
            
        }
    }


    public class AsQuery : QueryBase<AddOrEditCartLineCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditCartLineCommand ExecuteResult()
        {
            var q = Repository.GetById<CartLine>(Id);
            if (q == null)
                return new AddOrEditCartLineCommand();

            return new AddOrEditCartLineCommand()
            {
                Id = q.Id,
                Product = q.Product,
                Quantity = q.Quantity

            };
        }
    }
}

