using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MixerCompanionDesk.Core.Models;

namespace MixerCompanionDesk.UI.ViewModels;

/// <summary>
/// Represents the state and interaction model for a single mixer channel strip.
/// </summary>
public partial class ChannelStripViewModel : ObservableObject
{
    private readonly MixerChannel _channel;

    public ChannelStripViewModel(MixerChannel channel)
    {
        _channel = channel;
        Name = _channel.Name;
        Level = _channel.Level;
        IsMuted = _channel.IsMuted;
        IsSolo = _channel.IsSolo;
    }

    public string Name
    {
        get => _channel.Name;
        set
        {
            if (SetProperty(_channel.Name, value, _channel, (c, v) => c.Name = v))
            {
                OnPropertyChanged();
            }
        }
    }

    public double Level
    {
        get => _channel.Level;
        set
        {
            if (SetProperty(_channel.Level, value, _channel, (c, v) => c.Level = v))
            {
                OnPropertyChanged();
            }
        }
    }

    public bool IsMuted
    {
        get => _channel.IsMuted;
        set
        {
            if (SetProperty(_channel.IsMuted, value, _channel, (c, v) => c.IsMuted = v))
            {
                OnPropertyChanged();
            }
        }
    }

    public bool IsSolo
    {
        get => _channel.IsSolo;
        set
        {
            if (SetProperty(_channel.IsSolo, value, _channel, (c, v) => c.IsSolo = v))
            {
                OnPropertyChanged();
            }
        }
    }

    public MixerChannel Channel => _channel;

    [RelayCommand]
    private void Mute()
    {
        IsMuted = !IsMuted;
    }

    [RelayCommand]
    private void Solo()
    {
        IsSolo = !IsSolo;
    }

    [RelayCommand]
    private void ResetLevel()
    {
        Level = 50;
    }
}
