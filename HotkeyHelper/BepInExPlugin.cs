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

        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //SceneManager.sceneLoaded += SceneManager_sceneLoaded;

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

        private void OnDestroy()
        {
            Dbgl("Destroying plugin");
        }

        public static bool ispressed = false;

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

        private void Start()
        {
            //GameObject.Find("Cafe Visuals/Canvas Shelf Right/Canvas Shelf Right/");

        }

        //public void  cupconfill (Fillings filling)
        //{
        //    gamecupcontroller.FillCup(filling);
        //}

        /*private void Update()
        {
            if (!gotcupcontroller)
            {
                Dbgl($"ERROR: CupController not grabbed", true, false);
                return;
            }
            if (!gotunlocks)
            {
                Dbgl($"ERROR: unlocks not grabbed", true, false);
                return;
            }
            if (Input.GetKey("j"))
            {

            }
        }*/
            //char keypressed;
            //this is where the actual key stuff happens
            //Milk              z f4
            //Cream             x f6
            //WhippedCream      c
            //Ice               v
            //Coffee            a f1
            //Tea               s f3
            //Espresso          d f0
            //Sugar             f f7
            //Chocolate         g f2
            //Boba              q
            //Sprinklers        w
            //Caramelsauce      e
            //ChocolateSauce    r
            //BreastMilk    shift f5

            //Needs optimizations




            // TODO: Add stop noises
            /*
                        if (Input.GetKeyUp)
                        {
                        }*/

            /*
            if (!currentkey.Equals(""))
            {
                if (Input.GetKeyUp(currentkey))
                {
                    currentsound.EndLoop();
                    currentkey = "";
                }
                else
                {
                    gamecupcontroller.FillCup(currentfill);
                    return;
                }
            }

            if (!Input.anyKey)
                return;
            if (Input.GetKeyDown(confighotkeyMilk.Value) && unlockedMilk)
            {
                gamecupcontroller.FillCup(Fillings.Milk);
                soundMilk.PlayRandomLoop();
                return;
            }
            if (Input.GetKey(confighotkeyCream.Value) && unlockedCream)
            {
                gamecupcontroller.FillCup(Fillings.Cream);
                return;
            }
            if (Input.GetKey(confighotkeyCoffee.Value) && unlockedCoffee)
            {
                gamecupcontroller.FillCup(Fillings.Coffee);
                return;
            }
            if (Input.GetKey(confighotkeyTea.Value) && unlockedTea)
            {
                gamecupcontroller.FillCup(Fillings.Tea);
                return;
            }
            if (Input.GetKey(confighotkeyEspresso.Value) && unlockedEspresso)
            {
                gamecupcontroller.FillCup(Fillings.Espresso);
                return;
            }
            if (Input.GetKey(confighotkeySugar.Value) && unlockedSugar)
            {
                gamecupcontroller.FillCup(Fillings.Sugar);
                return;
            }
            if (Input.GetKey(confighotkeyChocolate.Value) && unlockedChocolate)
            {
                gamecupcontroller.FillCup(Fillings.Chocolate);
                return;
            }

            if (Input.GetKey(confighotkeyWhippedCream.Value) && unlockedWhippedCream)
            {

                gamecupcontroller.FillCup(Toppings.WhipedCream);
                return;
            }
            if (Input.GetKey(confighotkeyIce.Value) && unlockedIce)
            {
                GameObject.Find("Cafe Visuals/Canvas Shelf Right/").GetComponentInChildren<SoundEffectVariation>(true);
                gamecupcontroller.FillCup(Toppings.Ice);
                return;
            }
            if (Input.GetKey(confighotkeyBoba.Value) && unlockedBoba)
            {
                gamecupcontroller.FillCup(Toppings.Boba);
                return;
            }
            if (Input.GetKey(confighotkeySprinklers.Value) && unlockedSprinklers)
            {
                gamecupcontroller.FillCup(Toppings.Sprinkles);
                return;
            }
            if (Input.GetKey(confighotkeyCaramelsauce.Value) && unlockedCaramelsauce)
            {
                gamecupcontroller.FillCup(Toppings.CaramelSauce);
                return;
            }
            if (Input.GetKey(confighotkeyChocolateSauce.Value) && unlockedChocolateSauce)
            {
                gamecupcontroller.FillCup(Toppings.ChocolateSauce);
                return;
            }*/

            /*
            if (!Input.anyKeyDown)
                return;
            //We are only getting the 1st char detected, for now
            keypressed = Input.inputString[0];
            Dbgl($"Keypressed {keypressed}");

            if(isfilling[keypressed])
            {
                gamecupcontroller.FillCup(dicf[keypressed]);
            }
            else
            {
                gamecupcontroller.FillCup(dict[keypressed]);
            }*/

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
