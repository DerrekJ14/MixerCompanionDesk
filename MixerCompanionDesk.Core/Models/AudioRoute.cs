namespace MixerCompanionDesk.Core.Models;

/// <summary>
/// Represents a route from one logical source to a target output bus.
/// </summary>
public sealed class AudioRoute
{
    /// <summary>
    /// Gets or sets the route identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source channel identifier.
    /// </summary>
    public string SourceChannelId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the destination output identifier.
    /// </summary>
    public string DestinationId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the route gain in percent.
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the route is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;
}
