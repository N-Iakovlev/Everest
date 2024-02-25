using System.Reflection.Metadata;

namespace Everest.Domain;

#region << Using >>

using System.Diagnostics.CodeAnalysis;
using Incoding.Core;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

#endregion

public class Order : EverestEntityBase
{
    public virtual string Creator { get; set; }
    public virtual DateTime CreateDate { get; set; }
    public virtual int NunberOrder { get; set; }
    public virtual string StatusOrder { get; set; }
    public virtual List<Product> Products { get; set; }
    public virtual string Notes { get; set; }
    public virtual string Phone { get; set; }
    public virtual DateTime StatusChangeDate { get; set; }


    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Order>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.Creator);
            MapEscaping(q => q.CreateDate);
            MapEscaping(q => q.NunberOrder);
            MapEscaping(q => q.StatusOrder);
            MapEscaping(q => q.Notes);
            MapEscaping(q => q.Phone);
            MapEscaping(q => q.StatusChangeDate);
            HasMany(q => q.Products);
        }
    }
}

