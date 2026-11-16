namespace MixerCompanionDesk.Core.Models;

/// <summary>
/// Represents a temporary virtual mixer channel used for the visible control surface prototype.
/// </summary>
public sealed class MixerChannel
{
    /// <summary>
    /// Gets or sets the channel identifier.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the display name of the channel.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current channel level as a percentage.
    /// </summary>
    public double Level { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the channel is muted.
    /// </summary>
    public bool IsMuted { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the channel is soloed.
    /// </summary>
    public bool IsSolo { get; set; }
}
