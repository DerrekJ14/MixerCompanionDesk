# Mixer Companion Desk

Mixer Companion Desk is a Windows desktop audio control platform designed as a modern, extensible mixer and routing workstation for live audio, streaming, recording, and hardware integration.

## Purpose

The project began as a small audio device manager and has evolved into an architecture for a professional audio control surface inspired by products such as Voicemeeter, Elgato Wave Link, TotalMix, and digital mixer control surfaces.

The long-term direction includes:

- audio device management
- routing matrix control
- virtual mixer channels
- metering and level monitoring
- DSP processing blocks
- profile management
- MIDI controller support
- USB serial hardware control
- ESP32 surface support
- OBS and Stream Deck integrations
- multi-output routing for streaming and recording

## Architectural Principles

- Clean Architecture boundaries between UI, Core, Audio, and Storage
- UI never directly communicates with NAudio or OS-level audio APIs
- all business logic is behind interfaces
- dependency injection is used throughout the app
- audio engine implementations are replaceable without changing UI code
- MVVM is used for all views and view models
- CommunityToolkit.Mvvm is used for observable properties and commands

## Solution Structure

```text
MixerCompanionDesk/
├── MixerCompanionDesk.slnx
├── README.md
├── MixerCompanionDesk.Core/
│   ├── Interfaces/
│   └── Models/
├── MixerCompanionDesk.Audio/
│   └── Services/
├── MixerCompanionDesk.Storage/
│   └── Services/
├── MixerCompanionDesk.UI/
│   ├── Resources/
│   ├── ViewModels/
│   ├── Views/
│   ├── App.xaml
│   └── App.xaml.cs
├── MixerCompanionDesk.Tests/
│   └── ...
└── MixerCompanionDesk/
	└── legacy project entry preserved during transition
```

## Projects

### MixerCompanionDesk.Core

Purpose:
- shared domain models
- interface contracts
- application-level abstractions

Current responsibilities:
- `AudioDevice`
- `ApplicationSettings`
- `AudioChannel`
- `AudioRoute`
- `IAudioService`
- `ISettingsService`
- `IAudioRoutingService`

### MixerCompanionDesk.Audio

Purpose:
- implementations of audio services and engines
- isolation from UI and storage concerns

Current responsibilities:
- NAudio-backed system audio device enumeration
- playback and recording device access
- master volume and mute control
- in-memory virtual routing foundation

### MixerCompanionDesk.Storage

Purpose:
- persistence for settings and future profiles

Current responsibilities:
- JSON settings persistence in `%APPDATA%\MixerCompanionDesk\settings.json`
- safe loading defaults when the config is missing or invalid

### MixerCompanionDesk.UI

Purpose:
- desktop interaction layer
- WPF views and MVVM logic

Current responsibilities:
- dark, mixer-inspired interface
- device selection and master volume control
- virtual channel and routing panel
- DI-driven app startup

### MixerCompanionDesk.Tests

Purpose:
- validation of domain model and service behavior

Current responsibilities:
- Core model validation
- service-level smoke tests for the routing layer

## Core Domain Model

### AudioDevice

Represents a Windows audio endpoint.

Properties:
- `Id`
- `Name`
- `IsDefault`

### ApplicationSettings

Represents saved application settings.

Properties:
- `PlaybackDeviceId`
- `RecordingDeviceId`
- `MasterVolume`
- `Muted`

### AudioChannel

Represents a logical mixer channel.

Properties:
- `Id`
- `Name`
- `Level`
- `IsMuted`

### AudioRoute

Represents a logical mapping from source channel to destination bus.

Properties:
- `Id`
- `SourceChannelId`
- `DestinationId`
- `Gain`
- `IsEnabled`

## Dependency Injection

The WPF application uses `Microsoft.Extensions.Hosting` and `Microsoft.Extensions.DependencyInjection` to compose services.

Registered services include:
- `IAudioService` -> `AudioService`
- `IAudioRoutingService` -> `AudioRoutingService`
- `ISettingsService` -> `SettingsService`
- `MainViewModel`
- `MainWindow`

## UI Architecture

The UI is intentionally thin and follows MVVM.

Responsibilities:
- bind to observable view model state
- expose commands for device refresh, mute, save/load settings, and routing updates
- remain independent from the actual audio engine implementation

## Routing Architecture

The routing layer is a logical stepping stone toward a larger mixer engine.

Current design:
- a service interface provides virtual channel and route access
- route data is stored in memory in `AudioRoutingService`
- UI binds to the logical channel list and route list
- future routing engines can replace the in-memory version without changing UI contracts

## Future Roadmap

### Phase 2: Mixer and DSP Layer

- named buses and outputs
- per-channel gain, pan, and EQ placeholders
- meter models and streaming telemetry
- session profile definitions

### Phase 3: Hardware and Controller Integration

- MIDI control discovery
- USB serial device support
- ESP32-based hardware surfaces
- custom hardware abstraction layer

### Phase 4: Streaming and Broadcast Features

- OBS source control
- Stream Deck integration
- monitoring and recording bus routing
- stream profile presets

## Quality Standards

- .NET 8 target
- nullable reference types enabled
- clean separation of concerns
- XML documentation on public members
- dependency injection throughout
- tests for foundational service behavior

## Current State

The project is in an early but foundational state. It includes the architecture, UI shell, device control foundation, persistent settings, and a logical routing abstraction that can evolve into a full digital mixer and hardware control platform.
