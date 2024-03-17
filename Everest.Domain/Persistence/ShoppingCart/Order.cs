using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;
using NHibernate.Mapping;


namespace Everest.Domain;

public class Order : EverestEntityBase
{
    public enum OfStatus
    {
        New,
        Processing,
        Completed,
        Сanceled
    }
    public virtual OfStatus Status { get; set; }
    public virtual DateTime OrderDate { get; set; }
    public virtual string Comment { get; set; }
    public virtual string Email { get; set; }
    public virtual string Name { get; set; }
    public virtual User User { get; set; }
    
    

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Order>
    {
        public Map()
        {

            Id(o => o.Id).GeneratedBy.Identity();
            MapEscaping(o => o.Comment);
            MapEscaping(o => o.Email);
            MapEscaping(o => o.OrderDate);
            MapEscaping(o => o.Name);
            MapEscaping(o => o.Status).CustomType<OfStatus>();
            References(o => o.User);
        }
    }
}

public class OrderItem : EverestEntityBase
{
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }


    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<OrderItem>
    {
        public Map()
        {
            Id(o => o.Id).GeneratedBy.Identity();
            References(o => o.Product);
            References(o => o.Order);
        }
    }
}