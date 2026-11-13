using MixerCompanionDesk.Core.Models;

namespace MixerCompanionDesk.Core.Interfaces;

/// <summary>
/// Loads and persists application settings.
/// </summary>
public interface ISettingsService
{
    /// <summary>Loads settings or returns defaults when no settings exist.</summary>
    Task<ApplicationSettings> Load();

    /// <summary>Persists the supplied settings.</summary>
    Task Save(ApplicationSettings settings);
}
