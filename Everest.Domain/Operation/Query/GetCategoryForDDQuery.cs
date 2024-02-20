using Everest.Domain;
using Incoding.Core.CQRS.Core;
using Incoding.Core.ViewModel;

namespace Everest.Domain;
public class GetCategoryForDDQuery : QueryBase<List<KeyValueVm>>
{

    public Category.OfType? Type { get; set; }
    protected override List<KeyValueVm> ExecuteResult()
    {
        var keyValueVms = Repository.Query<Category>()
            .Where(q => !Type.HasValue || q.Type == Type)
            .Select(q => new KeyValueVm(q.Id, q.Name))
            .ToList();

        keyValueVms.Insert(0,new KeyValueVm(string.Empty));
        return keyValueVms;
    }
}