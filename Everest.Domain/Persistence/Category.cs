using System.Reflection.Metadata;

using System.ComponentModel.DataAnnotations;

namespace Everest.Domain;

using System.ComponentModel;

#region << Using >>

using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

#endregion


public class Category : EverestEntityBase
{
    public enum OfType
    {
        Product,
        Employee
    }
    public virtual string Name { get; set; }

    public virtual OfType Type { get; set; }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Category>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.Name); 
            MapEscaping(q => q.Type).CustomType<OfType>();
        }
    }
}
