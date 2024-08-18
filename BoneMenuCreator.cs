using UnityEngine;
using MelonLoader;
using BoneLib.BoneMenu;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Expressions.BoneMenu
{
    public static partial class BoneMenuCreator
    {
        private static Page _mainPage = null;
        private static string _blendShapeName = "";
        private static List<string> _blendShapeNames = new List<string>();  // List to store blend shape names
        private const string PreferenceCategory = "Expressions";
        private const string PreferenceKey = "SavedBlendShapes";

        #region MENU CATEGORIES

        // Create a text input for the blend shape name
        public static void CreateStringInput(Page page, string name, Action<string> onValueChanged)
        {
            var element = page.CreateString(name, Color.white, _blendShapeName, (v) =>
            {
                _blendShapeName = v;
                onValueChanged?.Invoke(v);
            });
        }

        // Create the text input field with an add button next to it
        public static void CreateStringInputWithAddButton(Page page)
        {
            // Create a text input for the blend shape name
            CreateStringInput(page, "BlendShape Name", (v) =>
            {
                _blendShapeName = v;
            });

            // Create an add button next to the input
            page.CreateFunction("+", Color.green, () =>
            {
                if (!string.IsNullOrEmpty(_blendShapeName) && !_blendShapeNames.Contains(_blendShapeName))
                {
                    _blendShapeNames.Add(_blendShapeName);
                    SaveBlendShapes();
                    RefreshPage();
                }
                else
                {
                    LogMessage("Blend shape name is empty or already added.");
                }
            });
        }

        // Create a toggle for the blend shape
        public static void CreateBlendShapeToggle(Page page, string blendShapeName)
        {
            // Create a toggle button for the blend shape
            page.CreateBool(blendShapeName, Color.white, false, (isEnabled) =>
            {
                ToggleBlendShape(blendShapeName, isEnabled);
            });
        }

        #endregion

        public static void OnPrepareMainPage()
        {
            LogMessage("Preparing the Expressions page in BoneMenu...");

            // Create the main page
            _mainPage = Page.Root.CreatePage("Expressions", Color.blue);
            if (_mainPage == null)
            {
                LogError("Failed to create Expressions page.");
                return;
            }

            // Load saved blend shapes
            LoadBlendShapes();
        }

        public static void OpenMainPage()
        {
            if (_mainPage != null)
            {
                LogMessage("Opening the Expressions page...");
                Menu.OpenPage(_mainPage);
            }
            else
            {
                LogError("Main page is null, cannot open Expressions page.");
            }
        }

        public static void OnPopulateMainPage()
        {
            if (_mainPage == null)
            {
                LogError("Cannot populate a null main page.");
                return;
            }

            // Clear page to ensure it is empty
            _mainPage.RemoveAll();

            // Create the input field with add button for the blend shape name
            CreateStringInputWithAddButton(_mainPage);

            // Populate the existing blend shape toggles
            foreach (var blendShapeName in _blendShapeNames)
            {
                CreateBlendShapeToggle(_mainPage, blendShapeName);
            }
        }

        private static void RefreshPage()
        {
            // Re-populate the main page to reflect the newly added blend shape
            OnPopulateMainPage();
        }

        private static void ToggleBlendShape(string blendShapeName, bool isEnabled)
        {
            // Iterate over all root objects in the active scene
            foreach (GameObject rootObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                // Recursively search all child objects
                ToggleBlendShapeInGameObject(rootObject, blendShapeName, isEnabled);
            }
        }

        private static void ToggleBlendShapeInGameObject(GameObject gameObject, string blendShapeName, bool isEnabled)
        {
            // Get the SkinnedMeshRenderer component from the GameObject
            SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                // Find the blend shape index by name
                int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName);

                if (blendShapeIndex >= 0)  // Ensure the blend shape exists
                {
                    // Set the blend shape weight: 100% if enabled, 0% if disabled
                    float weight = isEnabled ? 100f : 0f;
                    skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, weight);
                    LogMessage($"Blend Shape '{blendShapeName}' on '{gameObject.name}' set to {weight}%.");
                }
                else
                {
                    LogError($"Blend Shape '{blendShapeName}' not found on '{gameObject.name}'.");
                }
            }

            // Recursively check and apply to children
            foreach (Transform child in gameObject.transform)
            {
                ToggleBlendShapeInGameObject(child.gameObject, blendShapeName, isEnabled);
            }
        }

        private static void SaveBlendShapes()
        {
            string serializedBlendShapes = string.Join(",", _blendShapeNames);
            MelonPreferences.SetEntryValue(PreferenceCategory, PreferenceKey, serializedBlendShapes);
            LogMessage("[Expressions] Blend shapes saved!");

            // Save preferences only once per session or major event
            MelonPreferences.Save();
        }

        private static void LoadBlendShapes()
        {
            var preferencesCategory = MelonPreferences.CreateCategory(PreferenceCategory);
            var savedBlendShapes = preferencesCategory.CreateEntry(PreferenceKey, "");

            if (!string.IsNullOrEmpty(savedBlendShapes.Value))
            {
                _blendShapeNames = savedBlendShapes.Value.Split(',').ToList();
                LogMessage("Blend shapes loaded.");
            }
        }

        private static void LogMessage(string message)
        {
#if DEBUG
                MelonLogger.Msg(message);
#endif
        }

        private static void LogError(string message)
        {
            MelonLogger.Error(message); // Always show errors
        }
    }

    public class ExpressionsMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            BoneMenuCreator.OnPrepareMainPage();
        }

        public override void OnLateInitializeMelon()
        {
            BoneMenuCreator.OnPopulateMainPage();
            BoneMenuCreator.OpenMainPage();
        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName)
        {
            // Scene Loaded
        }

        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            // Scene Initialized
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            // Scene Unloaded
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
            // Application quitting
        }

        public override void OnPreferencesSaved()
        {
            // Preferences Saved
        }

        public override void OnPreferencesLoaded()
        {
            // Preferences Loaded
        }
    }
}
