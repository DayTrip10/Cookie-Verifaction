using UnityEngine;
using MelonLoader;
using BoneLib.BoneMenu;
using System;
using System.Collections.Generic;

namespace Expressions.BoneMenu
{
    public static partial class BoneMenuCreator
    {
        private static Page _mainPage = null;
        private static SkinnedMeshRenderer _skinnedMeshRenderer;
        private static string _blendShapeName = "";
        private static Dictionary<string, float> _blendShapeToggles = new Dictionary<string, float>();

        #region MENU CATEGORIES

        public static void CreateStringInput(Page page, string name, Action<string> onValueChanged)
        {
            var element = page.CreateString(name, Color.white, _blendShapeName, (v) =>
            {
                _blendShapeName = v;
                onValueChanged?.Invoke(v);
            });
        }

        public static void CreateBlendShapeSlider(Page page, string blendShapeName)
        {
            if (_skinnedMeshRenderer == null)
            {
                LogError("SkinnedMeshRenderer is not assigned.");
                return;
            }

            if (!_blendShapeToggles.ContainsKey(blendShapeName))
            {
                _blendShapeToggles[blendShapeName] = 0f; // Initialize the blend shape to 0%
            }

            var element = page.CreateFloat(blendShapeName, Color.white, _blendShapeToggles[blendShapeName], 0f, 100f, 1f, (v) =>
            {
                _blendShapeToggles[blendShapeName] = v;
                SetBlendShapeWeight(blendShapeName, v);
            });
        }

        public static void CreateAddBlendShapeButton(Page page)
        {
            page.CreateFunction("Add BlendShape Toggle", Color.green, () =>
            {
                if (!string.IsNullOrEmpty(_blendShapeName))
                {
                    CreateBlendShapeSlider(_mainPage, _blendShapeName);
                }
                else
                {
                    LogMessage("Blend shape name is empty.");
                }
            });
        }

        public static void CreateRefreshButton(Page page)
        {
            page.CreateFunction("Refresh Toggles", Color.yellow, () =>
            {
                RefreshBlendShapeToggles(_mainPage);
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

            // Try to find the SkinnedMeshRenderer
            _skinnedMeshRenderer = GameObject.Find("YourSkinnedMeshRendererObject")?.GetComponent<SkinnedMeshRenderer>();
            if (_skinnedMeshRenderer == null)
            {
                LogError("SkinnedMeshRenderer not found. Please check the GameObject name.");
            }
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

            // Create the input field for the blend shape name
            CreateStringInput(_mainPage, "BlendShape Name", (v) =>
            {
                _blendShapeName = v;
            });

            // Create the button to add a new blend shape slider for the blend shape
            CreateAddBlendShapeButton(_mainPage);

            // Create the refresh button to reload the blend shape toggles
            CreateRefreshButton(_mainPage);

            // Populate the existing blend shape toggles
            RefreshBlendShapeToggles(_mainPage);
        }

        private static void RefreshBlendShapeToggles(Page page)
        {
            // Clear existing blend shape toggles
            foreach (var blendShapeName in _blendShapeToggles.Keys)
            {
                CreateBlendShapeSlider(page, blendShapeName);
            }
        }

        private static void SetBlendShapeWeight(string blendShapeName, float weight)
        {
            if (_skinnedMeshRenderer != null)
            {
                int blendShapeIndex = _skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(blendShapeName);
                if (blendShapeIndex >= 0)
                {
                    _skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, weight);
                    LogMessage($"Blend Shape '{blendShapeName}' set to {weight}%.");
                }
                else
                {
                    LogError($"Blend Shape '{blendShapeName}' not found.");
                }
            }
            else
            {
                LogError("Cannot set Blend Shape weight because SkinnedMeshRenderer is null.");
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
