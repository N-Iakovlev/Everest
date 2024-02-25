using FluentValidation;
using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;

public class DeleteOrderCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<Order>(Id));
    }
}

public class AddOrEditOrderCommand : CommandBase
{
    public int? Id { get; set; }
    public DateTime StatusChangeDate { get; set; }
    public DateTime CreateDate { get; set; }
    public int NumberOrder { get; set; }
    public string? Products { get; set; }
    public string Creator { get; set; }
    public string Notes { get; set; }
    public string Phone { get; set; }
    public string StatusOrder { get; set; }


    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Order order = isNew ? new Order() : Repository.GetById<Order>(Id.GetValueOrDefault());

        
        
        order.Phone = Phone;
        order.StatusOrder = StatusOrder;
        order.Creator = Creator;
        order.Notes = Notes;

        Repository.SaveOrUpdate(order);
    }
    public class Validator : AbstractValidator<AddOrEditOrderCommand>
    {
        public Validator()
        {
            


        }
    }

    public class AddOrEditOrderCommandAsQuery : QueryBase<AddOrEditOrderCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditOrderCommand ExecuteResult()
        {
            var order = Repository.GetById<Order>(Id);
            if (order == null)
                return new AddOrEditOrderCommand();

            return new AddOrEditOrderCommand()
            {
                Id = order.Id,
                StatusChangeDate = order.StatusChangeDate,
                CreateDate = order.CreateDate,

                Phone = order.Phone,

                StatusOrder = order.StatusOrder,
                Creator = order.Creator,
                Notes = order.Notes
            };
        }

    }
}