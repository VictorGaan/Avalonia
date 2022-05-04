using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Demo.Models;
using Avalonia.Dialogs;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.VisualTree;
using MessageBox.Avalonia;
using Metsys.Bson;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Helper = Avalonia.Demo.Contracts.Helper;

namespace Avalonia.Demo.UserControls;

public partial class InsertServiceUserControl : UserControl
{
    private Service _service;

    public InsertServiceUserControl()
    {
        InitializeComponent();
        BtnBack.IsVisible = false;
        DataContext = _service = new Service();
    }

    public InsertServiceUserControl(Service service)
    {
        InitializeComponent();
        _service = service;
        DataContext = _service;
    }

    private async void BtnAttach_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Png files", Extensions = { "*" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Jpeg files", Extensions = { "jpeg" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Jpg files", Extensions = { "jpg" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Png files", Extensions = { "png" } });
        string[] result = null;
        result = await openFileDialog.ShowAsync(new Window());
        if (result is not null)
        {
            try
            {
                string image = result.FirstOrDefault();
                string name = GetFileName(image);
                CopyImage(image, name);

                _service.MainImagePath = $"Images/{name}";
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                var asset = assets.Open(new Uri($"avares://Avalonia.Demo/Images/{name}"));
                Bitmap bitmap = new Bitmap(asset);
                MainImage.Source = bitmap;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }

    private void BtnDeleteServicePhoto_OnClick(object sender, RoutedEventArgs e)
    {
        if (PhotosListBox.SelectedItem is ServicePhoto servicePhoto)
        {
            _service.ServicePhotos.Remove(servicePhoto);
            PhotosListBox.Items = _service.ServicePhotos.ToList();
        }
    }

    private async void BtnInsertServicePhoto_OnClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Png files", Extensions = { "*" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Jpeg files", Extensions = { "jpeg" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Jpg files", Extensions = { "jpg" } });
        openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Png files", Extensions = { "png" } });
        string[] result = null;
        result = await openFileDialog.ShowAsync(new Window());
        if (result is not null)
        {
            string image = result.FirstOrDefault();
            if (image is not null)
            {
                try
                {
                    string name = GetFileName(image);
                    CopyImage(image, name);
                    _service.ServicePhotos.Add(new ServicePhoto { PhotoPath = "Images/" + name });
                    PhotosListBox.Items = _service.ServicePhotos.ToList();
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
            }
        }
    }

    private string GetFileName(string path)
    {
        string[] imageParameters = path.Split('/');
        return imageParameters[^1];
    }

    private void CopyImage(string path, string name)
    {
        string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../.."));
        File.Copy(path, $"{projectDir}/Images/{name}", true);
    }

    private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
    {
        var duplicate = Helper.GetContext().Services.Any(x => x.Title == _service.Title);
        if (duplicate)
        {
            if (_service.Id == 0)
            {
                var message = MessageBoxManager.GetMessageBoxStandardWindow("Информация",
                    "Данный сервис есть в системе.");
                message.Show();
                return;
            }
        }

        if (_service.Id == 0)
        {
            Helper.GetContext().Add(_service);
        }

        Helper.GetContext().SaveChanges();
        Helper.CurrentUserControl.Content = new ServiceUserControl();
    }

    private void BtnBack_OnClick(object sender, RoutedEventArgs e)
    {
        Helper.CurrentUserControl.Content = new ServiceUserControl();
    }
}