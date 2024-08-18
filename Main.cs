using Expressions.BoneMenu;
using MelonLoader;

namespace Expressions
{
    public static class BuildInfo
    {
        public const string Name = "Expressions"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Expressions Menu for BONELAB Avatars"; // Description for the Mod.  (Set as null if none)
        public const string Author = "DayTrip10"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class ExpressionsMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Expressions Mod Initialized");
            BoneMenuCreator.OnPrepareMainPage();
        }

        public override void OnLateInitializeMelon()
        {
            MelonLogger.Msg("Late Initialization of Expressions Mod");
            BoneMenuCreator.OnPopulateMainPage();
            BoneMenuCreator.OpenMainPage();
        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName)
        {
            MelonLogger.Msg($"Scene Loaded: {sceneName} (Index: {buildindex})");
            // You might want to repopulate or update the BoneMenu depending on the scene
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            MelonLogger.Msg($"Scene Initialized: {sceneName} (Index: {buildindex})");
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"Scene Unloaded: {sceneName} (Index: {buildIndex})");
        }

        public override void OnUpdate()
        {
            // Handle updates here, if necessary
        }

        public override void OnFixedUpdate()
        {
            // Handle fixed updates here, if necessary
        }

        public override void OnLateUpdate()
        {
            // Handle late updates here, if necessary
        }

        public override void OnGUI()
        {
            // Handle GUI updates here, if necessary
        }

        public override void OnApplicationQuit()
        {
            MelonLogger.Msg("Expressions Mod is shutting down");
        }

        public override void OnPreferencesSaved()
        {
            MelonLogger.Msg("Preferences Saved");
        }

        public override void OnPreferencesLoaded()
        {
            MelonLogger.Msg("Preferences Loaded");
        }
    }
}
