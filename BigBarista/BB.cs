using BepInEx;
using BepInEx.Configuration;
using BepInEx.Bootstrap;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using SkToolbox;
using Debug = UnityEngine.Debug;

namespace BigBarista
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    [BepInDependency(modguidreq, BepInDependency.DependencyFlags.SoftDependency)] // Set the dependency 

    public class BepInExPlugin : BaseUnityPlugin
    {
        const string modguidreq = "com.Skrip.SkToolbox";
        static internal bool hassktoolbox = false;

        public const string // Declare plugin information
            PLUGIN_AUTH = "telnet",
            PLUGIN_NAME = "bigbarista",
            PLUGIN_GUID = PLUGIN_AUTH + "." + PLUGIN_NAME,
            PLUGIN_VERS = "0.0.1.0",
            PLUGIN_RECV = "1.0.0.24";

        internal static float ScaleFact = 1.5f;
        public static ConfigEntry<float> ConfigScaleFact;
        //public static ConfigEntry<bool> configdontscalez;
        public static ConfigEntry<float> configXoffset;
        public static ConfigEntry<float> configYoffset;
        public static ConfigEntry<float> configXoffscalefactor;
        public static ConfigEntry<float> configYoffscalefactor;

        internal static Vector3 BarScale { get; set; }
        internal static Vector3 BarPosit { get; set; }
        Harmony _harmony = new Harmony(PLUGIN_GUID + ".harmony.id");

        public static ConfigEntry<bool> isDebug;

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }
        public static Mesh customMesh { get; set; }

        private bool checkokversion(string version)
        {
            Version game = new Version(Application.version);
            Version recc = new Version(PLUGIN_RECV);
            return (game == recc);
        }

        private void Awake()
        {
            Logger.LogInfo("check metadata:");
            // from: https://github.com/BepInEx/BepInEx/discussions/320
            // Only already loaded mods show up in PluginInfos, thats why SoftDependency is used above to ensure that.
            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID.Equals(modguidreq))
                {
                    // found it
                    Logger.LogInfo($"Found {modguidreq}");
                    hassktoolbox = true;
                    break;
                }
            }

            isDebug = Config.Bind<bool>("General", "IsDebug", false, "Enable debug");

            // Set up configs
            ConfigScaleFact = Config.Bind("Barista Scale", "ScaleFact", 1.0f, "The scale factor that is applied to the barista. 2 will double her size while .5 will halve her size.");
            //configdontscalez = Config.Bind("Offsets", "/*Dontscalez*/", true, "Determines whether to scale z.");
            //configXoffscalefactor = Config.Bind("Position Offsets", "Xoffscalefactor", 3.6603f, "A scale factor that is applied to Position offset. Don't need to change this unless the horizontal position looks wrong for ALL scales.");
            //configYoffscalefactor = Config.Bind("Position Offsets", "Yoffscalefactor", 2.8007f, "A scale factor that is applied to Position offset. Don't need to change this unless the vertical position looks wrong for ALL scales.");
            configXoffset = Config.Bind("Position Offsets", "Xoffset", 0f, "A global horizontal position offset variable.");
            configYoffset = Config.Bind("Position Offsets", "Yoffset", 0f, "A global vertical position offset variable.");


            // Plugin startup logic
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());

            //SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private void OnDestroy()
        {
            Dbgl("Destroying plugin");
            _harmony.UnpatchAll();
        }

        public class BaristaScaleController
        {
            public BaristaScaleController(BaristaController baristaController)
            {
                controlledbarista = baristaController;
                //name = controlledbarista.name;
                controlledskinmesr = controlledbarista.GetComponentInChildren<SkinnedMeshRenderer>();
                controlledtransform = controlledbarista.GetComponent<Transform>();
                controlledrenderer = controlledtransform.GetComponent<Renderer>();
                posoffset.x = configXoffset.Value;
                posoffset.y = configYoffset.Value;
                posoffset.z = 0f;
                //barscale = controlledbarista.transform.localScale;
                //barpos = controlledtransform.localPosition;
                //pospropfactor_setting = new Vector3(configXoffscalefactor.Value, configYoffscalefactor.Value, 0);
            }

            private BaristaController controlledbarista;
            //private string name;
            private SkinnedMeshRenderer controlledskinmesr;
            private Transform controlledtransform;
            private Renderer controlledrenderer;
            private Vector3 originalcenter = new Vector3(3.6603f, 2.8007f, 2.0593f);
            //private Vector3 barpos;
            //private Vector3 barscale;
            private Vector3 posoffset = Vector3.zero;

            public void scalebarista(Vector3 scale)
            {
                controlledtransform.localScale = scale;
            }

            public void posbarista(Vector3 pos)
            {
                controlledtransform.localPosition = pos;
            }

            private Vector3 calcposproportional (Vector3 pos,bool includez=false)
            {
                //(Newscale-1)center
                Vector3 vect = Vector3.Scale((controlledtransform.localScale - Vector3.one), originalcenter) + posoffset;
                if (!includez)
                    vect.z = posoffset.z;
                return vect;
                //return new Vector3((controlledtransform.localScale.x - 1) * (pospropfactor_setting.x) + pos.x,
                //                   (controlledtransform.localScale.y - 1) * (pospropfactor_setting.y) + pos.y,
                //                   (controlledtransform.localScale.z - 1) * (pospropfactor_setting.z) + pos.z);
            }

            public void posproportionalbarista(Vector3 pos)
            {
                posbarista(calcposproportional(pos));
            }

            public void currposporbarista()
            {
                posproportionalbarista(controlledtransform.position);
            }

            public void bighead(float scale)
            { 
                // TODO: finish this
            }

        }

        static BaristaScaleController BSC;

        static BaristaController baristafound;

        //[HarmonyPatch(typeof(Scene), "Awake")]
        //static class Scene_Patch
        //{
        //    static void Postfix(Scene __instance)
        //    {
        //        Dbgl($"{__instance.name} Awake");

        //    }
        //}



        //private void test( )
        //{ 
        //    SkinnedMeshRenderer BaristaSkinMesh = BC.GetComponentInChildren<SkinnedMeshRenderer>();
        //    Vector3 originalext = BaristaSkinMesh.bounds.extents;

        //    Dbgl($"{BC.name} Awake");
        //    BarScale = new Vector3(ConfigScaleFact.Value, ConfigScaleFact.Value, 1.0f);


        //    Dbgl(BaristaSkinMesh.name);
        //    Transform insttransf = BC.GetComponent<Transform>();

        //    BarPosit = new Vector3((BarScale.x - 1) * (configXoffscalefactor.Value) + configXoffset.Value, (BarScale.y - 1) * (configYoffscalefactor.Value) + configYoffset.Value, 0);

        //    insttransf.localPosition = BarPosit;
        //}


        [HarmonyPatch(typeof(BaristaController), "Awake")]
        static class BaristaController_Patch
        {
            static void Postfix(BaristaController __instance)
            {
                baristafound = __instance;
                BSC = new BaristaScaleController(__instance);
                //SkinnedMeshRenderer BaristaSkinMesh = __instance.GetComponentInChildren<SkinnedMeshRenderer>();
                //Vector3 originalext = BaristaSkinMesh.bounds.extents;

                //Dbgl($"{__instance.name} Awake");
                //BarScale = new Vector3(ConfigScaleFact.Value, ConfigScaleFact.Value, 1.0f);


                //Dbgl(BaristaSkinMesh.name);
                //Transform insttransf = __instance.GetComponent<Transform>();

                //BarPosit = new Vector3((BarScale.x - 1) * (configXoffscalefactor.Value) + configXoffset.Value, (BarScale.y - 1) * (configYoffscalefactor.Value) + configYoffset.Value, 0);



                //insttransf.localScale = BepInExPlugin.BarScale;
                //insttransf.localPosition = BarPosit;
            }
        }

        [Command("baristascale", "Set the scale of the barista. moveproportional = true should keep the barista in the middle.","Big Barista Scaler")]
        public static void scalebaristacmd(float x, float y, float z = 1.0f, bool moveproportional = true)
        {
            if (BSC is null)
            {
                Dbgl($"Error: Barisa not found");
                throw new NullReferenceException("Barista Not found (on MainMenu?)");
            }
            Dbgl("Begin cmd baristascale done");
            BSC.scalebarista(new Vector3(x, y, z));
            if (moveproportional)
                BSC.currposporbarista();
        }

        [Command("baristapos", "Set the position of the barista.", "Big Barista Scaler")]
        public static void posbaristacmd(float x, float y, float z=0.0f)
        {
            if (BSC is null)
            {
                Dbgl($"Error: Barisa not found");
                throw new NullReferenceException("Barista Not found (on MainMenu?)");
            }
            BSC.posbarista(new Vector3(x, y, z));
        }

        [Command("bighead", "Sets big head mode. Set to 0 to turn off.", "Big Barista Scaler")]
        public static void bigheadcmd(float z = 0.0f)
        {
            if (BSC is null)
            {
                Dbgl($"Error: Barisa not found");
                throw new NullReferenceException("Barista Not found (on MainMenu?)");
            }
            BSC.bighead(z);
        }

    }
}
