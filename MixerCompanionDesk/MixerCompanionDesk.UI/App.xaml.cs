using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MixerCompanionDesk.Audio.Services;
using MixerCompanionDesk.Core.Interfaces;
using MixerCompanionDesk.Storage.Services;
using MixerCompanionDesk.UI.ViewModels;
using MixerCompanionDesk.UI.Views;

namespace MixerCompanionDesk;

/// <summary>
/// WPF application entry point and dependency injection composition root.
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    /// <inheritdoc />
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IAudioService, AudioService>();
                services.AddSingleton<IAudioRoutingService, AudioRoutingService>();
                services.AddSingleton<ISettingsService, SettingsService>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        await _host.StartAsync();
        MainWindow window = _host.Services.GetRequiredService<MainWindow>();
        await window.ViewModel.InitializeAsync();
        window.Show();
    }

    /// <inheritdoc />
    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
