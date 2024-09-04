using BepInEx.Unity.IL2CPP.Utils;
using BepInEx.Unity.IL2CPP;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Configuration;
using SkipSplashScreen.Plugins;

namespace SkipSplashScreen
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, ModName, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        const string ModName = "SkipSplashScreen";

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

            if (ConfigEnabled.Value)
            {

                _harmony.PatchAll(typeof(SkipSplashScreenPatch));
                Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
            }
            else
            {
                Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is disabled.");
            }

            //if (ConfigExamplesEnabled.Value)
            //{
            //    _harmony.PatchAll(typeof(ExampleSingleHitBigNotesPatch));
            //    _harmony.PatchAll(typeof(ExampleSortByUraPatch));
            //    Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} Example Patches are loaded!");
            //}
        }
    }
}
