using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace NoGigglingInColonistBar;

[DefOf]
public static class DefOfs
{
    public static MentalStateDef Giggling;

    static DefOfs()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DefOfs));
    }
}

[HarmonyPatch(typeof(ColonistBarColonistDrawer))]
public static class ColonistBarColonistDrawer_Patch
{
    public static bool inDrawColonist = false;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(ColonistBarColonistDrawer.DrawColonist))]
    public static void Prefix()
    {
        inDrawColonist = true;
    }

    [HarmonyFinalizer]
    [HarmonyPatch(nameof(ColonistBarColonistDrawer.DrawColonist))]
    public static void Finalizer()
    {
        inDrawColonist = false;
    }
}

[HarmonyPatch(typeof(Pawn))]
public static class Pawn_Patch
{
    private static bool ignore = false;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Pawn.MentalStateDef))]
    [HarmonyPatch(MethodType.Getter)]
    public static MentalStateDef Postfix( MentalStateDef result )
    {
        if( !ignore && ColonistBarColonistDrawer_Patch.inDrawColonist && result == DefOfs.Giggling )
            return null;
        return result;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Pawn.MentalState))]
    [HarmonyPatch(MethodType.Getter)]
    public static MentalState Postfix( MentalState result )
    {
        if( ColonistBarColonistDrawer_Patch.inDrawColonist && result?.def == DefOfs.Giggling )
            return null;
        return result;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Pawn.InMentalState))]
    [HarmonyPatch(MethodType.Getter)]
    public static bool Postfix( bool result, Pawn __instance )
    {
        if( ColonistBarColonistDrawer_Patch.inDrawColonist && result )
        {
            ignore = true;
            bool isGiggling = __instance.MentalStateDef == DefOfs.Giggling;
            ignore = false;
            if( isGiggling )
                return false;
        }
        return result;
    }
}
