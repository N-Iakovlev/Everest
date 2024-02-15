namespace Everest.Domain;

#region << Using >>

using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

#endregion

public class Employee : EverestEntityBase
{
    public virtual string FirstName { get; set; }

    public virtual string LastName { get; set; }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Employee>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.FirstName);
            MapEscaping(q => q.LastName);
        }
    }
}