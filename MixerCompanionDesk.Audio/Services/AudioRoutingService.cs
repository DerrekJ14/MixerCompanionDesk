using MixerCompanionDesk.Core.Interfaces;
using MixerCompanionDesk.Core.Models;

namespace MixerCompanionDesk.Audio.Services;

/// <summary>
/// Provides an in-memory routing foundation for virtual mixer channels and routes.
/// </summary>
public sealed class AudioRoutingService : IAudioRoutingService
{
    private readonly Dictionary<string, AudioChannel> _channels = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, AudioRoute> _routes = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes the routing service with default starter channels.
    /// </summary>
    public AudioRoutingService()
    {
        SetChannel(new AudioChannel { Id = "ch-main", Name = "Main", Level = 100, IsMuted = false });
        SetChannel(new AudioChannel { Id = "ch-game", Name = "Game", Level = 85, IsMuted = false });
        SetChannel(new AudioChannel { Id = "ch-chat", Name = "Chat", Level = 70, IsMuted = false });
        SetChannel(new AudioChannel { Id = "ch-mic", Name = "Mic", Level = 80, IsMuted = false });

        SetRoute(new AudioRoute { Id = "route-game", SourceChannelId = "ch-game", DestinationId = "main-bus", Gain = 100, IsEnabled = true });
        SetRoute(new AudioRoute { Id = "route-chat", SourceChannelId = "ch-chat", DestinationId = "main-bus", Gain = 80, IsEnabled = true });
        SetRoute(new AudioRoute { Id = "route-mic", SourceChannelId = "ch-mic", DestinationId = "main-bus", Gain = 90, IsEnabled = true });
    }

    /// <inheritdoc />
    public IReadOnlyList<AudioChannel> GetChannels() => _channels.Values.ToList();

    /// <inheritdoc />
    public IReadOnlyList<AudioRoute> GetRoutes() => _routes.Values.ToList();

    /// <inheritdoc />
    public void SetChannel(AudioChannel channel)
    {
        ArgumentNullException.ThrowIfNull(channel);
        if (string.IsNullOrWhiteSpace(channel.Id))
        {
            throw new ArgumentException("Channel Id is required.", nameof(channel));
        }

        _channels[channel.Id] = channel;
    }

    /// <inheritdoc />
    public void SetRoute(AudioRoute route)
    {
        ArgumentNullException.ThrowIfNull(route);
        if (string.IsNullOrWhiteSpace(route.Id))
        {
            throw new ArgumentException("Route Id is required.", nameof(route));
        }

        _routes[route.Id] = route;
    }

    /// <inheritdoc />
    public void RemoveRoute(string routeId)
    {
        if (!string.IsNullOrWhiteSpace(routeId))
        {
            _routes.Remove(routeId);
        }
    }
}
