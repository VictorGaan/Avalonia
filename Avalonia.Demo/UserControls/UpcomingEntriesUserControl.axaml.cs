using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Demo.Contracts;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace Avalonia.Demo.UserControls;

public partial class UpcomingEntriesUserControl : UserControl
{
    private DispatcherTimer _timer;

    public UpcomingEntriesUserControl()
    {
        InitializeComponent();
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(30);
        _timer.Tick += TimerOnTick;
        _timer.Start();
        Load();
    }

    private void Load()
    {
        var date = DateTime.Now;
        ServiceClientGrid.Items = Helper.GetContext().ClientServices.ToList()
            .Where(x => x.StartTime > date || x.StartTime > date.AddDays(1)).ToList();
    }

    private void TimerOnTick(object sender, EventArgs e)
    {
        Load();
    }
}