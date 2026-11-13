using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MixerCompanionDesk.Core.Interfaces;
using MixerCompanionDesk.Core.Models;

namespace MixerCompanionDesk.UI.ViewModels;

/// <summary>
/// Coordinates audio device management, routing state, and persisted application state.
/// </summary>
public partial class MainViewModel : ObservableObject
{
    private readonly IAudioService _audioService;
    private readonly ISettingsService _settingsService;
    private readonly IAudioRoutingService _audioRoutingService;
    private bool _isLoading;

    public ObservableCollection<AudioDevice> PlaybackDevices { get; } = [];
    public ObservableCollection<AudioDevice> RecordingDevices { get; } = [];
    public ObservableCollection<AudioChannel> MixerChannels { get; } = [];
    public ObservableCollection<AudioRoute> Routes { get; } = [];

    [ObservableProperty]
    private AudioDevice? selectedPlaybackDevice;

    [ObservableProperty]
    private AudioDevice? selectedRecordingDevice;

    [ObservableProperty]
    private AudioChannel? selectedChannel;

    [ObservableProperty]
    private double masterVolume;

    [ObservableProperty]
    private double channelLevel;

    [ObservableProperty]
    private bool isMuted;

    [ObservableProperty]
    private bool channelMuted;

    [ObservableProperty]
    private string statusText = "Ready";

    public MainViewModel(
        IAudioService audioService,
        ISettingsService settingsService,
        IAudioRoutingService audioRoutingService)
    {
        _audioService = audioService;
        _settingsService = settingsService;
        _audioRoutingService = audioRoutingService;
        RefreshRoutingState();
    }

    public async Task InitializeAsync()
    {
        await LoadSettingsAsync().ConfigureAwait(true);
    }

    private void RefreshRoutingState()
    {
        MixerChannels.Clear();
        foreach (AudioChannel channel in _audioRoutingService.GetChannels())
        {
            MixerChannels.Add(channel);
        }

        Routes.Clear();
        foreach (AudioRoute route in _audioRoutingService.GetRoutes())
        {
            Routes.Add(route);
        }

        SelectedChannel ??= MixerChannels.FirstOrDefault();
        if (SelectedChannel is not null)
        {
            ChannelLevel = SelectedChannel.Level;
            ChannelMuted = SelectedChannel.IsMuted;
        }
    }

    partial void OnSelectedChannelChanged(AudioChannel? value)
    {
        if (value is null)
        {
            return;
        }

        ChannelLevel = value.Level;
        ChannelMuted = value.IsMuted;
    }

    partial void OnChannelLevelChanged(double value)
    {
        if (SelectedChannel is null)
        {
            return;
        }

        SelectedChannel.Level = Math.Clamp(value, 0, 100);
        RefreshRoutingState();
    }

    partial void OnChannelMutedChanged(bool value)
    {
        if (SelectedChannel is null)
        {
            return;
        }

        SelectedChannel.IsMuted = value;
        RefreshRoutingState();
    }

    [RelayCommand]
    private void RefreshRoutingCommand()
    {
        RefreshRoutingState();
        StatusText = "Routing updated.";
    }

    [RelayCommand]
    private void ToggleChannelMute()
    {
        if (SelectedChannel is null)
        {
            return;
        }

        SelectedChannel.IsMuted = !SelectedChannel.IsMuted;
        ChannelMuted = SelectedChannel.IsMuted;
        RefreshRoutingState();
    }

    [RelayCommand]
    private void SetMainRoute()
    {
        if (SelectedChannel is null)
        {
            return;
        }

        _audioRoutingService.SetRoute(new AudioRoute
        {
            Id = $"route-{SelectedChannel.Id}",
            SourceChannelId = SelectedChannel.Id,
            DestinationId = "main-bus",
            Gain = SelectedChannel.Level,
            IsEnabled = !SelectedChannel.IsMuted
        });

        RefreshRoutingState();
        StatusText = $"Route updated for {SelectedChannel.Name}.";
    }

