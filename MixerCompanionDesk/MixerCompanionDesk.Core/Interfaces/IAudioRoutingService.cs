using MixerCompanionDesk.Core.Models;

namespace MixerCompanionDesk.Core.Interfaces;

/// <summary>
/// Provides logical routing, channel, and bus management for the mixer foundation.
/// </summary>
public interface IAudioRoutingService
{
    /// <summary>
    /// Gets the available virtual mixer channels.
    /// </summary>
    IReadOnlyList<AudioChannel> GetChannels();

    /// <summary>
    /// Gets the configured routes.
    /// </summary>
    IReadOnlyList<AudioRoute> GetRoutes();

    /// <summary>
    /// Creates or updates a virtual channel.
    /// </summary>
    void SetChannel(AudioChannel channel);

    /// <summary>
    /// Creates or updates a route.
    /// </summary>
    void SetRoute(AudioRoute route);

    /// <summary>
    /// Removes a route by identifier.
    /// </summary>
    void RemoveRoute(string routeId);
}
