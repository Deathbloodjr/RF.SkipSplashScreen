using BepInEx.Unity.IL2CPP.Utils;
using BepInEx.Unity.IL2CPP;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Configuration;
using SkipSplashScreen.Plugins;
using UnityEngine;
using System.Collections;

namespace SkipSplashScreen
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, ModName, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        public const string ModName = "SkipSplashScreen";

        public static Plugin Instance;
        private Harmony _harmony;
        public new static ManualLogSource Log;


        public ConfigEntry<bool> ConfigEnabled;



        public override void Load()
        {
            Instance = this;

            Log = base.Log;

            SetupConfig();
            SetupHarmony();
        }

        private void SetupConfig()
        {
            var dataFolder = Path.Combine("BepInEx", "data", ModName);

            ConfigEnabled = Config.Bind("General",
                "Enabled",
                true,
                "Enables the mod.");
        }

        private void SetupHarmony()
        {
            // Patch methods
            _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

            LoadPlugin(ConfigEnabled.Value);
        }

        public static void LoadPlugin(bool enabled)
        {
            if (enabled)
            {
                bool result = true;
                // If any PatchFile fails, result will become false
                result &= Instance.PatchFile(typeof(SkipSplashScreenPatch));
                if (result)
                {
                    Logger.Log($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
                }
                else
                {
                    Logger.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} failed to load.", LogType.Error);
                    // Unload this instance of Harmony
                    // I hope this works the way I think it does
                    Instance._harmony.UnpatchSelf();
                }
            }
            else
            {
                Logger.Log($"Plugin {MyPluginInfo.PLUGIN_NAME} is disabled.");
            }
        }

        private bool PatchFile(Type type)
        {
            if (_harmony == null)
            {
                _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            }
            try
            {
                _harmony.PatchAll(type);
#if DEBUG
                Logger.Log("File patched: " + type.FullName);
#endif
                return true;
            }
            catch (Exception e)
            {
                Logger.Log("Failed to patch file: " + type.FullName);
                Logger.Log(e.Message);
                return false;
            }
        }

        public static MonoBehaviour GetMonoBehaviour() => TaikoSingletonMonoBehaviour<CommonObjects>.Instance;
        public void StartCoroutine(IEnumerator enumerator)
        {
            GetMonoBehaviour().StartCoroutine(enumerator);
        }
    }
}
