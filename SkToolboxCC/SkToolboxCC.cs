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
using TMPro;


namespace SkToolboxCC
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    [BepInDependency("com.Skrip.SkToolbox", BepInDependency.DependencyFlags.HardDependency)] // Set the dependency 
    //[BepInIncompatibility("com.telnet.eventspp")] // Just planning ahead for something... 
    class BepInExLoader : BaseUnityPlugin
    {
        public const string // Declare plugin information
            PLUGIN_AUTH = "telnet",
            PLUGIN_NAME = "SkToolboxCC",
            PLUGIN_GUID = PLUGIN_AUTH + "." + PLUGIN_NAME,
            PLUGIN_VERS = "0.0.1.0",
            PLUGIN_RECV = "1.0.0.24";


        public static ConfigEntry<bool> isDebug;
        public static void Dbgl(string str = "", bool pref = true)
        {
            var DbglLogSource = BepInEx.Logging.Logger.CreateLogSource("Dbgl");
            if (isDebug.Value)
                DbglLogSource.LogDebug((pref ? typeof(BepInExLoader).Namespace + " " : "") + str);
            //Debug.Log((pref ? typeof(SkBepInExLoader).Namespace + " " : "") + str);
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
        static internal OrderManager ordermanager;
        static Button resetbutton;
        static BaristaTalkManager bartalkmanager;
        static BaristaMilkingHelper baristamilkinghelper;
        static GameObject scenegamemanager;
        static GameMode_Arcade GMA;
        static EventManager EM;
        static bool ischeater = false;
        static TextMeshProUGUI Congratsmessage;


        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            resetbutton = FindInActiveObjectByName("Button Reset Cup");
            checkokversion(PLUGIN_RECV);

        }
        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            string scenename = arg0.name;
            //Dbgl("check scene");
            if (scenename.Contains("MainMenu"))
            {
                //Dbgl("MainMenu");
                ischeater = false;
                sceneready = false;

                try
                {
                    Congratsmessage.text = "Congratulations!";
                }
                catch
                {
                    Dbgl("Congratsmessage.text does not exist yet");
                }
                Statics.TextActualTime = "Your Time";
                Statics.TextMoneyEarned = "Money Earned";
                return;
            }
            bool ishard = scenename.Contains("Hard");
            string hardstring = (ishard ? " Hard" : "");
            scenegamemanager = GameObject.Find("GameManager Arcade" + hardstring);
            GMA = scenegamemanager.GetComponent<GameMode_Arcade>();
            ordermanager = GameObject.FindObjectOfType<OrderManager>(); Dbgl("OrderManager");
            //ordermanager = GMA.GetComponent<OrderManager>();
            bartalkmanager = GameObject.Find("Canvas Dialog" + hardstring).GetComponent<BaristaTalkManager>();
            baristamilkinghelper = GameObject.Find("Sphere").GetComponent<BaristaMilkingHelper>();
            EM = scenegamemanager.GetComponent<EventManager>();
            //arcademanager = 
            Congratsmessage = GameObject.Find("Congrats").GetComponent< TextMeshProUGUI>();

            resetbutton = FindInActiveObjectByName("Button Reset Cup");

            sceneready = true;
        }

        static private bool checkissceneready(bool cmdprint = true)
        {
            if (!sceneready)
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
            Congratsmessage.text = "Congrats, cheater!";
            Statics.TextActualTime = "Your cheat Time";
            Statics.TextMoneyEarned = "Money Cheated";
            return true;
        }

        [Command("setmoney", "set money.", "Cheats")]
        public static void setmoneycmd(float moolah = 1000.0f)
        {
            if (!checkissceneready())
            {
                return;
            }
            GMA.Money = moolah;
        }

        [Command("addmoney", "Add money.", "Cheats")]
        public static void addmoneycmd (float moolah = 100.0f)
        {
            if (!checkissceneready())
                return;
            GMA.AddMoney(moolah);

        }

        //[Command("submoney", "Subtract money.", "Cheats")]
        //public static void submoneycmd(float moolah = 100.0f)
        //{
        //    if (!checkissceneready())
        //        return;
        //    GMA.SubMoney(moolah);  
        //}

        [Command("sethappiness", "Set happiness.", "Cheats")]
        public static void sethappinesscmd(float h = 100.0f)
        {
            if (!checkissceneready())
                return;
            GMA.Happiness = h;
        }

        [Command("randomevent", "Starts a random event.", "Events")]
        public static void randomeventcmd()
        {
            if (!checkissceneready())
                return;
            EM.RandomStartEvent();
        }

        [Command("endactiveevents", "Ends all active events.", "Events")]
        public static void endactiveeventscmd()
        {
            if (!checkissceneready())
                return;
            EM.EndActiveEvents();
        }

        [Command("barisay", "Have the barista say something.", "Misc")]
        public static void barisay(string talk = "something", float startdelay = 0f, float stopdelay = 20f)
        {
            if (!checkissceneready())
                return;
            bartalkmanager.StartBaristaTalk(talk, startdelay, stopdelay);
        }


        //[Command("testit", "Have the barista say something.", "Misc")]
        //public static void testit()
        //{
        //    SkToolbox.Logger.Submit(EM.GetPrivateField<float>("TimeEventStart").ToString());
        //}

        [Command("startEventint", "Start an event via int:\n0 = Always Flustered\n1=Sell More Value\n2=Fast Milk Fill\n3=Not Flustered Customers\n4=Milk Burst", "Events")]
        public static void starteventcmd(int i)
        {
            if (!checkissceneready())
                return;

            EM.EndActiveEvents();
            switch (i)
            {
                case 0:
                    if (EM.EventAlwaysFlustered)
                    {
                        EM.SetEventAlwaysFlustered(state: true);
                    }
                    break;
                case 1:
                    if (EM.EventSellMoreValue)
                    {
                        EM.SetEventSellMoreValue(state: true);
                    }
                    break;
                case 2:
                    if (EM.EventFastMilkFill)
                    {
                        EM.SetEventFastMilkFill(state: true);
                    }
                    break;
                case 3:
                    if (EM.EventNotFlusteredCustomers)
                    {
                        EM.SetEventNotFlusteredCustomer(state: true);
                    }
                    break;
                case 4:
                    if (EM.EventMilkBurst)
                    {
                        EM.SetEventEventMilkBurst(state: true);
                    }
                    break;
                default:
                    {
                        SkToolbox.Logger.Submit("Error: Invalid selection.");
                    }
                    break;
            }
            if (EM.EventIsActive)
            {
                EM.SetPrivateField<float>("TimeEventStart", Time.timeSinceLevelLoad);
                //EM.TimeEventStart = Time.timeSinceLevelLoad;
                EM.IconMainObject.SetActive(value: true);
            }
        }
        //LevelManager.ReloadScene
        //LevelManager.ChangeSceneDirect





    }
}