using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
using SkToolbox;
//TODO: fix ctrl reset
namespace HotKey
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    [BepInDependency("com.Skrip.SkToolbox", BepInDependency.DependencyFlags.SoftDependency)] // Set the dependency ] // Set the dependency 
    public class BepInExPlugin : BaseUnityPlugin
    {
        Harmony harmony = new Harmony(HARMONY_ID);

        public const string // Declare plugin information
            PLUGIN_AUTH = "telnet",
            PLUGIN_NAME = "hotkeyhelper",
            PLUGIN_GUID = PLUGIN_AUTH + "." + PLUGIN_NAME,
            PLUGIN_VERS = "0.0.1.1",
            PLUGIN_RECV = "1.1.0.0",
            HARMONY_ID = PLUGIN_GUID + "." + "harmony";

        public static ConfigEntry<bool> isDebug;
        public static string Dbglmessage = "";
        //public static int Dbglrepeatcount = 0;

        public static void Dbgl(string str = "", bool pref = true, bool repeat = true)//, int repeatmessagecount = -1)
        {
            if (!repeat)
            {
                if (Dbglmessage.Equals(str))//|| Dbglrepeatcount < 100) 
                {
                    //Dbglrepeatcount++;
                    return;
                }
                Dbglmessage = str;
                //Dbglrepeatcount = 0;
            }
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }

        const string category1 = "Hotkeys";
        const string DEFAULTCOMMANDDESC = "INSERT COMMAND DESCRIPTION HERE";

        [Command("resetcup", DEFAULTCOMMANDDESC, category1)]
        public static void resetcupcmd(string arg)
        {
            resetbutton.Invoke("Press", 0);
        }

        static bool sceneready = false;

        [Command("OrderSubmit", DEFAULTCOMMANDDESC, category1)] // Declare the 'EnableTools' command
        public static void EnableTools()
        {
            ordermanager.OrderFinished();
        }

        [Command("Hotkeyoverridetoggle", "toggles hotkeys on and off.","Hotkeys")]
        public static void hkot()
        {
            manualhotkeyoverride = !manualhotkeyoverride;
        }

        //public static List<ConfigEntry<string>> confighotkeyList = new List<ConfigEntry<string>>();

        public static ConfigEntry<string> confighotkeyMilk;
        public static ConfigEntry<string> confighotkeyCream;
        public static ConfigEntry<string> confighotkeyWhippedCream;
        public static ConfigEntry<string> confighotkeyIce;
        public static ConfigEntry<string> confighotkeyCoffee;
        public static ConfigEntry<string> confighotkeyTea;
        public static ConfigEntry<string> confighotkeyEspresso;
        public static ConfigEntry<string> confighotkeySugar;
        public static ConfigEntry<string> confighotkeyChocolate;
        public static ConfigEntry<string> confighotkeyBoba;
        public static ConfigEntry<string> confighotkeySprinklers;
        public static ConfigEntry<string> confighotkeyCaramelsauce;
        public static ConfigEntry<string> confighotkeyChocolateSauce;
        public static ConfigEntry<string> confighotkeyreset;
        public static ConfigEntry<string> confighotkeysubmit;
        public static ConfigEntry<string> Confighotkeymilking;


        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            // Set up configs for fillings
            //confighotkeyList.Add(Config.Bind("Hotkeys", $"hotkey{fill}", "z", $"Hotkey to add {fill} to cup"));

            // Set up configs for toppings


            // Set up configs
            confighotkeyMilk = Config.Bind("Hotkeys", "hotkeyMilk", "z", "Hotkey to add milk to cup");
            confighotkeyCream = Config.Bind("Hotkeys", "hotkeyCream", "x", "Hotkey to add Cream to cup");
            confighotkeyWhippedCream = Config.Bind("Hotkeys", "hotkeyWhippedCream", "c", "Hotkey to add WhippedCream to cup");
            confighotkeyIce = Config.Bind("Hotkeys", "hotkeyIce", "v", "Hotkey to add Ice to cup");
            confighotkeyCoffee = Config.Bind("Hotkeys", "hotkeyCoffee", "a", "Hotkey to add Coffee to cup");
            confighotkeyTea = Config.Bind("Hotkeys", "hotkeyTea", "s", "Hotkey to add Tea to cup");
            confighotkeyEspresso = Config.Bind("Hotkeys", "hotkeyEspresso", "d", "Hotkey to add Espresso to cup");
            confighotkeySugar = Config.Bind("Hotkeys", "hotkeySugar", "f", "Hotkey to add Sugar to cup");
            confighotkeyChocolate = Config.Bind("Hotkeys", "hotkeyChocolate", "g", "Hotkey to add Chocolate to cup");
            confighotkeyBoba = Config.Bind("Hotkeys", "hotkeyBoba", "q", "Hotkey to add Boba to cup");
            confighotkeySprinklers = Config.Bind("Hotkeys", "hotkeySprinklers", "w", "Hotkey to add Sprinklers to cup");
            confighotkeyCaramelsauce = Config.Bind("Hotkeys", "hotkeyCaramelsauce", "e", "Hotkey to add Caramelsauce to cup");
            confighotkeyChocolateSauce = Config.Bind("Hotkeys", "hotkeyChocolateSauce", "r", "Hotkey to add ChocolateSauce to cup");


            //https://answers.unity.com/questions/762073/c-list-of-string-name-for-inputgetkeystring-name.html
            confighotkeyreset = Config.Bind("Hotkeys", "hotkeyreset", "left ctrl", "Hotkey to reset cup");
            confighotkeysubmit = Config.Bind("Hotkeys", "hotkeysubmit", "space", "Hotkey to submit order");
            Confighotkeymilking = Config.Bind("Hotkeys", "hotkeymilking", "left shift", "Hotkey to milk Barista");

            Logger.LogInfo("Got Hotkey binds.");

            fcid.Add(Fillings.Milk, confighotkeyMilk.Value);
            fcid.Add(Fillings.Cream, confighotkeyCream.Value);
            fcid.Add(Fillings.Coffee, confighotkeyCoffee.Value);
            fcid.Add(Fillings.Tea, confighotkeyTea.Value);
            fcid.Add(Fillings.Espresso, confighotkeyEspresso.Value);
            fcid.Add(Fillings.Sugar, confighotkeySugar.Value);
            fcid.Add(Fillings.Chocolate, confighotkeyChocolate.Value);

            foreach (KeyValuePair<Fillings, string> i in fcid)
            {
                dicf.Add(i.Value, i.Key);
                Logger.LogInfo($"{i.Key}:{i.Value}");
            }

            tcid.Add(Toppings.WhipedCream, confighotkeyWhippedCream.Value);
            tcid.Add(Toppings.Ice, confighotkeyIce.Value);
            tcid.Add(Toppings.Boba, confighotkeyBoba.Value);
            tcid.Add(Toppings.Sprinkles, confighotkeySprinklers.Value);
            tcid.Add(Toppings.CaramelSauce, confighotkeyCaramelsauce.Value);
            tcid.Add(Toppings.ChocolateSauce, confighotkeyChocolateSauce.Value);

            //Logger.LogInfo("In tcid:");
            foreach (KeyValuePair<Toppings, string> i in tcid)
            {
                dict.Add(i.Value, i.Key);
                //Logger.LogInfo($"{i.Key}:{i.Value}");
            }
        }

        private Dictionary<string, Fillings> dicf = new Dictionary<string, Fillings>();
        private Dictionary<string, Toppings> dict = new Dictionary<string, Toppings>();

        public static Dictionary<Fillings, string> fcid = new Dictionary<Fillings, string>();
        public static Dictionary<Toppings, string> tcid = new Dictionary<Toppings, string>();


        static public Dictionary<char, FillingTool> keytofillingtool = new Dictionary<char, FillingTool>();

        static private OrderManager ordermanager;
        static private Button resetbutton;
        private BaristaMilkingHelper baristamilkinghelper;

        private void OnDestroy()
        {
            Dbgl("Destroying plugin");
        }

        //Inefficent Function from 
        //https://forum.unity.com/threads/is-there-no-way-to-get-reference-of-inactive-gameobject.472851/#post-4900355
        private Button FindInActiveObjectByName(string name)
        {
            Button[] objs = Resources.FindObjectsOfTypeAll<Button>() as Button[];
            foreach (Button i in objs)
            {
                if (i.name == name)
                {
                    return i;
                }
            }
            return null;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            string scenename = arg0.name;
            if (scenename.Contains("MainMenu"))
            {
                Dbgl("MainMenu");
                sceneready = false;
                hotkeyfillingtooldic.Clear();
                //fillingsfillingtooldictionary.Clear();
                //toppingsfillingtooldictionary.Clear();
                Statics.TextActualTime = "Your Time";
                Statics.TextMoneyEarned = "Money Earned";
                return;
            }
            bool ishard = scenename.Contains("Hard");
            string hardstring = (ishard ? " Hard" : "");

            ordermanager = GameObject.FindObjectOfType<OrderManager>();// Dbgl("OrderManager");
            //dialogmanager = GameObject.Find("Canvas Dialog" + hardstring).GetComponent<BaristaTalkManager>();
            baristamilkinghelper = GameObject.Find("Sphere").GetComponent<BaristaMilkingHelper>();

            resetbutton = FindInActiveObjectByName("Button Reset Cup");
            Statics.TextActualTime = "Your cheat? Time";
            Statics.TextMoneyEarned = "Money Cheated?";
            sceneready = true;

        }

        static public bool hotkeyoverride = false;
        static public bool manualhotkeyoverride = false;

        private bool boolstack = false;
        private void Update()
        {
            if (!sceneready || manualhotkeyoverride)
                return;

            // I'm still not convinced that this is the best way to do this...
            foreach (KeyValuePair<string, FillingTool> i in hotkeyfillingtooldic)
            {
                if (Input.GetKeyDown(i.Key))
                {
                    i.Value.Invoke("OnMouseDown", 0);
                    continue;
                }
                if (Input.GetKeyUp(i.Key))
                {
                    i.Value.Invoke("OnMouseUp", 0);
                    continue;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && !boolstack)
            {
                ordermanager.OrderFinished();
                boolstack = true;
                return;
            }
            if (Input.GetKeyDown(Confighotkeymilking.Value) && !boolstack)
            {
                baristamilkinghelper.Invoke("OnMouseDown", 0);
                boolstack = true;
                return;
            }

            if (Input.GetKeyDown(confighotkeyreset.Value) && !boolstack)
            {
                resetbutton.Invoke("Press", 0);
                boolstack = true;
                return;
            }
            boolstack = false;
            if (!Input.GetKeyUp(Confighotkeymilking.Value))
                return;
            baristamilkinghelper.Invoke("OnMouseUp", 0);
        }

        private static Dictionary<string, FillingTool> hotkeyfillingtooldic = new Dictionary<string, FillingTool>();

        [HarmonyPatch(typeof(FillingTool), "Awake")]
        static class FillingTool_Patch
        {
            static void Postfix(FillingTool __instance)
            {
                if (__instance.isTopping)
                {
                    Dbgl(__instance.CupToppings.ToString());
                    hotkeyfillingtooldic.Add(tcid[__instance.CupToppings], __instance);
                }
                else
                {
                    Dbgl(__instance.MashineFilling.ToString());
                    hotkeyfillingtooldic.Add(fcid[__instance.MashineFilling], __instance);
                }
            }
        }

    }
}