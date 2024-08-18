using UnityEngine;
using MelonLoader;
using BoneLib.BoneMenu;
using System;  // Add this using directive to include Action<> and other System types
using System.Collections.Generic;

namespace Expressions.BoneMenu
{
    public static partial class BoneMenuCreator
    {
        private static Page _mainPage = null;
        private static SkinnedMeshRenderer _skinnedMeshRenderer;
        private static string _blendShapeName = "";
        private static Dictionary<string, bool> _blendShapeToggles = new Dictionary<string, bool>();

        #region MENU CATEGORIES

        public static void CreateStringInput(Page page, string name, Action<string> onValueChanged)
        {
            var element = page.CreateString(name, Color.white, _blendShapeName, (v) =>
            {
                _blendShapeName = v;
                onValueChanged?.Invoke(v);
            });
        }

        public static void CreateBoolToggle(Page page, string blendShapeName)
        {
            if (_skinnedMeshRenderer == null)
            {
                MelonLogger.Error("SkinnedMeshRenderer is not assigned.");
                return;
            }

            if (!_blendShapeToggles.ContainsKey(blendShapeName))
            {
                _blendShapeToggles[blendShapeName] = false;
            }

            var element = page.CreateBool(blendShapeName, Color.white, _blendShapeToggles[blendShapeName], (v) =>
            {
                _blendShapeToggles[blendShapeName] = v;
                ToggleBlendShape(blendShapeName, v);
            });
        }

        public static void CreateAddToggleButton(Page page)
        {
            page.CreateFunction("Add BlendShape Toggle", Color.green, () =>
            {
                if (!string.IsNullOrEmpty(_blendShapeName))
                {
                    CreateBoolToggle(_mainPage, _blendShapeName);
                }
                else
                {
                    MelonLogger.Msg("Blend shape name is empty.");
                }
            });
        }

        #endregion

        public static void OnPrepareMainPage()
        {
            MelonLogger.Msg("Preparing the Expressions page in BoneMenu...");

            // Create the main page
            _mainPage = Page.Root.CreatePage("Expressions", Color.blue);
            if (_mainPage == null)
            {
                MelonLogger.Error("Failed to create Expressions page.");
                return;
            }

            // Try to find the SkinnedMeshRenderer
            _skinnedMeshRenderer = GameObject.Find("YourSkinnedMeshRendererObject")?.GetComponent<SkinnedMeshRenderer>();
            if (_skinnedMeshRenderer == null)
            {
                MelonLogger.Error("SkinnedMeshRenderer not found. Please check the GameObject name.");
            }
        }

        public static void OpenMainPage()
        {
            if (_mainPage != null)
            {
                MelonLogger.Msg("Opening the Expressions page...");
                Menu.OpenPage(_mainPage);
            }
            else
            {
                MelonLogger.Error("Main page is null, cannot open Expressions page.");
            }
        }

        public static void OnPopulateMainPage()
        {
            if (_mainPage == null)
            {
                MelonLogger.Error("Cannot populate a null main page.");
                return;
            }

            // Clear page to ensure it is empty
            _mainPage.RemoveAll();

            // Create the input field for the blend shape name
            CreateStringInput(_mainPage, "BlendShape Name", (v) =>
            {
                _blendShapeName = v;
            });

            // Create the button to add a new toggle for the blend shape
            CreateAddToggleButton(_mainPage);
        }

        private static void ToggleBlendShape(string blendShapeName, bool isEnabled)
        {
            if (_skinnedMeshRenderer != null)
            {
                int blendShapeIndex = _skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName);
                if (blendShapeIndex >= 0)
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, isEnabled ? 100f : 0f);
                    MelonLogger.Msg($"Blend Shape '{blendShapeName}' toggled to {(isEnabled ? "enabled" : "disabled")}.");
                }
                else
                {
                    MelonLogger.Error($"Blend Shape '{blendShapeName}' not found.");
                }
            }
            else
            {
                MelonLogger.Error("Cannot toggle Blend Shape because SkinnedMeshRenderer is null.");
            }
        }
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
