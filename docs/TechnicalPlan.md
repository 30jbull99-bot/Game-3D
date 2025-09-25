# Technical Plan

## Engine & Rendering Stack
- **Unity 2022.3 LTS** with **HDRP** for high-end graphics (deferred rendering, volumetrics, ray tracing optional).
- **Cinemachine** for camera rigs, **Timeline** for cinematics.
- **Shader Graph** for custom lit/FX materials (holograms, force fields, emissive decals).
- **VFX Graph** for muzzle flashes, explosions, energy trails and atmospheric effects.
- **Input System** package for cross-platform bindings (keyboard/mouse, gamepad).

## Code Architecture

### Layered Overview
1. **Presentation Layer** – UI, VFX, Audio. Managed via ScriptableObject configuration and Addressables for streaming.
2. **Gameplay Layer** – Player, AI, weapons, abilities, map editor logic. Uses ECS-style data containers with MonoBehaviour controllers.
3. **Systems Layer** – Game modes, save/load, services (analytics, leaderboards), dependency injection via Zenject (optional).

### Key Systems
- **Player Controller** – CharacterController-based movement with slope handling, sprinting, sliding, mantling.
- **Weapon Framework** – ScriptableObjects define ballistics, recoil, audio. Supports hitscan & projectile weapons, attachments and ability charges.
- **AI Director** – Controls spawn composition, behaviours and pacing (uses Utility AI scoring + Navigation).
- **Campaign Flow** – Manages mission states, dialogue triggers and checkpointing.
- **Survival Manager** – Procedural wave generator with weighted enemy tables and mutators.
- **Map Editor** – Runtime UI built with UI Toolkit; allows placing prefabs, editing terrain heightmaps, baking nav meshes.
- **Save/Progression** – Persistent profiles with unlocks, difficulty progress, cosmetics. Stored via JSON + encryption, cloud sync optional.

## Data Assets
- **WeaponDefinition** – damage, fire rate, recoil curves, animations.
- **EnemyArchetype** – health, behaviour tree reference, loot table.
- **MissionDefinition** – objective graphs, spawn lists, cinematics timeline.
- **SurvivalMutator** – modifiers applied per wave.
- **MapChunk** – modular level pieces for procedural assembly.

## Tooling
- **Addressables** for asset streaming and mod support.
- **Profile Analyzer** and **RenderDoc** for performance profiling.
- **PlasticSCM or Git LFS** for large binary assets.
- **YAML Merge** strategies for Unity scenes/prefabs.

## Networking & Leaderboards
- Survival leaderboards via **PlayFab** or **Steamworks**. Submit wave/time stats with anti-cheat heuristics.
- Optional co-op uses **Unity Netcode for GameObjects** with relay services.

## Build & Deployment
- CI pipeline via GitHub Actions: static analysis (Roslyn analyzers), unit tests (EditMode, PlayMode), build automation.
- Platform targets: Windows (DX12), PlayStation 5, Xbox Series X|S; scalable to Ultra settings + RTX.

## Risk Mitigation
- Prototype core combat loop before investing in high-cost assets.
- Maintain strict performance budgets (frame time < 11 ms on target hardware).
- Use modular data-driven design to support future expansions.

