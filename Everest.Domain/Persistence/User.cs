using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace Everest.Domain;

public class User : EverestEntityBase
{
    public virtual string TempId { get; set; }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<User>
    {
        protected Map()
        {
            Id(ca => ca.Id).GeneratedBy.Identity();
            MapEscaping(q => q.TempId);
        }
    }
  
}