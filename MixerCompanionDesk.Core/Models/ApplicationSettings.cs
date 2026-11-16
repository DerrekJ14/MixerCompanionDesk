namespace MixerCompanionDesk.Core.Models;

/// <summary>
/// User preferences persisted by the application.
/// </summary>
public sealed class ApplicationSettings
{
    /// <summary>Gets or sets the selected playback endpoint identifier.</summary>
    public string PlaybackDeviceId { get; set; } = string.Empty;

    /// <summary>Gets or sets the selected recording endpoint identifier.</summary>
    public string RecordingDeviceId { get; set; } = string.Empty;

    /// <summary>Gets or sets the master volume as a percentage from 0 to 100.</summary>
    public double MasterVolume { get; set; } = 100;

    /// <summary>Gets or sets a value indicating whether output is muted.</summary>
    public bool Muted { get; set; }
}
