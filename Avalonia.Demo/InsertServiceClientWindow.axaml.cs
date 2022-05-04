using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Demo.Contracts;
using Avalonia.Demo.Models;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Avalonia.Demo;

public partial class InsertServiceClientWindow : Window
{
    private Service _service;

    public InsertServiceClientWindow()
    {
        InitializeComponent();
    }

    public InsertServiceClientWindow(Service service) : this()
    {
        _service = service;
        DataContext = _service;
        Load();
    }

    private void Load()
    {
        CmbClients.Items = Helper.GetContext().Clients.ToList();
        CmbClients.SelectedIndex = 0;
    }

    private void BtnInsert_OnClick(object sender, RoutedEventArgs e)
    {
        Client client = CmbClients.SelectedItem as Client;
        DateTime date = DateTime.Now;
        if (DpDate.SelectedDate.HasValue && TpTime.SelectedTime.HasValue)
        {
            date = DpDate.SelectedDate.Value.Date.AddTicks(TpTime.SelectedTime.Value.Ticks);
        }

        ClientService clientService = new ClientService()
        {
            Client = client,
            Service = _service,
            Comment = TbComment.Text,
            StartTime = date,
        };
        Helper.GetContext().Add(clientService);
        Helper.GetContext().SaveChanges();
        Close();
    }
}