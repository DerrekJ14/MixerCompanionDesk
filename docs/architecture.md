# Architecture Overview

## Overview

Mixer Companion Desk is being built as a Windows desktop audio control platform that follows Clean Architecture principles and is designed to evolve from a simple device manager into a full mixer, routing matrix, DSP control surface, and hardware integration platform.

The architecture separates concerns across multiple projects so the UI can remain focused on interaction, while lower layers handle audio device access, persistence, and future system integrations.

## Layers

### Core

The Core project contains the application domain model and abstractions. It is intentionally dependency-free from infrastructure, UI, and operating-system libraries.

Responsibilities:
- domain models
- contracts / interfaces
- shared application rules
- future abstractions for DSP, hardware, and profile management

### Audio

The Audio project implements OS-level audio behavior behind the Core contracts. This provides a clean boundary for future engine replacement without changing the UI.

Responsibilities:
- NAudio access for Windows audio devices
- master volume and mute
- device enumeration
- routing service implementation
- future DSP and bus logic

### Storage

The Storage project persists settings and future profiles. It keeps file access and serialization isolated from the UI and the system audio layer.

Responsibilities:
- settings persistence
- schema-safe JSON serialization
- future project and profile persistence

### UI

The UI project contains the WPF application shell and MVVM view models. It depends on interfaces from Core and is intentionally agnostic to implementation details.

Responsibilities:
- view models
- views
- dark theme styling
- WPF binding and command behavior

## Dependency Direction

The dependency direction is controlled by Clean Architecture:

- UI depends on Core
- Audio depends on Core
- Storage depends on Core
- UI does not directly access NAudio
- business logic does not live in the views
- interface contracts drive implementation choices

## Repository and Solution Roots

- Repository Root: G:\CodeLibrary\MixerCompanionDesk
- Solution File: G:\CodeLibrary\MixerCompanionDesk\MixerCompanionDesk.slnx

All source code changes must occur under the repository root. Generated folders such as `bin/`, `obj/`, and `.vs/` are not source roots and should not be treated as project locations.

## Current Source Structure

```text
G:\CodeLibrary\MixerCompanionDesk
├── .gitignore
├── .gitattributes
├── README.md
├── MixerCompanionDesk.slnx
├── docs/
│   ├── architecture.md
│   ├── roadmap.md
│   └── technical-design.md
├── MixerCompanionDesk.Core/
│   ├── MixerCompanionDesk.Core.csproj
│   ├── Interfaces/
│   │   ├── IAudioRoutingService.cs
│   │   ├── IAudioService.cs
│   │   └── ISettingsService.cs
│   └── Models/
│       ├── ApplicationSettings.cs
│       ├── AudioChannel.cs
│       ├── AudioDevice.cs
│       ├── AudioRoute.cs
│       └── MixerChannel.cs
├── MixerCompanionDesk.Audio/
│   ├── MixerCompanionDesk.Audio.csproj
│   └── Services/
│       ├── AudioRoutingService.cs
│       └── AudioService.cs
├── MixerCompanionDesk.Storage/
│   ├── MixerCompanionDesk.Storage.csproj
│   └── Services/
│       └── SettingsService.cs
├── MixerCompanionDesk.UI/
│   ├── MixerCompanionDesk.UI.csproj
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── Controls/
│   │   └── ChannelStrip.xaml
│   ├── Resources/
│   │   └── DarkTheme.xaml
│   ├── ViewModels/
│   │   ├── ChannelStripViewModel.cs
│   │   └── MainViewModel.cs
│   └── Views/
│       └── MainWindow.xaml
└── MixerCompanionDesk.Tests/
    ├── MixerCompanionDesk.Tests.csproj
    └── CoreModelTests.cs
```

## File Creation Guardrails

Before creating new files, always perform the following checks:

- Identify the active repository root and solution file.
- Confirm the target file belongs to an existing project under the repository root.
- Verify the target file is included in the corresponding `.csproj`.
- Report the exact path being modified.
- Do not create duplicate files in alternate directory trees.
- Do not add source files under generated folders such as `bin/`, `obj/`, or `.vs/`.

## DI Composition Root

Dependency injection is configured in the WPF `App` class using `Host.CreateDefaultBuilder()`. This keeps the app startup configuration centralized and makes services replaceable.

Registered services include:
- `IAudioService`
- `IAudioRoutingService`
- `ISettingsService`
- `MainViewModel`
- `MainWindow`

## MVVM Model

The application uses CommunityToolkit.Mvvm and the ObservableObject / RelayCommand pattern.

This enables:
- observable properties for device, channel, and route state
- command-based interaction from the UI
- reduced boilerplate for view model logic
- maintainable behavior for settings and routing updates

## Domain Concepts

### Audio Device Management

The app tracks available playback and recording endpoints and supports changing the default output and reading the current master state.

### Routing Foundation

The routing layer introduces logical source-to-bus mapping between channels and destination buses. This provides a migration path into a larger matrix UI without forcing the UI to know the implementation details.

### Mixer Channel Model

A channel represents a logical audio source with a name, level, and mute state. This structure can later grow into a full DSP pipeline model.

### Profiles and Settings

The application stores saved state in JSON so the user experience can restore device selection and output state on app start.

## Future Evolution

This architecture supports a gradual expansion into:
- audio bus and route matrix management
- DSP blocks and signal processing stages
- per-channel metering
- MIDI and hardware mapping
- OBS, Stream Deck, and broadcast integrations
- custom USB serial and ESP32 surface control

## Design Goals

- extensible without breaking UI contracts
- low-coupling, interface-driven implementation
- grounded in a proper digital audio workstation mental model
- scalable from a device manager to a professional control surface


## Source File Creation Rules

Before creating or modifying any file:

- Identify the active solution file.
- Identify the owning project (*.csproj).
- Identify the physical folder containing that project.
- Report the full file path that will be modified.
- Verify the path is under the active solution root.
- Never create source files based on inferred paths. Always verify the target path exists in the active project structure.