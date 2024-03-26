using Incoding.Core.ViewModel;
namespace Everest.API;

public class DropDownItemModel
{
    // Значение элемента списка
    public object Value { get; set; }

    // Текст, отображаемый для элемента списка
    public string Text { get; set; }
    public string Search { get; set; }

    public bool? Selected { get; set; }

    public DropDownItemModel(object value, string text, bool selected = false)
    {
        this.Value = value;
        this.Text = text;
        this.Selected = selected;
    }

    public static implicit operator KeyValueVm(DropDownItemModel item)
    {
        return new KeyValueVm(item.Value, item.Text);
    }

    public static implicit operator DropDownItemModel(KeyValueVm item)
    {
        return new DropDownItemModel(item.Value, item.Text);
    }
}