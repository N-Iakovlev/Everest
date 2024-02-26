using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;

public class ShoppingCart : EverestEntityBase
{
    public virtual int UserId { get; set; }
    public virtual List<CartLine> CartLines { get; set; }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<ShoppingCart>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.UserId);
            HasMany(q => q.CartLines);
        }
    }
}

public class CartLine : EverestEntityBase
    {
        public virtual Product Product { get; set; }
        public virtual int Quantity { get; set; }


        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<CartLine>
        {
            public Map()
            {
                Id(q => q.Id).GeneratedBy.Identity();
                MapEscaping(q => q.Quantity);
                References(q => q.Product); // Связь с товаром

            }
        }
    }
