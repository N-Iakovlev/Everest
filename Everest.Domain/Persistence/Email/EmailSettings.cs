using Incoding.Core.Quality;
using Incoding.Data.NHibernate;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;

public class EmailSettings : EverestEntityBase
{
    public virtual string SmtpServer { get; set; }
    public virtual int Port { get; set; }
    public  virtual string Username { get; set; }
    public virtual string Password { get; set; }


    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<EmailSettings>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.SmtpServer);
            MapEscaping(q => q.Port);
            MapEscaping(q => q.Username);
            MapEscaping(q => q.Password);
        }
    }
}