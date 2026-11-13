# Product Roadmap

## Current Phase

### Phase 1: Core Foundation

Status: complete

Implemented:
- solution and project structure
- Core models and contracts
- NAudio-based audio device service
- JSON settings persistence
- WPF MVVM shell
- dark theme UI
- logical routing foundation

## Next Phase

### Phase 2: Mixer Control Surface

Goals:
- enhance the routing matrix
- add bus views and channel strip layout
- add per-channel level, mute, and solo controls
- introduce channel naming and custom profiles
- add session management and route persistence

Planned work:
- route editor UI
- save/load profiles
- virtual bus abstraction
- channel strip view model updates

## Phase 3: DSP and Monitoring

Goals:
- implement DSP stages as first-class services
- add meter models and UI elements
- add gain, pan, EQ, and filter placeholders
- support multiple output targets

Planned work:
- `IDspService` abstraction
- per-channel processing chain
- metering service and visualizer model

## Phase 4: Hardware Integration

Goals:
- support MIDI devices
- support USB serial controllers
- support ESP32-based surfaces
- introduce hardware adapter abstraction layer

Planned work:
- `IHardwareControllerService`
- `IMidiControllerService`
- serial command abstraction
- device binding and mapping UI

## Phase 5: Streaming and Broadcast Features

Goals:
- OBS integration
- Stream Deck integration
- recording output routing
- streaming bus controls

Planned work:
- live source selection
- profile save/load for stream sessions
- broadcast bus mapping

## Phase 6: Professional Workflow Features

Goals:
- session snapshots
- macro actions
- scene switching
- advanced signal path management
- monitoring and automation support

## Success Criteria

A phase is considered successful when:
- it adds a clear user-facing workflow improvement
- it matches a clean architecture boundary
- it remains testable and service-driven
- it supports future controller or DSP extension without UI rewrites
