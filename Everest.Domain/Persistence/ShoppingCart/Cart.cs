
using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

namespace Everest.Domain;

public class Cart : EverestEntityBase
{
    public virtual User User { get; set; }

    [UsedImplicitly]
    [Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true)]
    [ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Cart>
    {
        public Map()
        {
            Id(ca => ca.Id).GeneratedBy.Identity();
            References(ca => ca.User);
            
        }
    }
}

public class CartItem : EverestEntityBase
{
    public virtual Cart Cart { get; set; }
    public virtual Product Product { get; set; }
    


    [UsedImplicitly]
    [Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true)]
    [ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<CartItem>
    {
        public Map()
        {
            Id(ca => ca.Id).GeneratedBy.Identity();
            References(ca => ca.Product);
            References(ca => ca.Cart);
        }
    }
}

