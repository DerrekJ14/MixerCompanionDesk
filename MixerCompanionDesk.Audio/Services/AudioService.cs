using System.Runtime.InteropServices;
using MixerCompanionDesk.Core.Interfaces;
using MixerCompanionDesk.Core.Models;
using NAudio.CoreAudioApi;

namespace MixerCompanionDesk.Audio.Services;

/// <summary>
/// Provides Windows audio endpoint operations through NAudio.
/// </summary>
public sealed class AudioService : IAudioService
{
    /// <inheritdoc />
    public List<AudioDevice> GetPlaybackDevices() => GetDevices(DataFlow.Render);

    /// <inheritdoc />
    public List<AudioDevice> GetRecordingDevices() => GetDevices(DataFlow.Capture);

    /// <inheritdoc />
    public float GetMasterVolume()
    {
        using MMDevice device = GetDefaultPlaybackDevice();
        return (float)device.AudioEndpointVolume.MasterVolumeLevelScalar;
    }

    /// <inheritdoc />
    public void SetMasterVolume(float volume)
    {
        using MMDevice device = GetDefaultPlaybackDevice();
        device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Clamp(volume, 0f, 1f);
    }

    /// <inheritdoc />
    public void SetMute(bool muted)
    {
        using MMDevice device = GetDefaultPlaybackDevice();
        device.AudioEndpointVolume.Mute = muted;
    }

    /// <inheritdoc />
    public bool GetMuteState()
    {
        using MMDevice device = GetDefaultPlaybackDevice();
        return device.AudioEndpointVolume.Mute;
    }

    private static List<AudioDevice> GetDevices(DataFlow dataFlow)
    {
        using var enumerator = new MMDeviceEnumerator();
        string? defaultDeviceId = null;
        using (MMDevice? defaultDevice = TryGetDefaultDevice(enumerator, dataFlow))
        {
            defaultDeviceId = defaultDevice?.ID;
        }

        return enumerator.EnumerateAudioEndPoints(dataFlow, DeviceState.Active)
            .Select(device =>
            {
                using (device)
                {
                    return new AudioDevice
                    {
                        Id = device.ID,
                        Name = device.FriendlyName,
                        IsDefault = string.Equals(device.ID, defaultDeviceId, StringComparison.OrdinalIgnoreCase)
                    };
                }
            })
            .ToList();
    }

    private static MMDevice GetDefaultPlaybackDevice()
    {
        using var enumerator = new MMDeviceEnumerator();
        return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    }

    private static MMDevice? TryGetDefaultDevice(MMDeviceEnumerator enumerator, DataFlow dataFlow)
    {
        try
        {
            return enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia);
        }
        catch (COMException)
        {
            return null;
        }
    }
}
