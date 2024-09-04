﻿using HarmonyLib;
using Platform.Steam;
using Scripts.OutGame.Boot;
using Scripts.OutGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
        public static bool BootSceneUiController_StartAsync_Prefix(BootSceneUiController __instance, ref float duration)
        {
            Plugin.Log.LogInfo("");
            Plugin.Log.LogInfo("BootSceneUiController_StartAsync_Prefix");

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
            Plugin.Log.LogInfo("");
            Plugin.Log.LogInfo("FadeCover_FadeInAsync_Prefix");

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
            Plugin.Log.LogInfo("");
            Plugin.Log.LogInfo("FadeCover_FadeOutAsync_Prefix");

            if (SkipFade)
            {
                duration = newDuration;
            }

            SkipFade = false;

            return true;
        }

        //[HarmonyPatch(typeof(UiBootImage))]
        //[HarmonyPatch(nameof(UiBootImage.SetType))]
        //[HarmonyPatch(MethodType.Normal)]
        //[HarmonyPrefix]
        //public static bool UiBootImage_Bind_Prefix(UiBootImage __instance)
        //{
        //    Plugin.Log.LogInfo("");
        //    Plugin.Log.LogInfo("UiBootImage_Bind_Prefix");

        //    __instance.Skip();

        //    return true;
        //}

        //[HarmonyPatch(typeof(BootSceneUiController))]
        //[HarmonyPatch(nameof(BootSceneUiController.Bind))]
        //[HarmonyPatch(MethodType.Normal)]
        //[HarmonyPostfix]
        //public static void BootSceneUiController_Bind_Postfix(BootSceneUiController __instance)
        //{

        //    Plugin.Log.LogInfo("");
        //    Plugin.Log.LogInfo("BootSceneUiController_Bind_Postfix");

        //    __instance.Teardown();

        //}
    }
}