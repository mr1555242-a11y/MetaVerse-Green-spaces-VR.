# Metaverse Green Spaces VR

**Metaverse Green Spaces VR** is an interactive virtual reality (VR) experience built with **Unity**, **C#**, and the **XR Interaction Toolkit** that transforms environmental restoration from an abstract concept into a direct, step-by-step simulation. Instead of merely telling learners why healthy ecosystems matter, the simulation allows users to witness cause and effect unfold directly before their eyes—from a harsh, degraded landscape to a thriving habitat.

---

## Project Story & Narrative Overview

**Metaverse Green Spaces VR** brings environmental restoration to life by turning complex ecological recovery into an observable, interactive journey. Rather than relying solely on passive explanations, viewers and learners experience how thoughtful land stewardship, reforestation, and water renewal reshape a fragile environment step by step.

The simulation begins in a stressed, degraded landscape—parched, cracked earth stretching across the terrain with only sparse shrubs surviving. Key environmental indicators immediately highlight the severity of the condition:
- **Temperature:** `25°C`
- **Soil Moisture:** `15%`
- **Green Coverage:** `1%`

From this barren starting point, restoration progresses as saplings grow into a lush, vibrant forest canopy and water returns to the environment through a clear stream and falling rain. Real-time metrics track recovery step by step as hydration climbs from **40%** to **55%**, then **95%**, and finally reaches **100%**, moving the ecosystem toward **Habitat Balance: Optimal** and **"Ecosystem Status: Thriving."**

---

## Step-by-Step Demo & Simulation Walkthrough

1. **Stage 0 — Degraded Baseline & Environmental Stress:**
   - Visuals show cracked, dry earth and sparse shrubs under harsh conditions.
   - Core indicators start at **25°C** temperature, **15%** soil moisture, and **1%** green coverage.
2. **Stage 1 — Reforestation, Rainfall & Stream Return (`40%` Hydration):**
   - Young saplings begin growing into a denser forest canopy.
   - Rainfall particles activate and a clear stream begins flowing through the landscape, pushing hydration to **40%**.
3. **Stage 2 — Canopy Expansion & Moisture Recovery (`55%` Hydration):**
   - Vegetation expands further as soil moisture and green coverage rise.
   - Hydration progresses to **55%** with improving water and air indicators.
4. **Stage 3 — Dense Forest & Ecological Stabilization (`95%` Hydration):**
   - A fuller canopy and steady water renewal move the system toward **Optimal / Stable** balance.
   - Hydration climbs to **95%** alongside rising **Water Quality**, **Air Purity**, and **Biodiversity**.
5. **Stage 4 — Full Hydration & Thriving Ecosystem (`100%` Hydration):**
   - Hydration reaches **100%**, unlocking **Habitat Balance: Optimal** and **"Ecosystem Status: Thriving."**
   - The habitat comes alive with **colorful fish swimming in the clear stream**, **blooming flowers**, and a **blue butterfly resting on a purple bloom before taking flight**.

---

## Step-by-Step Development, Challenges & Solutions

### 1. Ecological State & Preset Architecture (`EcosystemState.cs`)
- **What We Did:** Defined the `EcosystemSnapshot` data structure and preset states (`InitialDegradedState()` and `ThrivingState()`) covering temperature (`25°C`), soil moisture (`15%`), green coverage (`1%`), hydration (`40% → 55% → 95% → 100%`), water quality, air purity, biodiversity, habitat balance, and ecosystem status.
- **Challenge:** Preventing scattered numbers, inconsistent labels, or conflicting ecological states across simulation stages.
- **Solution:** Centralized state definitions and presets inside `EcosystemState.cs` using clean C# structs and helper methods.

### 2. Progressive Restoration & Live HUD Controller (`RestorationController.cs`)
- **What We Did:** Built the step-by-step restoration sequence that scales the forest canopy, triggers rainfall and the clear stream, steps hydration through **40% → 55% → 95% → 100%**, reveals biodiversity (`swimming fish`, `blooming flowers`, `blue butterfly`), and updates live UI/HUD text and sliders.
- **Challenge:** Synchronizing smooth visual changes (canopy scale, rain, stream, wildlife/flora) with live metric updates while keeping frame performance steady.
- **Solution:** Implemented time-based coroutine interpolation (`AnimateHydrationTo`) using `Time.deltaTime` and `Mathf.Lerp`, paired with modular `GameObject` and `ParticleSystem` toggles.

### 3. XR Interaction & Simulation Trigger (`VRRestorationTrigger.cs`)
- **What We Did:** Connected XR interaction events (`selectEntered` and `selectExited`) to start or reset the restoration sequence cleanly.
- **Challenge:** Avoiding duplicate event subscriptions, missing interactable components, or null reference errors when toggling objects or scenes.
- **Solution:** Used `[RequireComponent(typeof(XRBaseInteractable))]`, null checks, and clean listener registration/unregistration inside `OnEnable` and `OnDisable`.

---

## Current Role, Future Vision & Global Impact

- **Current Role:** Serves as an interactive educational and awareness tool that helps students, educators, and communities experience cause-and-effect ecological recovery firsthand rather than relying only on static lessons or passive viewing.
- **Future Vision:** Has strong potential to expand into schools, environmental training programs, and sustainability workshops worldwide—incorporating additional biomes (such as mangroves, wetlands, and urban heat islands), interactive conservation choices, and localized climate/environmental data.
- **Global Impact:** By making ecological restoration tangible and measurable, the simulation can help foster stronger environmental awareness, responsible land and water stewardship, and long-term support for sustainable ecosystems and biodiversity.

---

## Technical Architecture & Repository Structure

- **Engine & Language:** Unity, C#
- **XR Framework:** Unity XR Interaction Toolkit
- **UI & Readouts:** TextMeshPro (`TMP_Text`) and UI `Slider`

```text
Assets/
└── Scripts/
    ├── EcosystemState.cs          # Ecological snapshot struct & state presets
    ├── RestorationController.cs   # Step-by-step restoration flow, hydration milestones & HUD updates
    └── VRRestorationTrigger.cs    # XR interaction trigger to start and reset the simulation
