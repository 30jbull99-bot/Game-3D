# Eclipse Protocol – Design Overview

## Vision Statement
Eclipse Protocol is a cinematic, high-intensity first-person shooter that fuses narrative-driven campaigns with endlessly replayable combat arenas. Players take the role of **Spectre**, an elite operative tasked with neutralizing a rogue AI army that has seized control of off-world colonies. The experience blends tight shooting mechanics, hero-style abilities and dynamic level destruction, all rendered with cutting-edge graphics powered by Unity HDRP.

## Core Pillars

1. **High-Fidelity Action** – Photorealistic environments, volumetric lighting and reactive materials make every encounter feel visceral.
2. **Player Agency** – Modular loadouts, upgradeable abilities and map editor tools empower players to craft their own experience.
3. **Replayability** – Procedural wave generation, mutators and seasonal challenges keep survival mode fresh.
4. **Immersive Narrative** – Cinematic storytelling with branching dialogue, in-engine cutscenes and performance capture elevate the campaign.

## Game Modes

### Campaign
- 8 story missions across diverse biomes (desert colonies, orbital stations, subterranean labs).
- Mission structure: **Briefing → Insertion → Combat → Boss → Extraction**.
- Optional objectives unlock lore intel, cosmetics and weapon variants.
- Difficulty scaling via adaptive AI director and dynamic encounter pacing.

### Endless Survival
- Wave-based arenas with procedurally spawning enemies, hazards and power-ups.
- Mutators (fog, low gravity, bullet sponge) refresh gameplay daily.
- Global and friends leaderboards with anti-cheat validation.
- Seasonal reward track with exclusive cosmetics and weapon skins.

### Map Editor
- Grid/spline-based level blockout tools with snapping and prefab libraries.
- Supports lighting probes, reflection capture and navigation baking.
- Share creations via cloud backend (Addressables + PlayFab/Steam Workshop).
- Supports scripting simple encounter logic via visual graph (Unity Visual Scripting integration).

## Art Direction
- Photorealistic grounded sci-fi, with brutalist architecture and holographic UI accents.
- Color palette: deep blues, tungsten highlights, neon emissives for interactables.
- Character design mixes tactical gear with advanced exosuits and hard-surface armor.
- Soundscape: hybrid orchestral/industrial soundtrack, reactive music system.

## Player Experience Goals
- Empower players with precise controls and satisfying feedback (muzzle flash, recoil, hit markers).
- Provide sense of progression through upgrades, weapon mastery and narrative arcs.
- Encourage creativity via map editor and customizable loadouts.

## Roadmap Highlights
| Milestone | Focus | Key Deliverables |
|-----------|-------|------------------|
| Pre-Alpha | Core systems | Player controller, shooting, AI prototype, basic survival loop |
| Alpha     | Content expansion | Campaign mission greyboxes, main menu, map editor v1 |
| Beta      | Polish & UX | HDRP lighting pass, cinematics, full UI/UX, optimization |
| Launch    | Live ops | Leaderboards, seasonal events, mod support |

