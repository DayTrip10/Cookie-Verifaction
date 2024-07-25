using MelonLoader;

namespace Blower
{
    public static class BuildInfo
    {
        public const string Name = "EverydayBlower"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "Fusion Cheat Menu"; // Description for the Mod.  (Set as null if none)
        public const string Author = "DayTrip10"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "1.0.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }

    public class Blowermod : MelonMod
    {
        public override void OnInitializeMelon() {
            
        }

        public override void OnLateInitializeMelon() // Runs after OnApplicationStart.
        {
            
        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName) // Runs when a Scene has Loaded and is passed the Scene's Build Index and Name.
        {
            
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName) // Runs when a Scene has Initialized and is passed the Scene's Build Index and Name.
        {
           
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName) {
           
        }

        public override void OnUpdate() // Runs once per frame.
        {
           
        }

        public override void OnFixedUpdate() // Can run multiple times per frame. Mostly used for Physics.
        {
           
        }

        public override void OnLateUpdate() // Runs once per frame after OnUpdate and OnFixedUpdate have finished.
        {
            
        }

        public override void OnGUI() // Can run multiple times per frame. Mostly used for Unity's IMGUI.
        {
           
        }

        public override void OnApplicationQuit() // Runs when the Game is told to Close.
        {
            
        }

        public override void OnPreferencesSaved() // Runs when Melon Preferences get saved.
        {
            
        }

        public override void OnPreferencesLoaded() // Runs when Melon Preferences get loaded.
        {
            
        }
    }
}