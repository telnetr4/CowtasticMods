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


// IF YOU ARE READING THIS: THIS IS PROBABLY A TERRIBLE WAY TO IMPLIMENT HOTKEYS.
// IF YOU CAN MAKE IT BETTER, FEEL FREE.
namespace HotKey
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        Harmony harmony = new Harmony("my.harmony.id");

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


        internal const string PLUGIN_GUID = "telnet.HotKeyHelper";
        internal const string PLUGIN_NAME = "HotKeyHelper";
        internal const string PLUGIN_VERS = "0.0.0.1";

        //FillingTool Milkfill;// = GameObject.Find("Milk").GetComponent<FillingTool>();


        //FillingTool Creamfill = GameObject.Find("Cream").GetComponent<FillingTool>();
        //FillingTool WhippedCreamfill = GameObject.Find("WhippedCream").GetComponent<FillingTool>();
        //FillingTool Icefill = GameObject.Find("Ice").GetComponent<FillingTool>();
        //FillingTool Coffeefill = GameObject.Find("Coffee").GetComponent<FillingTool>();
        //FillingTool Teafill = GameObject.Find("Tea").GetComponent<FillingTool>();
        //FillingTool Espressofill = GameObject.Find("Espresso").GetComponent<FillingTool>();
        //FillingTool Sugarfill = GameObject.Find("Sugar").GetComponent<FillingTool>();
        //FillingTool Chocolatefill = GameObject.Find("Chocolate").GetComponent<FillingTool>();
        //FillingTool Bobafill = GameObject.Find("Boba").GetComponent<FillingTool>();
        //FillingTool Sprinklersfill = GameObject.Find("Sprinklers").GetComponent<FillingTool>();
        //FillingTool Caramelsaucefill = GameObject.Find("Caramelsauce").GetComponent<FillingTool>();
        //FillingTool ChocolateSaucefill = GameObject.Find("ChocolateSauce").GetComponent<FillingTool>();

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



            // set keys to hotkeys in dictionary

            dicf.Add(confighotkeyMilk.Value[0], Fillings.Milk);
            dicf.Add(confighotkeyCream.Value[0], Fillings.Cream);
            dicf.Add(confighotkeyCoffee.Value[0], Fillings.Coffee);
            dicf.Add(confighotkeyTea.Value[0], Fillings.Tea);
            dicf.Add(confighotkeyEspresso.Value[0], Fillings.Espresso);
            dicf.Add(confighotkeySugar.Value[0], Fillings.Sugar);
            dicf.Add(confighotkeyChocolate.Value[0], Fillings.Chocolate);

            dict.Add(confighotkeyWhippedCream.Value[0], Toppings.WhipedCream);
            dict.Add(confighotkeyIce.Value[0], Toppings.Ice);
            dict.Add(confighotkeyBoba.Value[0], Toppings.Boba);
            dict.Add(confighotkeySprinklers.Value[0], Toppings.Sprinkles);
            dict.Add(confighotkeyCaramelsauce.Value[0], Toppings.CaramelSauce);
            dict.Add(confighotkeyChocolateSauce.Value[0], Toppings.ChocolateSauce);

            dicfilltool.Add("Milk", confighotkeyMilk.Value[0]);
            dicfilltool.Add("Cream",         confighotkeyCream.Value[0]);
            dicfilltool.Add("WhipedCream",         confighotkeyWhippedCream.Value[0]);
            dicfilltool.Add("Ice",         confighotkeyIce.Value[0]);
            dicfilltool.Add("Coffee",         confighotkeyCoffee.Value[0]);
            dicfilltool.Add("Tea",         confighotkeyTea.Value[0]);
            dicfilltool.Add("Espresso",         confighotkeyEspresso.Value[0]);
            dicfilltool.Add("Sugar",         confighotkeySugar.Value[0]);
            dicfilltool.Add("Chocolate",         confighotkeyChocolate.Value[0]);
            dicfilltool.Add("Boba",         confighotkeyBoba.Value[0]);
            dicfilltool.Add("Sprinklers",         confighotkeySprinklers.Value[0]);
            dicfilltool.Add("Caramelsauce",         confighotkeyCaramelsauce.Value[0]);
            dicfilltool.Add("ChocolateSauce",         confighotkeyChocolateSauce.Value[0]);
        }
        //private Dictionary<char, dynamic> dic = new Dictionary<char, dynamic>();

        private Dictionary<char, Fillings> dicf = new Dictionary<char, Fillings>();
        private Dictionary<char, Toppings> dict = new Dictionary<char, Toppings>();

        static private Dictionary<string, char> dicfilltool = new Dictionary<string, char>();

        private Dictionary<char, bool> dicunlocked = new Dictionary<char, bool>();

        private Dictionary<char, bool> isfilling = new Dictionary<char, bool>();

        private OrderManager ordermanager;
        static private Button resetbutton;
        private BaristaTalkManager dialogmanager;
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
                    return i;
            }
            return null;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (!arg0.name.Contains("MainMenu"))
            {
                ordermanager = GameObject.FindObjectOfType<OrderManager>(); Dbgl("OrderManager");
                dialogmanager = GameObject.Find("Canvas Dialog").GetComponent<BaristaTalkManager>();
                baristamilkinghelper = GameObject.Find("Sphere").GetComponent<BaristaMilkingHelper>();

                resetbutton = FindInActiveObjectByName("Button Reset Cup");

                sceneready = true;

            }
            else
                sceneready = false;
        }

        static bool sceneready = false;

        private bool boolstack = false;
        private void Update()
        {

            if (!sceneready)
                return;

            if (Input.GetKeyDown(KeyCode.Space) && !boolstack)
            {
                ordermanager.OrderFinished();
                boolstack = true;
                return;
            }
            if (Input.GetKeyDown(Confighotkeymilking.Value) && !boolstack)
            {
                baristamilkinghelper.Invoke("OnMouseDown",0);
                boolstack = true;
                return;
            }

            if (Input.GetKeyDown(confighotkeyreset.Value) && !boolstack)
            {
                //dialogmanager.DoBaristaEventCupReset();
                //ordermanager.DoResetCup();
                resetbutton.Invoke("Press",0);
                boolstack = true;
                return;
            }
            boolstack = false;
            if (!Input.GetKeyUp(Confighotkeymilking.Value))
                return;
            baristamilkinghelper.Invoke("OnMouseUp", 0);
        }

            static string pressed = "";

        [HarmonyPatch(typeof(FillingTool), "Update")]
        static class FillingTool_Patch
        {
            static void Prefix(FillingTool __instance, ref UpgradeManager ___upgradeManager, ref bool ___justUnlock, ref bool ___FillCup, ref bool ___CupFilledWithTopping, ref BaseGameMode ___gamemode, ref SoundEffectVariation ___SoundVariation)
            {
                bool fillit = false;

                //Dbgl("" + dicfilltool[__instance.name]+":"+__instance.name);
                string watchkey = dicfilltool[__instance.name].ToString();
                bool iskeypressed = Input.GetKey(watchkey);

                if (Input.GetKeyUp(watchkey))
                {
                    ___FillCup = false;
                    ___justUnlock = false;
                    ___SoundVariation.EndLoop();
                    //__instance.SoundVariation.EndLoop();
                    ___CupFilledWithTopping = false;
                    return;
                }

                if (iskeypressed)
                    pressed = watchkey;
                
                fillit = iskeypressed || ___FillCup;
                if (fillit)
                {
                    Dbgl($"{__instance.name}:{dicfilltool[__instance.name].ToString()}");
                }
                if (__instance.Unlocked)
                {
                    if (___upgradeManager != null && !___upgradeManager.UpgardePanelVisibility && fillit && !___CupFilledWithTopping)
                    {
                        if (__instance.isTopping)
                        {
                            ___gamemode.cupController.FillCup(__instance.CupToppings);
                            __instance.SoundVariation.PlayRandomOneShot(true);
                            ___CupFilledWithTopping = true;
                            Dbgl($"FillCuptopping");
                            return;
                        }
                        Dbgl($"FillCupfilling");
                        ___gamemode.cupController.FillCup(__instance.MashineFilling);
                        ___SoundVariation.PlayRandomLoop(true);
                    }
                }
                else if (iskeypressed && ___gamemode != null && ___gamemode.Money >= (float)__instance.MoneyToUnlock)
                {
                    ___gamemode.SubMoney((float)__instance.MoneyToUnlock);
                    __instance.SetUnlockedState(true);
                }
            }
        }
    }
}



