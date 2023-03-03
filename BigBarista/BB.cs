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


namespace BigBarista
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]

    public class BepInExPlugin : BaseUnityPlugin
    {
        internal const string PLUGIN_GUID = "telnet.bigbarista";
        internal const string PLUGIN_NAME = "bigbarista";
        internal const string PLUGIN_VERS = "0.0.0.2";

        internal static float ScaleFact = 1.5f;
        public static ConfigEntry<float> ConfigScaleFact;
      //public static ConfigEntry<bool> configdontscalez;
        public static ConfigEntry<float> configXoffset;
        public static ConfigEntry<float> configYoffset;
        public static ConfigEntry<float> configXoffscalefactor;
        public static ConfigEntry<float> configYoffscalefactor;

        internal static Vector3 BarScale { get; set; }
        internal static Vector3 BarPosit { get; set; }
        Harmony harmony = new Harmony("my.harmony.id");

        public static ConfigEntry<bool> isDebug;

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }
        public static Mesh customMesh { get; set; }

        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", false, "Enable debug");

            // Set up configs
            ConfigScaleFact = Config.Bind("Barista Scale", "ScaleFact", 1.0f, "The scale factor that is applied to the barista. 2 will double her size while .5 will halve her size.");
            //configdontscalez = Config.Bind("Offsets", "/*Dontscalez*/", true, "Determines whether to scale z.");
            configXoffscalefactor = Config.Bind("Position Offsets", "Xoffscalefactor", 4.53f  , "A scale factor that is applied to Position offset. Don't need to change this unless the horizontal position looks wrong for ALL scales.");
            configYoffscalefactor = Config.Bind("Position Offsets", "Yoffscalefactor", 3.0142f, "A scale factor that is applied to Position offset. Don't need to change this unless the vertical position looks wrong for ALL scales.");
            configXoffset = Config.Bind("Position Offsets", "Xoffset", 0f, "A global horizontal position offset variable.");
            configYoffset = Config.Bind("Position Offsets", "Yoffset", 0f, "A global vertical position offset variable.");


            // Plugin startup logic
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());

            //SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }

        private void OnDestroy()
        {
            Dbgl("Destroying plugin");
            harmony.UnpatchAll();
        }

        //[HarmonyPatch(typeof(Scene), "Awake")]
        //static class Scene_Patch
        //{
        //    static void Postfix(Scene __instance)
        //    {
        //        Dbgl($"{__instance.name} Awake");

        //    }
        //}


        [HarmonyPatch(typeof(BaristaController), "Awake")]
        static class BaristaController_Patch
        {
            static void Postfix(BaristaController __instance)
            {
                SkinnedMeshRenderer BaristaSkinMesh = __instance.GetComponentInChildren<SkinnedMeshRenderer>();
                Vector3 originalext = BaristaSkinMesh.bounds.extents;

                Dbgl($"{__instance.name} Awake");
                BarScale = new Vector3(ConfigScaleFact.Value, ConfigScaleFact.Value, 1.0f);
                

                Dbgl(BaristaSkinMesh.name);
                Transform insttransf = __instance.GetComponent<Transform>();

                BarPosit = new Vector3((BarScale.x - 1) * (configXoffscalefactor.Value) +configXoffset.Value, (BarScale.y - 1) * (configYoffscalefactor.Value) + configYoffset.Value, 0);



                insttransf.localScale = BepInExPlugin.BarScale;
                insttransf.localPosition = BarPosit;
            }
        }
    }
}
