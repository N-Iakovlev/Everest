using Incoding.Core.ViewModel;

namespace Everest.API;

public class DropDownModel
{
    
    public string Text { get; set; }

    
    public string Url { get; set; }

    
    public string Template { get; set; }

    
    public List<KeyValueVm> Items { get; set; }
}
