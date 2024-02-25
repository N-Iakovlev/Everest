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
    public virtual byte[] ProductPhoto { get; set; }
    public virtual decimal Price { get; set; }
    public virtual string ShortDescription { get; set; }
    public virtual string LongDescription { get; set; }
    public virtual Category Category { get; set; }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Product>
    {
        public Map()
        {
            Id(pr => pr.Id).GeneratedBy.Identity();
            MapEscaping(pr => pr.ProductName);
            MapEscaping(pr => pr.ProductPhoto).Length(int.MaxValue);
            MapEscaping(pr => pr.Price);
            MapEscaping(pr => pr.ShortDescription);
            MapEscaping(pr => pr.LongDescription);
            References(pr => pr.Category);

        }
    }
}