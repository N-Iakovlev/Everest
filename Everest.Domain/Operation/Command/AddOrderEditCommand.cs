using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Incoding.Web.MvcContrib;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;
using NHibernate.Engine;
using static Everest.Domain.Order;

namespace Everest.Domain;

public class AddEditOrderCommand : CommandBase
{
    public int? Id {get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Comment { get; set; }
    public OfStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
  
    public List<OrderItem> OrderItems { get; set; }



    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Order order = isNew ? new Order() : Repository.GetById<Order>(Id.GetValueOrDefault());
        
        order.OrderDate = DateTime.Now;
        order.Status = Status;
        order.Email = Email;
        order.Comment = Comment;
        OrderItems = new List<OrderItem>();
        
        Repository.SaveOrUpdate(order); 

        
    }
    public class Validator : AbstractValidator<AddEditOrderCommand>
    {
        public Validator()
        {
            RuleFor(q => q.Name).NotEmpty();
            RuleFor(q => q.Email).NotEmpty();
        }
    }
    public class AsQuery : QueryBase<AddEditOrderCommand>
    {
        public int Id { get; set; }
        protected override AddEditOrderCommand ExecuteResult()
        {
            var order = Repository.LoadById<Order>(Id);
            if (order != null)
            
                return new AddEditOrderCommand();
            return new AddEditOrderCommand
            {
                Email = order.Email,
                Comment = order.Comment,
                
            };
        }
        
    }
}


 