// BEYOND HERE IS NOTHING BUT JUNK, FRUSTRATION AND BROKEN DREAMS

//private Dictionary<char, Delegate> dict = new Dictionary<char, Delegate>();
//{
//    {confighotkeyMilk          .Value, cupconfill(Fillings.Milk)},
//    {confighotkeyCream         .Value, cupconfill()},
//    {confighotkeyWhippedCream  .Value, cupconfill()},
//    {confighotkeyIce           .Value, cupconfill()},
//    {confighotkeyCoffee        .Value, cupconfill()},
//    {confighotkeyTea           .Value, cupconfill()},
//    {confighotkeyEspresso      .Value, cupconfill()},
//    {confighotkeySugar         .Value, cupconfill()},
//    {confighotkeyChocolate     .Value, cupconfill()},
//    {confighotkeyBoba          .Value, cupconfill()},
//    {confighotkeySprinklers    .Value, cupconfill()},
//    {confighotkeyCaramelsauce  .Value, cupconfill()},
//    {confighotkeyChocolateSauce.Value, cupconfill()},
//};


        //private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        //{
        //    if (!arg0.name.Contains("MainMenu"))
        //    {
        //        /*
        //        //get unlocks
        //        unlockedMilk = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Milk/Image Locked").activeSelf;
        //        unlockedCream = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Cream/Image Locked").activeSelf;
        //        unlockedWhippedCream = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/WhipedCream/Image Locked").activeSelf;
        //        unlockedIce = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Ice/Image Locked").activeSelf;
        //        unlockedCoffee = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Coffee/Image Locked").activeSelf;
        //        unlockedTea = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Tea/Image Locked").activeSelf;
        //        unlockedEspresso = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Espresso/Image Locked").activeSelf;
        //        unlockedSugar = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Sugar/Image Locked").activeSelf;
        //        unlockedChocolate = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Chocolate/Image Locked").activeSelf;
        //        unlockedBoba = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Boba/Image Locked").activeSelf;
        //        unlockedSprinklers = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Sprinklers/Image Locked").activeSelf;
        //        unlockedCaramelsauce = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/Caramelsauce/Image Locked").activeSelf;
        //        unlockedChocolateSauce = !GameObject.Find("Cafe Visuals/Canvas Shelf Right/ChocolateSauce/Image Locked").activeSelf;
        //        gotunlocks = true;

        //        //Get sounds
        //        soundMilk = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Milk").GetComponent<SoundEffectVariation>();
        //        soundCream = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Cream").GetComponent<SoundEffectVariation>();
        //        soundWhippedCream = GameObject.Find("Cafe Visuals/Canvas Shelf Right/WhipedCream").GetComponent<SoundEffectVariation>();
        //        soundIce = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Ice").GetComponent<SoundEffectVariation>();
        //        soundCoffee = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Coffee").GetComponent<SoundEffectVariation>();
        //        soundTea = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Tea").GetComponent<SoundEffectVariation>();
        //        soundEspresso = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Espresso").GetComponent<SoundEffectVariation>();
        //        soundSugar = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Sugar").GetComponent<SoundEffectVariation>();
        //        soundChocolate = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Chocolate").GetComponent<SoundEffectVariation>();
        //        soundBoba = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Boba").GetComponent<SoundEffectVariation>();
        //        soundSprinklers = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Sprinklers").GetComponent<SoundEffectVariation>();
        //        soundCaramelsauce = GameObject.Find("Cafe Visuals/Canvas Shelf Right/Caramelsauce").GetComponent<SoundEffectVariation>();
        //        soundChocolateSauce = GameObject.Find("Cafe Visuals/Canvas Shelf Right/ChocolateSauce").GetComponent<SoundEffectVariation>();
        //        */
        //        // Get GameManager

        //    }
        //}

