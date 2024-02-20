using System.Reflection.Metadata;
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
    public virtual byte[] Avatar { get; set; }
    public virtual string Phone { get; set; }
    public virtual string Email { get; set; }
    public virtual Category Category { get; set; }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Employee>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.FirstName);
            MapEscaping(q => q.LastName);
            MapEscaping(q => q.Avatar).Length(int.MaxValue);
            MapEscaping(q => q.Phone);
            MapEscaping(q => q.Email);
            References(q => q.Category);
        }
    }
}
