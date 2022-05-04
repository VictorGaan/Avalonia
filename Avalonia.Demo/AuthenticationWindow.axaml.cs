using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Demo.Contracts;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Avalonia.Demo;

public partial class AuthenticationWindow : Window
{
    public AuthenticationWindow()
    {
        InitializeComponent();
    }

    private void BtnGuest_OnClick(object sender, RoutedEventArgs e)
    {
        Helper.CurrentRole = Roles.Guest;
        new MainWindow().Show();
        Close();
    }

    private void BtnAdmin_OnClick(object sender, RoutedEventArgs e)
    {
        if (TbCode.Text == "0000")
        {
            Helper.CurrentRole = Roles.Admin;
            new MainWindow().Show();
            Close();
        }
    }
}