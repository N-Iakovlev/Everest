namespace Everest.Domain;

#region << Using >>

using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

#endregion

public class Partner : EverestEntityBase
{
    public virtual string CompanyName { get; set; }
    public virtual byte[] Label { get; set; }
    public virtual string Site { get; set; }
 

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Partner>
    {
        public Map()
        {
            Id(pr => pr.Id).GeneratedBy.Identity();
            MapEscaping(pr => pr.CompanyName);
            MapEscaping(pr => pr.Label).Length(int.MaxValue);
            MapEscaping(pr => pr.Site);
        }
    }
}