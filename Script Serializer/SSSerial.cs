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

namespace Static_String_Serializer
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        //Harmony harmony = new Harmony("my.harmony.id");

        public static ConfigEntry<bool> isDebug;

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }

        internal const string PLUGIN_GUID = "telnet.Static_String_Serializer";
        internal const string PLUGIN_NAME = "Static String Serializer";
        internal const string PLUGIN_VERS = "0.0.0.1";

        internal void staticserializer(string stringname)
        {
            typeof(Statics).GetProperty("ml");
        }

        internal static BaseGameMode Test;
        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());

            Statics.Tea = "FUCK";
            Statics.CustomerDialogDrinkModiferTea = "FUCK";
            Statics.Coffee = "ASS";
            Statics.CustomerDialogDrinkTypeCoffee = "ASS";
            Statics.CustomerDialogDrinkModiferCoffee = "ASS ";
            Statics.BaristaTalk_PatHead = new string[] {"FUCK YOU"};

        }

        


    }
}
