using Avalonia.Controls;

namespace Avalonia.Demo.Contracts;

public class Helper
{
    private static user50Context s_instanse;
    public static UserControl CurrentUserControl { get; set; }
    public static Roles CurrentRole{ get; set; }
    public static user50Context GetContext() => s_instanse ??= new user50Context();
}