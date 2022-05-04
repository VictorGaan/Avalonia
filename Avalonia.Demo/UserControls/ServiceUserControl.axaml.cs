using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Demo.Contracts;
using Avalonia.Demo.Models;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MessageBox.Avalonia;

namespace Avalonia.Demo.UserControls;

public partial class ServiceUserControl : UserControl
{
    private List<string> _sortBy = new List<string>()
    {
        "по возрастанию", "по убыванию"
    };

    private List<string> _filterBy = new List<string>()
    {
        "Все", "от 0 до 5%", "от 10 до 15%", "от 15 до 30%", "от 30 до 70%", "от 70 до 100%"
    };

    public ServiceUserControl()
    {
        InitializeComponent();
        Load();
        LoadData();
    }

    private void Load()
    {
        CmbSort.Items = _sortBy;
        CmbSort.SelectedIndex = 0;
        CmbFilter.Items = _filterBy;
        CmbFilter.SelectedIndex = 0;
    }

    private void LoadData()
    {
        var services = Helper.GetContext().Services.ToList();
        services = CmbSort.SelectedIndex switch
        {
            0 => services.OrderBy(x => x.IsDiscount ? x.DiscountCost : x.Cost).ToList(),
            1 => services.OrderByDescending(x => x.IsDiscount ? x.DiscountCost : x.Cost).ToList(),
            _ => services
        };
        services = CmbFilter.SelectedIndex switch
        {
            0 => services,
            1 => services.Where(x => x.Discount is >= 0 and < 5).ToList(),
            2 => services.Where(x => x.Discount is >= 10 and < 15).ToList(),
            3 => services.Where(x => x.Discount is >= 15 and < 30).ToList(),
            4 => services.Where(x => x.Discount is >= 30 and < 70).ToList(),
            5 => services.Where(x => x.Discount is >= 70 and < 100).ToList(),
            _ => services
        };
        services = services.Where(x =>
            x.Title.Contains(TbSearch.Text ?? "") || (x.Description ?? "").Contains(TbSearch.Text ?? "")).ToList();
        ServiceListBox.Items = services;
        TbCount.Text = $"{services.Count} из {Helper.GetContext().Services.Count()}";
    }

    private void BtnUpdateService_OnClick(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Service service) return;
        Helper.CurrentUserControl.Content = new InsertServiceUserControl(service);
    }

    private void BtnDeleteService_OnClick(object sender, RoutedEventArgs e)
    {
        if ((sender as Button)?.DataContext is not Service service) return;
        if (!service.ClientServices.Any())
        {
            Helper.GetContext().Remove(service);
            Helper.GetContext().SaveChanges();
            LoadData();
        }
        else
        {
            var message = MessageBoxManager.GetMessageBoxStandardWindow("Информация",
                "Удаление не возможно, так как данный сервис используется клиентом.");
            message.Show();
        }
    }

    private void CmbSort_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadData();
    }

    private void CmbFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadData();
    }

    private void TbSearch_OnKeyUp(object sender, KeyEventArgs e)
    {
        LoadData();
    }

    private void BtnInsertServiceClient_OnClick(object sender, RoutedEventArgs e)
    {
        if (ServiceListBox.SelectedItem is Service service)
        {
            new InsertServiceClientWindow(service).Show();
        }
    }
}