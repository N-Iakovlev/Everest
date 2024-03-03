using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;
using Microsoft.AspNetCore.SignalR;
using NHibernate.Mapping;


namespace Everest.Domain;

public class Order : EverestEntityBase
{
    public enum OfStatus
    {
        New,
        Processing,
        Shipped,
        Completed
    }
    public virtual OfStatus Status { get; set; }
    public  virtual DateTime OrderDate { get; set; }
    public virtual IList<OrderDetail> OrderDetails { get; set; }
    public virtual int UserId { get; set; }


    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Order>
    {
        public Map()
        {

            Id(o => o.Id).GeneratedBy.Identity();
            MapEscaping(o => o.Status).CustomType<OfStatus>();
            MapEscaping(o => o.OrderDate);
            HasMany(o => o.OrderDetails);
        }
    }
}

public class OrderDetail : EverestEntityBase
{
    public virtual int CartId { get; set; }
    public virtual int ProductId { get; set; }

   
    
    


    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<OrderDetail>
    {
        public Map()
        {
            Id(o => o.Id).GeneratedBy.Identity();
            MapEscaping(o => o.CartId); // Ссылка на корзину, в которой находится продукт
            MapEscaping(o => o.ProductId); // Ссылка на сам продукт
        }
    }
}