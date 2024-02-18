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
    public virtual int Quantity { get; set; }
    public virtual string ProductArticl { get; set; }
    public virtual string Description { get; set; }
    public virtual string Brand { get; set; }




    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Product>
    {
        public Map()
        {
            Id(pr => pr.Id).GeneratedBy.Identity();
            MapEscaping(pr => pr.ProductName);
            MapEscaping(pr => pr.ProductPhoto);
            MapEscaping(pr => pr.Price);
            MapEscaping(pr => pr.Quantity);
            MapEscaping(pr => pr.ProductArticl);
            MapEscaping(pr => pr.Description);
            MapEscaping(pr => pr.Brand);


        }
    }
}