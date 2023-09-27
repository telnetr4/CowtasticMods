using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using SkToolbox;
using UnityEngine.UI;
//using TMPro;


namespace Skcheats
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    [BepInDependency("com.Skrip.SkToolbox", BepInDependency.DependencyFlags.HardDependency)] // Set the dependency 
    //[BepInIncompatibility("com.telnet.eventspp")] // Just planning ahead for something... 
    class BepInExLoader : BaseUnityPlugin
    {
        //Harmony harmony = new Harmony(HARMONY_ID);

        public const string // Declare plugin information
            PLUGIN_AUTH = "telnet",
            PLUGIN_NAME = "Skcheats",
            PLUGIN_GUID = PLUGIN_AUTH + "." + PLUGIN_NAME,
            PLUGIN_VERS = "0.0.1.2",
            PLUGIN_RECV = "1.1.0.0";//,
            //HARMONY_ID = PLUGIN_GUID + "." + "harmony";


        public static ConfigEntry<bool> isDebug;
        public static void Dbgl(string str = "", bool pref = true)
        {
            var DbglLogSource = BepInEx.Logging.Logger.CreateLogSource("Dbgl");
            if (isDebug.Value)
                DbglLogSource.LogMessage((pref ? typeof(BepInExLoader).Namespace + " " : "") + str);
            //Debug.Log((pref ? typeof(SkBepInExLoader).Namespace + " " : "") + str);
        }

        private bool checkokversion(string version)
        {
            Version game = new Version(Application.version);
            Version recc = new Version(PLUGIN_RECV);
            bool check = game == recc;
            if (!check)
                Logger.LogWarning($"WARNING: {PLUGIN_NAME} v{PLUGIN_VERS} was not made for game version {Application.version}. You may experience errors.");
            return (game == recc);
        }

        static bool sceneready = false;
        //static internal OrderManager ordermanager;
        //static Button resetbutton;
        //static BaristaTalkManager bartalkmanager;
        //static BaristaMilkingHelper baristamilkinghelper;
        static GameObject scenegamemanager;
        static GameMode_Arcade GMA;
        static GameObject sandbutton;
        //static EventManager EM;
        static bool ischeater = false;
        //static TextMeshProUGUI Congratsmessage;


        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", false, "Enable debug");
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            //resetbutton = FindInActiveObjectByName("Button Reset Cup");
            checkokversion(PLUGIN_RECV);
            //harmony.PatchAll(Assembly.GetExecutingAssembly());

        }
        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            string scenename = arg0.name;
            Dbgl("check scene");
            if (scenename.Contains("Tutorial"))
            {
                Dbgl("tutorial");
                sceneready = false;
                return;
            }
            if (scenename.Contains("MainMenu"))
            {
                //Dbgl("MainMenu");
                ischeater = false;
                sceneready = false;
                Button[] listbuttons;
                listbuttons = GameObject.Find("Main Menu").GetComponentsInChildren<UnityEngine.UI.Button>(true);
                foreach (Button i in listbuttons)
                {
                    if (i.gameObject.name == "Button Sandbox")
                    {
                        Dbgl("sandbutton found");
                        sandbutton = i.gameObject;
                        break;
                    }
                }
                //try
                //{
                //    Congratsmessage.text = "Congratulations!";
                //}
                //catch
                //{
                //    Dbgl("Congratsmessage.text does not exist yet");
                //}
                Statics.TextActualTime = "Your Time";
                Statics.TextMoneyEarned = "Money Earned";
                return;
            }
            if (sandbutton != null)
                sandbutton = null;
            //string[] GMstring = scenename.Split('_');

            string GMstring = "GameManager " + scenename.Substring(5).Replace('_', ' ').Replace("UltraChaos", "Ultra Chaos").Replace(" Casual", "");
            Dbgl("string is \"" + GMstring + "\"");

            scenegamemanager = GameObject.Find(GMstring);
            //Dbgl(scenegamemanager.name);
            GMA = scenegamemanager.GetComponent<GameMode_Arcade>();
            ////ordermanager = GameObject.FindObjectOfType<OrderManager>(); Dbgl("OrderManager");
            ////ordermanager = GMA.GetComponent<OrderManager>();
            ////bartalkmanager = GameObject.Find("Canvas Dialog" + hardstring).GetComponent<BaristaTalkManager>();
            ////baristamilkinghelper = GameObject.Find("Sphere").GetComponent<BaristaMilkingHelper>();
            ////EM = scenegamemanager.GetComponent<EventManager>();
            ////arcademanager = 
            //Congratsmessage = GameObject.Find("Congrats").GetComponent< TextMeshProUGUI>();

            ////resetbutton = FindInActiveObjectByName("Button Reset Cup");

            sceneready = true;
        }

        static public bool checkissceneready(bool cmdprint = true,bool wantmainmenu=false)
        {

            if (sceneready == wantmainmenu)
            {
                if (cmdprint)
                    SkToolbox.Logger.Submit("Error: Not usable on this scene.");
                return false;
            }
            if (cmdprint)
            {
                SkToolbox.Logger.Submit("Ready!");
            }
            //sets that the user is using cheats
            ischeater = true;
            //Congratsmessage.text = "Congrats, cheater!";
            Statics.TextActualTime = "Your cheat Time";
            Statics.TextMoneyEarned = "Money Cheated";
            Dbgl("end checkscene");
            return true;
        }

        //[Command("setmoney", "set money.", "Cheats")]
        //public static void setmoneycmd(float moolah = 1000.0f)
        //{
        //    if (!checkissceneready())
        //    {
        //        return;
        //    }
        //    GMA.Money = moolah;
        //}
        //[Command("nothing", "nothing", "nothing")]
        //public static void nothing()
        //{
        //    //Dbgl("doing checkscene");

        //    //sets that the user is using cheats
        //    ischeater = true;
        //    //Congratsmessage.text = "Congrats, cheater!";
        //    Statics.TextActualTime = "Your cheat Time";
        //    Statics.TextMoneyEarned = "Money Cheated";
        //    //Dbgl("end checkscene");
        //    //Dbgl("Hello wolrd.");
        //}

        [Command("addmoney", "Add money.", "Cheats")]
        public static void addmoneycmd(float moolah = 100.0f)
        {
            if (!checkissceneready())
                return;
            GMA.AddMoney(moolah);
        }

        [Command("togglesandbox", "Temporarily toggles the Sandbox mode button. Can only be used on the main menu.", "Archievements")]
        public static void togglesandcmd()
        {
            if (!checkissceneready(true, true))
                return;
            sandbutton.SetActive(!sandbutton.activeSelf);
        }

