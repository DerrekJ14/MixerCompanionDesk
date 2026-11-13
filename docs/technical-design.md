# Technical Design

## Goal

Mixer Companion Desk is built to serve as a future-ready Windows desktop audio control application with a modular, service-oriented design that can evolve from a simple audio device manager into a professional mixer and control surface platform.

## Core Design Principles

### 1. Separation of concerns

The project is divided into projects that align with technical responsibility, not feature grouping alone. This keeps each layer focused and reduces coupling between UI, platform APIs, and persistence.

### 2. Dependency inversion

Higher-level code depends on abstractions, not concrete implementations. This includes:
- `IAudioService`
- `ISettingsService`
- `IAudioRoutingService`

### 3. Replaceable backends

The Audio project can be swapped for alternate audio engines without changing the UI contracts. This is crucial for future hardware control and DSP engine replacements.

### 4. MVVM-first UI

All manageable UI behavior is moved into view models, with WPF controlling only view composition and binding.

### 5. Persistence safety

Settings are stored in JSON in a user-scoped AppData folder to ensure a stable local persistence path without requiring a database.

## Implementation Structure

### Core Project

The Core project is the center of the domain model. It defines reusable contracts and shared entities used by other layers.

Key types:
- `AudioDevice`
- `ApplicationSettings`
- `AudioChannel`
- `AudioRoute`
- `IAudioService`
- `ISettingsService`
- `IAudioRoutingService`

### Audio Project

This project contains the concrete platform implementations for the audio domain.

Current implementations:
- `AudioService` using NAudio
- `AudioRoutingService` using in-memory routing state

This is a natural place for future engines such as:
- DSP engine service
- hardware route service
- bus balancing service
- channel processor pipeline

### Storage Project

This project handles the data access concerns.

Current implementation:
- `SettingsService` stores user settings as JSON

Planned additions:
- profile storage
- session state persistence
- route preset storage

### UI Project

This project is responsible for user interaction and rendering.

Current view model responsibilities:
- device discovery and selection
- output levels and mute handling
- routing channel selection and updates
- settings save/load operations
- status feedback to the user

## Service Responsibilities

### IAudioService

Provides the minimal abstraction for system-level audio device behavior.

Methods:
- `GetPlaybackDevices()`
- `GetRecordingDevices()`
- `GetMasterVolume()`
- `SetMasterVolume(float volume)`
- `SetMute(bool muted)`
- `GetMuteState()`

### ISettingsService

Provides application settings storage and loading.

Methods:
- `Load()`
- `Save(ApplicationSettings settings)`

### IAudioRoutingService

Defines the route and channel abstraction for logical mixer behavior.

Methods:
- `GetChannels()`
- `GetRoutes()`
- `SetChannel(AudioChannel channel)`
- `SetRoute(AudioRoute route)`
- `RemoveRoute(string routeId)`

## Data Flow

The flow of data in the application is intentionally simple:

1. UI binds to view model state
2. view model calls services via interfaces
3. audio or storage implementations perform the work
4. UI updates observable properties to reflect state changes

This keeps the interaction model deterministic and makes future instrumentation easier.

## Future Hardware and Controller Design

The architecture is designed to allow future additions such as:
- MIDI device adapters
- USB serial command interfaces
- ESP32 controller transports
- custom hardware mapping registries

These will likely be added as dedicated services behind new interfaces rather than being embedded directly in the UI.

## Future DSP Architecture

The long-term plan is to move from the current routing placeholder into a formal DSP abstraction. This will likely include:
- gain stages
- mute and solo stages
- EQ or filter processing blocks
- meter readers
- bus-level processing

The modular service structure allows this to evolve without breaking the existing UI contracts.

## Validation Strategy

The project follows a lightweight validation model:
- build the solution on each milestone
- validate with unit tests for the Core and service foundation
- keep models and contracts deterministic
- ensure UI stays thin and test-friendly

## Risks and Mitigations

### Risk: OS-specific audio APIs

Mitigation:
- keep audio access behind `IAudioService`
- isolate NAudio usage to the Audio project
- gracefully handle missing device conditions

### Risk: architecture drift

Mitigation:
- keep the Core project free from UI and infrastructure references
- prefer interfaces over implementation assumptions
- enforce thin view models

### Risk: feature sprawl

Mitigation:
- focus on foundational mixer primitives first
- add future services only when the abstraction is needed
- maintain clean layering as the project evolves
