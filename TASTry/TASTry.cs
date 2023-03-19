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
using DebugU = UnityEngine.Debug;
using UnityEngine.UI;
using TMPro;

namespace TASTry
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    [BepInDependency("com.Skrip.SkToolbox", BepInDependency.DependencyFlags.HardDependency)] // Set the dependency
    [BepInDependency("telnet.hotkeyhelper", BepInDependency.DependencyFlags.HardDependency)] // Set the dependency
    public class BepInExPlugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> isDebug;

        [Conditional("DEBUG")]
        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                DebugU.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }

        Harmony harmony = new Harmony(HARMONY_ID);

        public const string // Declare plugin information
            PLUGIN_AUTH = "telnet",
            PLUGIN_NAME = "TASTry",
            PLUGIN_GUID = PLUGIN_AUTH + "." + PLUGIN_NAME,
            PLUGIN_VERS = "0.0.0.1",
            PLUGIN_RECV = "1.0.0.24",
            HARMONY_ID = PLUGIN_GUID + "." + "harmony";

        static int scenetype = 0;

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

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            string scenename = arg0.name;
            if (scenename.Contains("MainMenu"))
            {
                Dbgl("MainMenu");
                scenetype = 0;

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
            //bartalkmanager = GameObject.Find("Canvas Dialog" + hardstring).GetComponent<BaristaTalkManager>();
            baristamilkinghelper = GameObject.Find("Sphere").GetComponent<BaristaMilkingHelper>();
            EM = scenegamemanager.GetComponent<EventManager>();
            //arcademanager = 
            Congratsmessage = GameObject.Find("Congrats").GetComponent<TextMeshProUGUI>();
        }
        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;


        }

        private void Update()
        {
            //TODO: check barista levels
            //TODO: milk first
            baristamilkinghelper.Invoke("OnMouseDown", 0);
            //TODO: 
            //TODO: check cup
            //TODO: fill til
            //TODO: submit
        }

        class botthought
        {
           private enum botdirective
            {
                checkbarista,
                milkbarista,
                resetifcup,
                checkcup,
                filltil,
                submit,
                upgrade
            }

            private botdirective curdirective = 0;

            void Awake()
            {

            }

            void Update()
            {

            }

            void checkcup()
            {
                this.Fullness = Mathf.Clamp01(this.Fullness);
                this.Espresso = Mathf.Clamp01(this.Espresso);
                this.Coffee = Mathf.Clamp01(this.Coffee);
                this.Chocolate = Mathf.Clamp01(this.Chocolate);
                this.Tea = Mathf.Clamp01(this.Tea);
                this.Milk = Mathf.Clamp01(this.Milk);
                this.BreastMilk = Mathf.Clamp01(this.BreastMilk);
                this.Cream = Mathf.Clamp01(this.Cream);
                this.Sugar = Mathf.Clamp01(this.Sugar);
            }

        }

        //TODO:start auto command
        //            Congratsmessage.text = "Congrats,TAS!";
        //Statics.TextActualTime = "TASTime";
        //    Statics.TextMoneyEarned = "TASMoney";
        //TODO:stop auto cmd
    }
}
