# Game-3D

This repository contains the source code for **Eclipse Protocol**, a high-end 3D first-person shooter designed for Unity's High Definition Render Pipeline (HDRP). The project blueprint includes:

- AAA-inspired rendering setup with physically based materials, volumetric lighting and post-processing.
- Fully featured main menu with options for Campaign, Endless Survival, Map Editor and Settings.
- Modular player controller, weapon and AI systems designed for quick iteration.
- Campaign progression with handcrafted missions, story beats and cinematic sequences.
- Endless Survival mode with procedural wave generation, difficulty scaling and leaderboard support.
- In-engine Map Editor allowing designers to block out arenas, place enemies, author navigation and export playable levels.

> **Note**
> Due to the size of Unity HDRP projects and the lack of GPU access in this environment, the repository focuses on scripts, configuration assets and documentation required to build the game inside Unity. Import the project into Unity 2022.3 (or later) with HDRP installed to access the full experience.

## Repository Layout

```text
Assets/
  Scripts/
    Combat/           -- Weapons, projectiles, damage handling.
    Enemies/          -- AI behaviours, spawners and boss logic.
    Managers/         -- Game mode orchestration, save data, services.
    MapEditor/        -- Runtime level editor tools.
    Player/           -- First-person controller, abilities and camera.
    UI/               -- Menus, HUD and in-game overlays.
  Shaders/           -- Custom shader graphs (create within Unity).
  Settings/          -- HDRP, Input System and quality presets.
  Scenes/            -- Unity scenes (create inside the editor).
Docs/
  DesignOverview.md  -- High level pitch, art direction and feature roadmap.
  TechnicalPlan.md   -- Systems architecture, data flow and pipelines.
```

## Getting Started

1. Install **Unity Hub** and **Unity 2022.3 LTS** with the **High Definition Render Pipeline** template and **Input System** package.
2. Clone this repository and open it with Unity Hub.
3. When prompted, install the project dependencies (TextMeshPro, Cinemachine, Addressables).
4. Generate HDRP pipeline asset via *Window → Rendering → HDRP Wizard* if missing.
5. Configure default scenes:
   - `MainMenu` – root scene with menu UI and service bootstrapper.
   - `CampaignHub` – level selection, narrative triggers and mission briefings.
   - `SurvivalArena` – procedurally generated arena entry point.
   - `MapEditor` – runtime editor scene with tool panels.

## Building

1. Set *MainMenu* as the first scene in build settings.
2. Ensure HDRP quality settings are configured for target platform (PC / next-gen consoles).
3. Use *File → Build Settings* and choose the desired platform.
4. Enable DLSS/FSR where appropriate via HDRP camera settings.

## License

Released under the MIT License. See [LICENSE](LICENSE) when added.
