using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Reflection;

namespace NoGigglingInColonistBar;

[StaticConstructorOnStartup]
public class HarmonyPatches
{
    static HarmonyPatches()
    {
        var harmony = new Harmony("llunak.NoGigglingInColonistBar");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}
