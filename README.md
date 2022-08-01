# DSPMac
Mod for Dyson Sphere Program for better compatibility with macOS

**This mod permanently (persists after exit) modifies DSP's globalgamemanagers file, which is pretty far outside the bounds of a normal mod. A backup of the original (ends in .bak) is created in case you need to manually restore it.**

## Why?
DSP runs very well in [Crossover Mac](https://www.codeweavers.com/crossover#mac) 21.0+ with the notable exception of the player mech not being visible. This is a result of Unity using geometry shaders for gpu skinning on this model, and Crossover/MoltenVK does not yet support geometry shaders. Luckily, gpu skinning is a toggle in PlayerSettings. This mod simply turns gpu skinning off.

## Tips
- Getting BepInEx to run within Crossover/Wine requires you to do a couple configuration steps. Follow [this guide](https://docs.bepinex.dev/articles/advanced/proton_wine.html) to make sure it's actually loaded when DSP launches.

## Thanks
This mod borrows heavily from [Faeryn's OutwardVisibleInBackground](https://github.com/Faeryn/OutwardVisibleInBackground) mod.

## Changelog
- v1.0.0
	- Initial release
