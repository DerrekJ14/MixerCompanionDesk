namespace MixerCompanionDesk.Core.Models;

/// <summary>
/// Represents a virtual mixer channel used for routing and level control.
/// </summary>
public sealed class AudioChannel
{
    /// <summary>
    /// Gets or sets the unique channel identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name for the channel.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current volume level in percent.
    /// </summary>
    public double Level { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the channel is muted.
    /// </summary>
    public bool IsMuted { get; set; }
}
