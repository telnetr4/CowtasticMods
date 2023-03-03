using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace TASTry
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        Harmony harmony = new Harmony("my.harmony.id");

        public static ConfigEntry<bool> isDebug;

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }

        internal const string PLUGIN_GUID = "telnet.TASTry";
        internal const string PLUGIN_NAME = "TAS Try";
        internal const string PLUGIN_VERS = "0.0.0.1";

        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                Dbgl($"Keydown U");
            }
        }

        [HarmonyPatch(typeof(GameMode_Arcade), "Update")]
        static class GameMode_Arcade_Patch
        {
            private static int framecount = 0;
            private static int pourslower = 5;

            static void Postfix(BaristaController __instance)
            {
                if (Input.GetKey(KeyCode.K))
                {
                    Dbgl($"Keydown K");
                    if (framecount > pourslower)
                    {
                        __instance.GetComponent<BaseGameMode>().FillCupCoffee();
                        framecount = 0;
                    }
                    else
                    {
                        framecount += 1;
                    }
                }
                if (Input.GetKeyUp(KeyCode.K))
                {
                    framecount = 0;
                }
            }
        }

    }
}
