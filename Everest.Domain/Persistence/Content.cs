namespace Everest.Domain;

#region << Using >>

using System.Diagnostics.CodeAnalysis;
using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;

#endregion

public class Content : EverestEntityBase
{
    public virtual string ShortDescription { get; set; }
    public virtual string LongDescription { get; set; }
    public virtual byte[] ContentImage { get; set; }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<Content>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.ShortDescription);
            MapEscaping(q => q.LongDescription);
            MapEscaping(q => q.ContentImage).Length(int.MaxValue);
        }
    }
}