using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace lousyloader
{
    internal static class FileLoader
    {
        public static List<string> TextureModFolders = new List<string>();

        internal static void LoadTextures()
        {
            foreach (string modName in TextureModFolders)
            {
                string textureDir = BepInEx.Paths.PluginPath;
                try
                {
                    foreach (string filepath in Directory.EnumerateFiles(textureDir, "*.*", SearchOption.AllDirectories))
                    {
                        Texture2D texture2D = new Texture2D(2, 2, GraphicsFormat.R8G8B8A8_UNorm, 1, TextureCreationFlags.None);
                        texture2D.LoadImage(File.ReadAllBytes(filepath));
                    }
                }
                catch (Exception e)
                {
                    //Log.LogError("Error loading Textures. Please make sure you configured the mod folders correctly and it doesn't contain any unrelated files.");
                    //Logger.LogInfo(e.GetType() + " " + e.Message);
                }
            }
        }
    }
}