    [RelayCommand]
    private void AddDemoRoute()
    {
        _audioRoutingService.SetRoute(new AudioRoute
        {
            Id = "route-demo",
            SourceChannelId = "ch-game",
            DestinationId = "stream-bus",
            Gain = 75,
            IsEnabled = true
        });

        RefreshRoutingState();
        StatusText = "Demo route added.";
    }

    [RelayCommand]
    private async Task RefreshDevicesAsync()
    {
        try
        {
            List<AudioDevice> playbackDevices = _audioService.GetPlaybackDevices();
            List<AudioDevice> recordingDevices = _audioService.GetRecordingDevices();

            PlaybackDevices.Clear();
            foreach (AudioDevice device in playbackDevices)
            {
                PlaybackDevices.Add(device);
            }

            RecordingDevices.Clear();
            foreach (AudioDevice device in recordingDevices)
            {
                RecordingDevices.Add(device);
            }

            SelectedPlaybackDevice ??= PlaybackDevices.FirstOrDefault(device => device.IsDefault) ?? PlaybackDevices.FirstOrDefault();
            SelectedRecordingDevice ??= RecordingDevices.FirstOrDefault(device => device.IsDefault) ?? RecordingDevices.FirstOrDefault();
            StatusText = $"Found {PlaybackDevices.Count} playback and {RecordingDevices.Count} recording device(s).";
            await SaveSettingsAsync().ConfigureAwait(true);
        }
        catch (Exception ex) when (ex is COMException or InvalidOperationException)
        {
            StatusText = $"Audio devices unavailable: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task MuteAsync()
    {
        try
        {
            _audioService.SetMute(IsMuted);
            StatusText = IsMuted ? "Output muted." : "Output unmuted.";
            await SaveSettingsAsync().ConfigureAwait(true);
        }
        catch (InvalidOperationException ex)
        {
            StatusText = ex.Message;
        }
    }

    [RelayCommand]
    private async Task SaveSettingsAsync()
    {
        if (_isLoading)
        {
            return;
        }

        await _settingsService.Save(CreateSettings()).ConfigureAwait(true);
        StatusText = "Settings saved.";
    }

    [RelayCommand]
    private async Task LoadSettingsAsync()
    {
        _isLoading = true;
        try
        {
            ApplicationSettings settings = await _settingsService.Load().ConfigureAwait(true);
            MasterVolume = Math.Clamp(settings.MasterVolume, 0, 100);
            IsMuted = settings.Muted;

            await RefreshDevicesAsync().ConfigureAwait(true);

            SelectedPlaybackDevice = PlaybackDevices.FirstOrDefault(device => device.Id == settings.PlaybackDeviceId)
                ?? SelectedPlaybackDevice;
            SelectedRecordingDevice = RecordingDevices.FirstOrDefault(device => device.Id == settings.RecordingDeviceId)
                ?? SelectedRecordingDevice;

            _audioService.SetMasterVolume((float)(MasterVolume / 100));
            _audioService.SetMute(IsMuted);
            StatusText = "Settings loaded.";
        }
        catch (InvalidOperationException ex)
        {
            StatusText = ex.Message;
        }
        finally
        {
            _isLoading = false;
        }
    }

    partial void OnMasterVolumeChanged(double value)
    {
        if (_isLoading)
        {
            return;
        }

        try
        {
            _audioService.SetMasterVolume((float)(value / 100));
            _ = SaveSettingsAsync();
        }
        catch (InvalidOperationException ex)
        {
            StatusText = ex.Message;
        }
    }

    partial void OnSelectedPlaybackDeviceChanged(AudioDevice? value)
    {
        if (!_isLoading)
        {
            _ = SaveSettingsAsync();
        }
    }

    partial void OnSelectedRecordingDeviceChanged(AudioDevice? value)
    {
        if (!_isLoading)
        {
            _ = SaveSettingsAsync();
        }
    }

    private ApplicationSettings CreateSettings() => new()
    {
        PlaybackDeviceId = SelectedPlaybackDevice?.Id ?? string.Empty,
        RecordingDeviceId = SelectedRecordingDevice?.Id ?? string.Empty,
        MasterVolume = MasterVolume,
        Muted = IsMuted
    };
}
