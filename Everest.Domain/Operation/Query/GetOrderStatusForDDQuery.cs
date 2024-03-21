using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using Incoding.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;

public class GetOrderStatusForDDQuerys : QueryBase<List<KeyValueVm>>
{

    public Order.OfStatus? Status { get; set; }
    protected override List<KeyValueVm> ExecuteResult()
    {
        var keyValueVms = new List<KeyValueVm>();

        if (Status.HasValue)
        {
            // Создаем KeyValueVm для каждого статуса заказа
            keyValueVms = Enum.GetValues(typeof(Order.OfStatus))
                .Cast<Order.OfStatus>()
                .Select(status => new KeyValueVm(status.ToString(), status.ToString()))
                .ToList();
        }
        else
        {
            // Добавляем пустой элемент в начало списка
            keyValueVms.Add(new KeyValueVm(string.Empty));

            // Добавляем остальные статусы заказов
            keyValueVms.AddRange(Enum.GetValues(typeof(Order.OfStatus))
                .Cast<Order.OfStatus>()
                .Select(status => new KeyValueVm(status.ToString(), status.ToString())));
        }

        return keyValueVms;
    }
}