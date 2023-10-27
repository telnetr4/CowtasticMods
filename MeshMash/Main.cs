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
// Lovingly ripped off from aedenthorn.CustomMeshes 0.2.3

namespace MeshMash
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal const string PLUGIN_GUID = "telnet.meshmash";
        internal const string PLUGIN_NAME = "MeshMash";
        internal const string PLUGIN_VERS = "1.0.0.0";

        private static Dictionary<string, Dictionary<string, Dictionary<string, CustomMeshData>>> customMeshes = new Dictionary<string, Dictionary<string, Dictionary<string, CustomMeshData>>>();
        private static Dictionary<string, AssetBundle> customAssetBundles = new Dictionary<string, AssetBundle>();
        private static Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> customGameObjects = new Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>>();
        private static BepInExPlugin context;

        public static ConfigEntry<bool> isDebug;

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }
        public static Mesh customMesh { get; set; }

        private void Awake()
        {
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug");

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            PreloadMeshes();
        }
        private static void PreloadMeshes()
        {
            foreach (AssetBundle ab in customAssetBundles.Values)
                ab.Unload(true);
            customMeshes.Clear();
            customGameObjects.Clear();
            customAssetBundles.Clear();

            Dbgl($"Importing meshes");

            string path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "CustomMeshes");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return;
            }

            foreach (string dir in Directory.GetDirectories(path))
            {
                string dirName = Path.GetFileName(dir);
                Dbgl($"Importing meshes: {dirName}");

                customMeshes[dirName] = new Dictionary<string, Dictionary<string, CustomMeshData>>();
                customGameObjects[dirName] = new Dictionary<string, Dictionary<string, GameObject>>();

                foreach (string subdir in Directory.GetDirectories(dir))
                {
                    string subdirName = Path.GetFileName(subdir);
                    Dbgl($"Importing meshes: {dirName}\\{subdirName}");

                    customMeshes[dirName][subdirName] = new Dictionary<string, CustomMeshData>();
                    customGameObjects[dirName][subdirName] = new Dictionary<string, GameObject>();

                    foreach (string file in Directory.GetFiles(subdir))
                    {
                        try
                        {
                            SkinnedMeshRenderer renderer = null;
                            Mesh mesh = null;
                            Dbgl($"Importing {file} {Path.GetFileNameWithoutExtension(file)} {Path.GetFileName(file)} {Path.GetExtension(file).ToLower()}");
                            string name = Path.GetFileNameWithoutExtension(file);
                            if (name == Path.GetFileName(file))
                            {
                                AssetBundle ab = AssetBundle.LoadFromFile(file);
                                customAssetBundles.Add(name, ab);

                                GameObject prefab = ab.LoadAsset<GameObject>("Player");
                                if (prefab != null)
                                {
                                    renderer = prefab.GetComponentInChildren<SkinnedMeshRenderer>();
                                    if (renderer != null)
                                    {
                                        mesh = renderer.sharedMesh;
                                        Dbgl($"Importing {file} asset bundle as player");
                                    }
                                    else
                                    {
                                        Dbgl($"No SkinnedMeshRenderer on {prefab}");
                                    }
                                    if (mesh == null)
                                        mesh = ab.LoadAsset<Mesh>("body");
                                }
                                else
                                {
                                    mesh = ab.LoadAsset<Mesh>("body");

                                    if (mesh != null)
                                    {
                                        Dbgl($"Importing {file} asset bundle as mesh");
                                    }
                                    else
                                    {
                                        Dbgl("Failed to find body");
                                    }
                                }
                            }
                            else if (Path.GetExtension(file).ToLower() == ".fbx")
                            {
                                GameObject obj = MeshImporter.Load(file);
                                GameObject obj2 = obj?.transform.Find("Player")?.Find("Visual")?.gameObject;
                                //
                                /*
                                int children = obj.transform.childCount;
                                for(int i = 0; i < children; i++)
                                {
                                    Dbgl($"fbx child: {obj.transform.GetChild(i).name}");
                                }
                                */
                                mesh = obj.GetComponentInChildren<MeshFilter>().mesh;
                                if (obj2 != null)
                                    renderer = obj2.GetComponentInChildren<SkinnedMeshRenderer>();
                                if (mesh != null)
                                {
                                    if (renderer != null)
                                        Dbgl($"Importing {file} fbx as player");
                                    else
                                        Dbgl($"Importing {file} fbx as mesh");
                                }
                            }
                            else if (Path.GetExtension(file).ToLower() == ".obj")
                            {
                                mesh = new ObjImporter().ImportFile(file);
                                if (mesh != null)
                                    Dbgl($"Imported {file} obj as mesh");
                            }
                            if (mesh != null)
                            {
                                customMeshes[dirName][subdirName].Add(name, new CustomMeshData(dirName, name, mesh, renderer));
                                Dbgl($"Added mesh data to customMeshes[{dirName}][{subdirName}][{name}]");
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(BaristaController), "Awake")]
        static class BaristaController_Patch
        {
            static void Postfix(BaristaController __instance)
            {
                Dbgl($"{__instance.name} Awake");
                SkinnedMeshRenderer BaristaSkinMesh = __instance.GetComponentInChildren<SkinnedMeshRenderer>();
                Dbgl(BaristaSkinMesh.name);
                Transform insttransf = __instance.GetComponent<Transform>();

            }
        }

        private static Transform RecursiveFind(Transform parent, string childName)
        {
            Transform child = null;
            for (int i = 0; i < parent.childCount; i++)
            {
                child = parent.GetChild(i);
                if (child.name == childName)
                    break;
                child = RecursiveFind(child, childName);
                if (child != null)
                    break;
            }
            return child;
        }
    }

}