//[HarmonyPatch(typeof(FillingTool), "Update")]
//static class FillingTool_Patch
//{
//    private static int framecount = 0;
//    private static int pourslower = -1;

//    static void prefix(FillingTool __instance)
//    {
//        __instance.
//    }

//    //static void Postfix(FillingTool __instance)
//    //{
//    //    string keypressed;
//    //    if(!Input.anyKey)
//    //    {
//    //        framecount = 0;
//    //        return; 
//    //    }
//    //    Dbgl($"{__instance.name}");
//    //    __instance.OnMouseDown();
//    //    switch(__instance.name)
//    //    {
//    //        case "Milk":
//    //            keypressed = "z";
//    //            break;
//    //        case "Cream":
//    //            keypressed = "x";
//    //            break;
//    //        case "WhippedCream":
//    //            keypressed = "c";
//    //            break;
//    //        case "Ice":
//    //            keypressed = "v";
//    //            break;
//    //        case "Coffee":
//    //            keypressed = "a";
//    //            break;
//    //        case "Tea":
//    //            keypressed = "s";
//    //            break;
//    //        case "Espresso":
//    //            keypressed = "d";
//    //            break;
//    //        case "Sugar":
//    //            keypressed = "f";
//    //            break;
//    //        case "Chocolate":
//    //            keypressed = "g";
//    //            break;
//    //        case "Boba":
//    //            keypressed = "q";
//    //            break;
//    //        case "Sprinklers":
//    //            keypressed = "w";
//    //            break;
//    //        case "Caramelsauce":
//    //            keypressed = "e";
//    //            break;
//    //        case "ChocolateSauce":
//    //            keypressed = "r";
//    //            break;
//    //        default:
//    //            keypressed = "";
//    //            break;
//    //    }


//    //    if (Input.GetKey(keypressed))
//    //    {
//    //        Dbgl($"Keydown {keypressed}");
//    //        //if (framecount > pourslower)
//    //        //{
//    //        //    __instance.
//    //        //    framecount = 0;
//    //        //}
//    //        //else
//    //        //{
//    //        //    framecount += 1;
//    //        //}
//    //    }
//    //    if (Input.GetKeyUp(KeyCode.K))
//    //    {
//    //        framecount = 0;
//    //    }
//    //}
//}
