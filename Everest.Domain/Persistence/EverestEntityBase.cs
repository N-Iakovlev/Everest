namespace Everest.Domain;

#region << Using >>

using Incoding.Core.Data;

#endregion

public class EverestEntityBase : IncEntityBase
{
    public new virtual int Id { get; set; }
}