using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Debug = UnityEngine.Debug;


namespace STFU
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERS)]
    //[BepInDependency(com.Skrip.SkToolbox, BepInDependency.DependencyFlags.SoftDependency)] // Set the dependency 
    //[BepInIncompatibility("com.telnet.eventspp")] // Just planning ahead for something... 
    public class BepInExPlugin : BaseUnityPlugin
    {
        public const string // Declare plugin information
            PLUGIN_AUTH = "telnet",
            PLUGIN_NAME = "STFU",
            PLUGIN_GUID = PLUGIN_AUTH + "." + PLUGIN_NAME,
            PLUGIN_VERS = "0.0.1.0",
            PLUGIN_RECV = "1.0.0.24";

        internal static BaseGameMode Test;
        public static ConfigEntry<bool> isDebug;
        Harmony _harmony = new Harmony(PLUGIN_GUID + ".Hid");

        public static void Dbgl(string str = "", bool pref = true)
        {
            var DbglLogSource = BepInEx.Logging.Logger.CreateLogSource("Dbgl");
            if (isDebug.Value)
                DbglLogSource.LogDebug((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }

        private void Awake()
        {
            Logger.LogInfo("Patching " + _harmony.Id);
            //_harmony.PatchAll(typeof(Patchclass));
            _harmony.PatchAll(typeof(NOUPVIS));

            Logger.LogInfo("Patched!");
            GameObject ass = GameObject.Find("ass");
            Logger.LogInfo(ass.name);
        }

        private void OnDestroy()
        {
            _harmony.UnpatchAll();
        }

        class NOUPVIS
        {
            [HarmonyPatch(typeof(ButtonUpgrade), "UpdateVisuals")]
            [HarmonyPrefix]
            static bool UpdateVisualsPrefix()
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(ButtonUpgrade), "UpdateVisuals")]
        public class Patchclass
        {
            [HarmonyTranspiler]
            IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return new CodeMatcher(instructions)
                    .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
                        new CodeMatch(OpCodes.Ldstr),
                        new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(Debug), "Log")))
                    .RemoveInstructions(2)
                    .InstructionEnumeration();
            }
        }

    }
}

            /*
            [HarmonyTranspiler]

            IEnumerable<CodeInstruction> OnClickTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                return new CodeMatcher(instructions)
                    .MatchForward(false, // false = move at the start of the match, true = move at the end of the match
                        new CodeMatch(OpCodes.Ldarg_0),
                        new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(ButtonUpgrade), "DoUpgrade")))
                    .Insert(new CodeInstruction(OpCodes.Ldstr))
                    .SetOperandAndAdvance("Upgrade Times Limit Reached").InsertAndAdvance(
        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Debug), "Log"))
    )
                    .InstructionEnumeration();
            }
        }
    }
}
/*foreach (CodeInstruction instruction in instructions)
{
    // Match against call SomeOtherMethod(int32)
    // At that point the value of `foo` is on the stack, in which case our delegate will consume it
    // This is why our delegate returns the new value that's pushed onto the stack
    if (instruction.opcode == OpCodes.Call)
        yield return Transpilers.EmitDelegate<Func<int, int>>(foo => foo + 5); // Emit a `call Delegate`
    yield return instruction;
}
}
}
}*/

