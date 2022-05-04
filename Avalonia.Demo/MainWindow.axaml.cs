using System.Linq;
using Avalonia.Controls;
using Avalonia.Demo.Contracts;
using Avalonia.Demo.UserControls;
using Avalonia.Interactivity;

namespace Avalonia.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Helper.CurrentUserControl = Frame;
            if (Helper.CurrentRole==Roles.Admin)
            {
                Helper.CurrentUserControl.Content = new AdminUserControl();
            }
            else
            {
                Helper.CurrentUserControl.Content = new ServiceUserControl();
            }
        }
    }
}