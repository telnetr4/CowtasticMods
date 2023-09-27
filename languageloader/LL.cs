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
using SkToolbox;
using System.Xml;
using System.Xml.Serialization;
using Debug = UnityEngine.Debug;
using UnityEngine.Experimental.Rendering;
using System.Linq;


namespace lousyloader
{
    public class objtexttureload
    {
        private GameObject obj;
        private SkinnedMeshRenderer SMR;
        
        public objtexttureload(string objname)
        {
            this.obj = GameObject.Find("objname");
            this.SMR = this.obj.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public void destroy()
        {
            this.obj = null;
            this.SMR = null;
        }

    }

    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    [BepInDependency("com.Skrip.SkToolbox", BepInDependency.DependencyFlags.HardDependency)] // Set the dependency 
    //[BepInIncompatibility("com.telnet.eventspp")] // Just planning ahead for something... 
    class BepInExLoader : BaseUnityPlugin
    {
        public const string // Declare plugin information
            PLUGIN_AUTH = "telnet",
            PLUGIN_NAME = "lousyloader",
            PLUGIN_GUID = PLUGIN_AUTH + "." + PLUGIN_NAME,
            PLUGIN_VERS = "0.0.0.1",
            PLUGIN_RECV = "1.1.0.1";

        public static ConfigEntry<bool> isDebug;

        public static void Dbgl(string str = "")
        {
            var DbglLogSource = BepInEx.Logging.Logger.CreateLogSource(typeof(BepInExLoader).Namespace);
            if (isDebug.Value)
                DbglLogSource.LogMessage(str);
            //Debug.Log((pref ? typeof(SkBepInExLoader).Namespace + " " : "") + str);
        }

        public static GameObject BaristaObj;
        public static SkinnedMeshRenderer BaristaSMR;
        public static GameObject ApronObj;
        public static SkinnedMeshRenderer ApronSMR;
        public static GameObject PantsObj;
        public static SkinnedMeshRenderer PantsSMR;
        public static GameObject PoofyPantsObj;
        public static SkinnedMeshRenderer PoofyPantsSMR;
        public static GameObject BikiniObj;
        public static SkinnedMeshRenderer BikiniSMR;
        public static GameObject CafeObj;
        public static MeshRenderer CafeMR;
        public static Texture2D bodytext = new Texture2D(4096, 4096);
        public static Texture2D toptext = new Texture2D(4096, 4096);
        public static Texture2D facetext = new Texture2D(4096, 4096);
        public static Texture2D pantstex = new Texture2D(2048, 1365);

        private bool checkokversion(string version)
        {
            Version game = new Version(Application.version);
            Version recc = new Version(PLUGIN_RECV);
            bool check = game == recc;
            if (!check)
                Logger.LogWarning($"WARNING: {PLUGIN_NAME} v{PLUGIN_VERS} was not made for game version {Application.version}. You may experience errors.");
            return (game == recc);
        }

        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            //AddStreamingAssetsTranslations();
            //this.BaristaAni = base.GetComponent<Animator>();
            //BaristaAni.GetComponent<GameObject>().GetComponentInChildren<SkinnedMeshRenderer>();
            Loadfilenames();
        }

        private string AssetsPath = Path.Combine(BepInEx.Paths.PluginPath, PLUGIN_NAME);

        static string[] filenames;
        public void Loadfilenames()
        {
            filenames = Directory.GetFiles(this.AssetsPath, "*.png", SearchOption.TopDirectoryOnly);
            //Texture2D test = new Texture2D(2, 2);
            Dbgl("loaded from file");

        }

        public static void loadimagecomponent(Texture2D texture, string filename, string name)
        {
            //SkToolbox.Logger.Submit("Starting loading "+ Path.Combine(BepInEx.Paths.PluginPath, PLUGIN_NAME, name, "Barista_Body.png"));
            try
            {
                
                Resources.Load<Texture2D>(Path.Combine(BepInEx.Paths.PluginPath, PLUGIN_NAME, name, filename));
                //texture.LoadImage(File.ReadAllBytes(Path.Combine(BepInEx.Paths.PluginPath, PLUGIN_NAME, name, filename)));
            }
            catch
            {
                SkToolbox.Logger.Submit("PNG file " + name + "/Barista_Body.png does not exist");
                return;
            }
        }

        [Command("loadimage", "Loads character skins based on the folder name.", "Lousy Loader")]
        public static void loadimagecmd(string name)
        {
            ////SkToolbox.Logger.Submit("Starting loading "+ Path.Combine(BepInEx.Paths.PluginPath, PLUGIN_NAME, name, "Barista_Body.png"));
            //try
            //{
            //    bodytext.LoadImage(File.ReadAllBytes(Path.Combine(BepInEx.Paths.PluginPath, PLUGIN_NAME, name, "Barista_Body.png")));
            //}
            //catch
            //{
            //    SkToolbox.Logger.Submit("PNG file " + name + "/Barista_Body.png does not exist");
            //    return;
            //}
            loadimagecomponent(bodytext, "Barista_Body.png", name);
            loadimagecomponent(toptext, "Barista_Top.png", name);
            loadimagecomponent(facetext, "Barista_Face.png", name);
            loadimagecomponent(pantstex, "Pants.png", name);

            if (BaristaObj == null)
            {
                BaristaObj = GameObject.Find("Barista");
                BaristaSMR = BaristaObj.GetComponentInChildren<SkinnedMeshRenderer>();
            }

            if (ApronObj == null)
            {
                ApronObj = GameObject.Find("Apron");
                ApronSMR = ApronObj.GetComponentInChildren<SkinnedMeshRenderer>();
            }

            if (PantsObj == null)
            {
                PantsObj = GameObject.Find("Pants");
                PantsSMR = PantsObj.GetComponentInChildren<SkinnedMeshRenderer>();
            }
            
            //SkToolbox.Logger.Submit("found barb");

            Texture2D[] texturearray = {bodytext, toptext, toptext, bodytext, facetext};
            //BaristaSMR.material.SetTexture(bodytext);
            for (int i = 0; i<5; i++)
            {
                //SkToolbox.Logger.Submit(i.ToString());
                BaristaSMR.materials[i].SetTexture("_Texture",texturearray[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                //SkToolbox.Logger.Submit(i.ToString());
                ApronSMR.materials[i].SetTexture("_Texture", toptext);
            }

            PantsSMR.materials[0].SetTexture("_Texture", pantstex);


        }
    }
}
