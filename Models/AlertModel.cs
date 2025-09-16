using BlazorApp1.Components.ViewElements;

namespace BlazorApp1.Models
{
    public enum PopupType
    {
        Warning,
        Danger,
        Success
    }

    public record AlertModel(string Message, PopupType Type);
}
