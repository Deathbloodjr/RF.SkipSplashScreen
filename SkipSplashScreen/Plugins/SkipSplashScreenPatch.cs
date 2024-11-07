using HarmonyLib;
using Platform.Steam;
using Scripts.OutGame.Boot;
using Scripts.OutGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace SkipSplashScreen.Plugins
{
    internal class SkipSplashScreenPatch
    {
        static bool SkipFade = false;

        const float newDuration = 0f;

        [HarmonyPatch(typeof(BootSceneUiController))]
        [HarmonyPatch(nameof(BootSceneUiController.Method_Private_UniTask_Single_Boolean_0))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static bool BootSceneUiController_StartAsync_PlayAsync_Prefix(BootSceneUiController __instance, ref float duration)
        {
            Logger.Log("BootSceneUiController_StartAsync_PlayAsync_Prefix");

            SkipFade = true;
            duration = newDuration;

            return true;
        }

        [HarmonyPatch(typeof(BootSceneUiController))]
        [HarmonyPatch(nameof(BootSceneUiController.IsShowESRB))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void BootSceneUiController_IsShowESRB_Postfix(BootSceneUiController __instance, ref bool __result)
        {
            Logger.Log("BootSceneUiController_IsShowESRB_Postfix");

            __result = false;
        }

        [HarmonyPatch(typeof(BootImage))]
        [HarmonyPatch(nameof(BootImage.SetPEGI))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static bool BootImage_SetPEGI_Prefix(BootImage __instance)
        {
            Logger.Log("BootImage_SetPEGI_Prefix");

            __instance.Type.Value = BootImage.Types.AutoSaveWarning;

            return false;
        }

        [HarmonyPatch(typeof(BootImage))]
        [HarmonyPatch(nameof(BootImage.PlayAsync))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static bool BootImage_PlayAsync_Prefix(BootImage __instance, ref float duration)
        {
            Logger.Log("BootImage_PlayAsync_Prefix");
            SkipFade = true;
            duration = newDuration;

            return true;
        }



        [HarmonyPatch(typeof(FadeCover))]
        [HarmonyPatch(nameof(FadeCover.FadeInAsync))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static bool FadeCover_FadeInAsync_Prefix(BootSceneUiController __instance, ref float duration)
        {
            //Logger.Log("");
            //Logger.Log("FadeCover_FadeInAsync_Prefix");

            if (SkipFade)
            {
                duration = newDuration;
            }

            return true;
        }


        [HarmonyPatch(typeof(FadeCover))]
        [HarmonyPatch(nameof(FadeCover.FadeOutAsync))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPrefix]
        public static bool FadeCover_FadeOutAsync_Prefix(BootSceneUiController __instance, ref float duration)
        {
            //Logger.Log("");
            //Logger.Log("FadeCover_FadeOutAsync_Prefix");

            if (SkipFade)
            {
                duration = newDuration;
            }

            SkipFade = false;

            return true;
        }
    }
}
