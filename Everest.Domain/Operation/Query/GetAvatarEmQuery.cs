using Incoding.Core.CQRS.Core;

namespace Everest.Domain;


public class GetAvatarEmQuery : QueryBase<byte[]> 
{
    public  int Id { get; set; }
    protected override byte[] ExecuteResult()
    {
        return Repository.GetById<Employee>(Id).Avatar;
    }

}
