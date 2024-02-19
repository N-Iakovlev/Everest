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
    public virtual int? EmployeeCategoryId { get; set; }
    public virtual EmployeeCategory? EmployeeCategory { get; set; }

    public Employee()
    {
        EmployeeCategory = new EmployeeCategory();

    }


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
            MapEscaping(q => q.EmployeeCategoryId);

            References(q => q.EmployeeCategory).Column(nameof(EmployeeCategoryId))
                                               .Nullable() // Разрешаем значение null
                                               .ReadOnly()
                                               .LazyLoad();
       

        }
    }
}
public class EmployeeCategory : EverestEntityBase
{
    public new virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual IList<Employee> Employees { get; set; }

    public EmployeeCategory()
    {
        Employees = new List<Employee>();
    }

    [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
    public class Map : NHibernateEntityMap<EmployeeCategory>
    {
        public Map()
        {
            Id(q => q.Id).GeneratedBy.Identity();
            MapEscaping(q => q.Name);

            HasMany(q => q.Employees).KeyColumn(nameof(Employee.EmployeeCategoryId))
                                     .Cascade.Delete()
                                     .LazyLoad();
        }
    }
}