#if DEBUG
        [Command("setpref", "sets a pref.", "Archievements")]
        public static void setprefcmd(string prefname, string stringin)
        {
            PlayerPrefs.SetString(prefname, stringin);
        }
#endif

        [Command("readstats", "reads stats.", "Archievements")]
        public static void readstatscmd(string stringin)
        {
            SkToolbox.Logger.Submit(PlayerPrefs.GetString(stringin));
        }

        //[Command("british", "british","british")]
        //public static void britishcmd()
        //{
        //    Statics.Boba = "Tea Bubbles";
        //    Statics.BreastMilk = "Tit-Tea";
        //    Statics.Milk = "White Tea";
        //    Statics.BaristaTalk_Cookie = new DialogSentence[]
        //    {
        //        new DialogSentence("I want that biscuit!", "Spoken_Giggles"),
        //        new DialogSentence("That biscuit looks tasty.", "Spoken_Giggles"),
        //        new DialogSentence("I wonder how that biscuit tastes...?", "Spoken_Giggles"),
        //        new DialogSentence("I wonder if the whole biscuit will fit in my mouth?", "Spoken_Giggles"),
        //        new DialogSentence("When do I get the magic biscuit?", "Spoken_Giggles"),
        //        new DialogSentence("This job promised certain benefits, right?", "Spoken_Giggles"),
        //        new DialogSentence("I want to grow...", "Spoken_Giggles"),
        //        new DialogSentence("I know you have magic for me~", "Spoken_Giggles")
        //    };

        //}

        ////[Command("submoney", "Subtract money.", "Cheats")]
        ////public static void submoneycmd(float moolah = 100.0f)
        ////{
        ////    if (!checkissceneready())
        ////        return;
        ////    GMA.SubMoney(moolah);  
        ////}

        [Command("sethappiness", "Set happiness.", "Cheats")]
        public static void sethappinesscmd(float h = 100.0f)
        {
            if (!checkissceneready())
                return;
            GMA.Happiness = h;
        }
    }
}