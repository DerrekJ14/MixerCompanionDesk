using System.Text.Json;
using MixerCompanionDesk.Core.Interfaces;
using MixerCompanionDesk.Core.Models;

namespace MixerCompanionDesk.Storage.Services;

/// <summary>
/// Persists application settings as JSON in the current user's application data folder.
/// </summary>
public sealed class SettingsService : ISettingsService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.General)
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _settingsPath;

    /// <summary>
    /// Initializes a settings service using the standard application data location.
    /// </summary>
    public SettingsService()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _settingsPath = Path.Combine(appDataPath, "MixerCompanionDesk", "settings.json");
    }

    /// <inheritdoc />
    public async Task<ApplicationSettings> Load()
    {
        try
        {
            if (!File.Exists(_settingsPath))
            {
                return new ApplicationSettings();
            }

            await using FileStream stream = File.OpenRead(_settingsPath);
            return await JsonSerializer.DeserializeAsync<ApplicationSettings>(stream, SerializerOptions)
                .ConfigureAwait(false) ?? new ApplicationSettings();
        }
        catch (IOException)
        {
            return new ApplicationSettings();
        }
        catch (UnauthorizedAccessException)
        {
            return new ApplicationSettings();
        }
        catch (JsonException)
        {
            return new ApplicationSettings();
        }
    }

    /// <inheritdoc />
    public async Task Save(ApplicationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        try
        {
            string? directory = Path.GetDirectoryName(_settingsPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using FileStream stream = File.Create(_settingsPath);
            await JsonSerializer.SerializeAsync(stream, settings, SerializerOptions).ConfigureAwait(false);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
