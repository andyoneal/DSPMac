# DSPMac
Mod for Dyson Sphere Program for better compatibility with macOS

**WARNING: This mod permanently (persists after exit) modifies DSP's globalgamemanagers file, which is pretty far outside the bounds of a normal mod. A backup of the original (ends in .bak) is created in case you need to manually restore it.**

## Why?
DSP runs very well in [Crossover Mac](https://www.codeweavers.com/crossover#mac) 21.0+ with the notable exception of the player mecha not being visible. This is a result of Unity using geometry shaders and stream-output (aka transform feedback) for gpu skinning on this model, but Crossover/MoltenVK does not yet support geometry shaders, which is because Apple's Metal does not support them, which is because [they were a bad idea that won't die](https://www.jlekstrand.net/jason/blog/2018/10/transform-feedback-is-terrible-so-why/). Luckily, gpu skinning is a toggle in PlayerSettings. This mod simply turns gpu skinning off.

## Installation
### Required Step (Crossover/Wine only)
Wine/Crossover will prevent BepInEx (mod loader) from running correctly, regardless of the method you use to install this. To fix this, we need to add an override for winhttp.dll.
- If you are running directly on Wine (without Crossover), follow [this guide](https://docs.bepinex.dev/articles/advanced/proton_wine.html).
- If you are running with Crossover, select the bottle where DSP is installed, open the Wine Configuration control panel, and on the Libraries tab, add a new override for ```winhttp```. You can see a screenshot of this on step 2 of the Wine-only guide above.

### Easy Way
This mod is on [Thunderstore](https://dsp.thunderstore.io/package/Andy/DSP_Mac/), so the easiest way is probably using their mod manager or another like r2modman. You will still need to do the required step above.

### Manual Install
1. Open the directory for Dyson Sphere Program within the bottle. On macOS with Crossover, this buried deep in something like ```~/Library/Application Support/CrossOver/Bottles/BOTTLENAME/drive_c/Program Files (x86)/Steam/steamapps/common/Dyson Sphere Program/```
2. [BepInEx 5.4.17](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.17) is required. Download the x64 zip. Versions of BepInEx other than 5.4.17 may or may not work.
3. Extract the zip into the game directory you found in step 1.
4. Make sure you have done the required step above or BepInEx will not run.
5. Run Dyson Sphere Program and exit once you get to the title screen. This causes BepInEx to initialize and generate the folders we'll need to use.
6. Inside the Dyson Sphere Program directory, open the BepInEx folder, then the patchers folder. (It will be empty)
7. Download the latest DSPMod from [Releases](https://github.com/andyoneal/DSPMac/releases).
8. Extract the zip into the patchers folder. You should now see AssetTools.NET.dll and DSPMac_patcher.dll in the directory.
9. Run Dyson Sphere Program again and the player mecha should now be visible.

## Tips
- You should turn on DXVK and Performance Enhanced Graphics (CSMT) for your wine bottle. These are toggles in Crossover. Without DXVK, you will have a lot more graphics issues than just the invisible mecha.
- After testing with a new game and a very late game save on an M1 Max, setting the Logical Frame Thread Count (in Gameplay settings) to 6 (default is 10) gave me the best performance. I'm guessing an M1 Pro would also run best with 6, and an M1 would run best with 4.

## Thanks
This mod borrows heavily from [Faeryn's OutwardVisibleInBackground](https://github.com/Faeryn/OutwardVisibleInBackground) mod.

## Changelog
- v1.0.0
	- Initial release
