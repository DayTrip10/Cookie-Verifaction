using UnityEngine;
using BoneLib.BoneMenu;
using MelonLoader;
using Il2CppSLZ.Marrow.Data;
using Il2CppSLZ.Marrow.Pool;
using LabFusion.Data;
using LabFusion.Utilities;
using System.Collections;
using LabFusion.Debugging; //The crowbar spawn method is in this namespace so idk why it dosent exist

namespace Everyday_Blower
{
    public static class BoneMenuCreator
    {
        private static Page _mainPage = null;
        private static bool isSpawningCrowbars = false;
        private static Transform spawnLocation;

        public static void OnPrepareMainPage()
        {
            _mainPage = Page.Root.CreatePage("Everyday Blower", Color.blue);
            InitializeSpawnLocation();
        }

        public static void OpenMainPage()
        {
            Menu.OpenPage(_mainPage);
        }

        public static void OnPopulateMainPage()
        {
            // Clear page
            _mainPage.RemoveAll();

            // Create Everyday Blower menu
            _mainPage.CreateFunction("Activate Blower", Color.green, () =>
            {
                MelonLogger.Msg("Blower activated!");
            });

            _mainPage.CreateFunction("Deactivate Blower", Color.red, () =>
            {
                MelonLogger.Msg("Blower deactivated!");
            });

            _mainPage.CreateFloat("Blower Speed", Color.white, 0.1f, 1.0f, 0.0f, 2.0f, (speed) =>
            {
                MelonLogger.Msg($"Blower speed set to {speed}");
            });

            var spawnCrowbarsToggle = _mainPage.CreateBool("Spawn Crowbars", Color.yellow, false, (isToggled) =>
            {
                isSpawningCrowbars = isToggled;
                if (isSpawningCrowbars)
                {
                    MelonLogger.Msg("Crowbar spawning activated!");
                    StartSpawningCrowbars();
                }
                else
                {
                    MelonLogger.Msg("Crowbar spawning deactivated!");
                }
            });
        }

        private static void InitializeSpawnLocation()
        {
            // Assuming you have a default spawn location in your scene
            // Adjust this logic as needed to find or set the spawn location
            spawnLocation = new GameObject("SpawnLocation").transform;
            spawnLocation.position = Vector3.zero; // Set your desired spawn position
            MelonLogger.Msg("Spawn location initialized.");
        }

        private static void StartSpawningCrowbars()
        {
            MelonCoroutines.Start(SpawnCrowbarsCoroutine());
        }

        private static IEnumerator SpawnCrowbarsCoroutine()
        {
            while (isSpawningCrowbars)
            {
                if (spawnLocation != null)
                {
                    DebugZoneMigrator.SpawnMigrator(); // Ensure this method is available and correctly referenced
                    MelonLogger.Msg("Crowbar spawned!");
                }
                else
                {
                    MelonLogger.Error("Spawn location is null!");
                }
                yield return new WaitForSeconds(1f); // Adjust the spawn interval as needed
            }
        }
    }
}
