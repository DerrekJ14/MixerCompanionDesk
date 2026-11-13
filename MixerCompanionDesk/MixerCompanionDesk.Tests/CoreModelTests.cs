using MixerCompanionDesk.Core.Models;
using Xunit;

namespace MixerCompanionDesk.Tests;

public sealed class CoreModelTests
{
    [Fact]
    public void AudioDevice_UsesNameForDisplay()
    {
        var device = new AudioDevice { Id = "device-1", Name = "Studio Output" };

        Assert.Equal("Studio Output", device.ToString());
    }

    [Fact]
    public void ApplicationSettings_HasUsefulDefaults()
    {
        var settings = new ApplicationSettings();

        Assert.Equal(100, settings.MasterVolume);
        Assert.False(settings.Muted);
        Assert.Empty(settings.PlaybackDeviceId);
        Assert.Empty(settings.RecordingDeviceId);
    }
}
