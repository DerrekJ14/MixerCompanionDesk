using MixerCompanionDesk.Core.Models;

namespace MixerCompanionDesk.Core.Interfaces;

/// <summary>
/// Provides audio endpoint and system output controls.
/// </summary>
public interface IAudioService
{
    /// <summary>Gets the available playback endpoints.</summary>
    List<AudioDevice> GetPlaybackDevices();

    /// <summary>Gets the available recording endpoints.</summary>
    List<AudioDevice> GetRecordingDevices();

    /// <summary>Gets the master volume as a normalized value from 0 to 1.</summary>
    float GetMasterVolume();

    /// <summary>Sets the master volume using a normalized value from 0 to 1.</summary>
    void SetMasterVolume(float volume);

    /// <summary>Sets the system output mute state.</summary>
    void SetMute(bool muted);

    /// <summary>Gets the current system output mute state.</summary>
    bool GetMuteState();
}
