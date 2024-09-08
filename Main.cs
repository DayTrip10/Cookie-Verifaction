using MelonLoader;
using UnityEngine;
using System.Collections;

namespace cookieverifier
{
    // BuildInfo: Contains metadata for the mod
    public static class BuildInfo
    {
        public const string Name = "CookieVerifier"; // Name of the Mod
        public const string Description = "Cookie Verifier for Steam ID Whitelisting"; // Description of the Mod
        public const string Author = "DayTrip10"; // Author of the Mod
        public const string Company = null; // Company that made the Mod (null if none)
        public const string Version = "1.0.0"; // Version of the Mod
        public const string DownloadLink = null; // Download Link for the Mod (null if none)
    }

    // Main mod class extending MelonMod
    public class CookieVerifierMod : MelonMod
    {
        private bool objectsFound = false; // Flag to stop the loop once objects are found

        public override void OnInitializeMelon()
        {
            // Start checking for game objects only if the player is verified
            if (WhitelistManager.IsPlayerVerified())
            {
                MelonCoroutines.Start(CheckForGripObjects());
            }
        }

        // Coroutine to check for the existence of the game objects every 5 seconds
        private IEnumerator CheckForGripObjects()
        {
            while (!objectsFound)
            {
                // Look for "Grip Norm" and "Grip Modded" objects in the scene
                GameObject gripNorm = GameObject.Find("Grip Norm");
                GameObject gripModded = GameObject.Find("Grip Modded");

                // If both objects are found, toggle their active states
                if (gripNorm != null && gripModded != null)
                {
                    gripNorm.SetActive(false);  // Deactivate "Grip Norm"
                    gripModded.SetActive(true); // Activate "Grip Modded"
                    objectsFound = true;        // Stop checking after objects are found
                }

                // Wait for 5 seconds before checking again
                yield return new WaitForSeconds(5f);
            }
        }
    }
}
