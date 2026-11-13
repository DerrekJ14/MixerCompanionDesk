namespace MixerCompanionDesk.Core.Models;

/// <summary>
/// Represents an audio endpoint exposed by the operating system.
/// </summary>
public sealed class AudioDevice
{
    /// <summary>Gets the stable endpoint identifier.</summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>Gets the friendly endpoint name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets a value indicating whether this is the system default endpoint.</summary>
    public bool IsDefault { get; init; }

    /// <inheritdoc />
    public override string ToString() => Name;
}
