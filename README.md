# Blue Drifter

> A first-person bounty-hunting game where the hard part isn't the shooting — it's making sure
> you've found the right person.

> ⚠️ **Attribution — this is not my design.** Blue Drifter is a student re-implementation of a
> game concept created by [Sokpop Collective](https://sokpop.co). The design is theirs: the
> bounty-hunting loop, and the central idea that you are given a name rather than a face and
> must identify your target before engaging. This was built as a coursework exercise in
> reproducing an existing design's systems in Unity. **The code and systems implementation
> below are my own work; the game design is not.**

**Platform:** PC / WebGL · **Engine:** Unity · **Language:** C# · **Role:** Solo
**Module:** Games Programming 1, Coursework 1 — MSc Computer Games Programming

---

## The Loop

You take a contract from a terminal. It gives you two things: a **name** and a **destination**.
It does not give you a face.

So you drive out, and then you have to *find* them. Raising your binoculars puts scanner marks
over every NPC in view — anonymous until you hold on one long enough to resolve it into a name.
The crowd looks identical without the optics. With them, one of those marks is your contract and
the rest are civilians and armed guards who will shoot back if you make yourself a problem.

Once it's done, you collect the target's **ID** as proof and return it. The game's central
verb is *identification*, not elimination — the weapon is the easy part, and the binoculars are
the actual tool.

---

## Systems

### Target identification

`uiMarksScript` drives a world-space canvas above each NPC that continuously billboards toward
the player — `Quaternion.LookRotation` on the flattened vector to the player so the label
stays readable from any angle without tilting.

The marks are **only rendered while the player is both holding binoculars and aiming**, checked
against the weapon state machine each frame:

```csharp
if (player.GetComponent<PlayerWeaponScript>().weaponState == PlayerWeaponScript.Weaponstate.Binoculars
    && player.GetComponent<PlayerWeaponScript>().aiming)
```

Before an NPC is scanned it shows an anonymous mark; after, the mark is replaced by their name.
That single swap is the whole identification mechanic — information, not combat, is what the
player is acquiring.

### Binoculars

`binocularScript` interpolates the model between a hip position and a raised position using a
normalised `t` clamped per-frame, with `Vector3.Lerp` for translation and `Quaternion.Slerp`
for rotation, so raising and lowering are one continuous animation driven by a single value
rather than an animation clip. At full extension the mesh renderer is disabled — preventing the
model clipping through the near plane — and the spectrum and filter overlays are enabled in its
place. Editor gizmos mark both anchor positions so the transforms can be placed visually.

### Guard AI

Guards run a two-level state machine — an outer `Guardstate` (`walking` / `attack`) and an
inner `Attackstate` with four phases:

```csharp
public enum Attackstate { startAttack, lookAtPlayer, checkPlayer, walkToPlayer }
```

The important phase is `checkPlayer`. Guards do not aggro on sight — they stop, turn to face
the player, then raycast forward to confirm what they're actually looking at:

- Ray hits the **player** → open fire, hold at engagement distance
- Ray hits **something else** (cover, geometry, another NPC) → the guard reduces its
  `stoppingDistance` to 60% of its current value and advances, then re-checks
- Ray hits **nothing** → advance

That third branch is what makes them feel deliberate rather than scripted: a guard whose line
of sight is blocked repeatedly closes the gap in shrinking increments until it can actually see
you, which reads as the guard working the angle instead of teleporting to omniscience. Patrol
uses `NavMeshAgent` pathing to `Random.onUnitSphere` offsets within a configurable radius, with
animator state driven off agent velocity.

### Supporting systems

Drivable vehicle with its own manager for travelling between contract sites, separate bandit AI,
a quest/progression manager gating three contract chains, waypoint markers, weapon and health
systems, and a rain manager with ambient audio.

---

## What I'd Do Differently

- **Dead branch in the guard raycast.** Inside `if (hasHit)` there's a `if (hit.collider == null)`
  check — unreachable, since a successful raycast hit always has a collider. It hid a case I
  thought I was handling.
- **`GetComponent` in `Update()`.** `uiMarksScript` calls `player.GetComponent<PlayerWeaponScript>()`
  twice per frame, per NPC. With a crowd on screen that's hundreds of lookups a frame for a
  reference that never changes — it should be cached in `Start()`.
- **Cross-object reach-through.** Several systems locate each other via
  `GameObject.FindGameObjectWithTag` at startup. Serialised references or a service locator
  would make the dependencies visible instead of implicit.
- **`terminalScript` hardcodes three contracts** as nine parallel string fields
  (`targetName1..3`, `targetDestination1..3`). A `Contract` ScriptableObject with a list would
  have made adding a fourth contract a data change rather than a code change — which is exactly
  the lesson I applied on later projects.

---

## Credits

Original game design by [Sokpop Collective](https://sokpop.co). This repository is an
independent student re-implementation built for coursework — all Unity implementation, C#
systems, and AI code here are mine; the design they realise is not.

---

## Note on repository history

This repository originally carried ~404 MB of committed WebGL build artifacts — seven versions,
each stored both zipped and unpacked, accounting for 82% of its size. History has been rewritten
to remove them so the project can be cloned in reasonable time. The Unity project itself is
unchanged.
