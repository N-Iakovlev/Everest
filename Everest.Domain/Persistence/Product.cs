namespace Everest.Domain;

#region << Using >>

using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

#endregion

public class Product : EverestEntityBase
{
    public virtual string ProductName { get; set; }

    public virtual string ProductCategory { get; set; }
    

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Product>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.ProductName);
            MapEscaping(q => q.ProductCategory);
        }
    }
}