using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using AssetsTools.NET;
using AssetsTools.NET.Extra;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Mono.Cecil;

namespace DSPMac_patcher
{
    public class DSPMacPatcher
    {
        /**
	    * Note: This preloader patcher does not actually patch any assemblies. It works on the GlobalGameManagers asset using AssetsTools.NET.
	    */
		public static IEnumerable<string> TargetDLLs { get; } = Array.Empty<string>();

        private static readonly ManualLogSource Log = Logger.CreateLogSource("DSPMac");

        public const string GUID = "com.andyo.dspmac";
        private const string DISPLAY_NAME = "DSP Mac Compatibility";

        public static void Initialize()
        {
			Log.LogInfo("Checking GlobalGameManagers");
			ConfigFile config = new ConfigFile(Utility.CombinePaths(Paths.ConfigPath, GUID + ".cfg"), false);
			ConfigEntry<bool> enabled = config.Bind(DISPLAY_NAME, DISPLAY_NAME, true, "Disable GPU Skinning (uses geometry shaders, which are not supported on macOS)");
			bool modEnabled = enabled.Value;
			Log.LogInfo("GlobalGameManagers patch should be " + (modEnabled ? "enabled" : "disabled"));

			string ggmFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DSPGAME_Data", "globalgamemanagers");
			AssetsManager am = new AssetsManager();
			Stream classdata = Assembly.GetExecutingAssembly().GetManifestResourceStream("DSPMac_patcher.classdata.tpk");
			am.LoadClassPackage(classdata);
			AssetsFileInstance ggm = am.LoadAssetsFile(ggmFilePath, false);
			AssetsFile ggmFile = ggm.file;
			AssetsFileTable ggmTable = ggm.table;
			am.LoadClassDatabaseFromPackage(ggmFile.typeTree.unityVersion);

			AssetFileInfoEx playerSettings = ggmTable.GetAssetInfo(1);
			AssetTypeValueField playerSettingsBase = am.GetTypeInstance(ggmFile, playerSettings).GetBaseField();
			AssetTypeValue gpuSkinningValue = playerSettingsBase.Get("gpuSkinning").GetValue();
			bool gpuSkinning = gpuSkinningValue.AsBool();

			if (!gpuSkinning && modEnabled)
			{
				Log.LogInfo("GlobalGameManagers is already patched");
				am.UnloadAllAssetsFiles();
				return;
			}

            string backupPath = ggmFilePath + ".bak";

            Log.LogInfo("Patching GlobalGameManagers (gpuSkinning=" + !modEnabled + ")");
			gpuSkinningValue.Set(!modEnabled);
			byte[] playerSettingsBaseBytes = playerSettingsBase.WriteToByteArray();

			AssetsReplacerFromMemory replacer = new AssetsReplacerFromMemory(0, playerSettings.index, (int)playerSettings.curFileType, 0xffff, playerSettingsBaseBytes);
			string ggmTempFilePath = ggmFilePath + ".tmp";
			using (AssetsFileWriter writer = new AssetsFileWriter(File.OpenWrite(ggmTempFilePath)))
			{
				ggmFile.Write(writer, 0, new List<AssetsReplacer> { replacer }, 0);
			}
			am.UnloadAllAssetsFiles();
            File.Replace(sourceFileName: ggmTempFilePath, destinationFileName: ggmFilePath, destinationBackupFileName: backupPath);
            Log.LogInfo("Finished patching GlobalGameManagers. Backup created.");
		}

#pragma warning disable IDE0060 // Remove unused parameter
		public static void Patch(AssemblyDefinition assembly)
#pragma warning restore IDE0060 // Remove unused parameter
        {

        }

    }
}
