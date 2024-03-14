namespace Everest.Domain;

#region << Using >>

using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

#endregion

public class Contacts : EverestEntityBase
{
    public virtual string Adress { get; set; }
    public virtual int  Phone { get; set; }
    public virtual string Email { get; set; }
    public virtual string Company { get; set; }
    public virtual string Domen { get; set; }
    

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Contacts>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.Adress);
            MapEscaping(q => q.Phone); 
            MapEscaping(q => q.Email);
            MapEscaping(q => q.Company);
            MapEscaping(q => q.Domen);
        }
    }
}