using UnityEngine;
using MelonLoader;
using System;
using BoneLib.BoneMenu;

namespace Blower.BoneMenu
{
    public interface IBlowerPref<T>
    {
        T GetValue();
        void SetValue(T value);
        event Action<T> OnValueChanged;
    }

    public class BlowerBoolPref : IBlowerPref<bool>
    {
        private bool _value;
        public event Action<bool> OnValueChanged;
        private readonly MelonPreferences_Entry<bool> _preferenceEntry;

        public BlowerBoolPref(string category, string key, bool defaultValue)
        {
            var preferencesCategory = MelonPreferences.CreateCategory(category);
            _preferenceEntry = preferencesCategory.CreateEntry(key, defaultValue);
            _value = _preferenceEntry.Value;
        }

        public bool GetValue()
        {
            return _value;
        }

        public void SetValue(bool value)
        {
            if (_value != value)
            {
                _value = value;
                _preferenceEntry.Value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public static partial class BoneMenuCreator
    {
        private static Page _mainPage = null;
        private static BlowerBoolPref _modEnabledPref;

        public static void RemoveEmptyPage(Page parent, Page child, Element link)
        {
            if (child.Elements.Count <= 0)
            {
                parent.Remove(link);
            }
        }

        #region MENU CATEGORIES
        public static void CreateBytePreference(Page page, string name, byte increment, byte minValue, byte maxValue, IBlowerPref<byte> pref)
        {
            var element = page.CreateInt(name, Color.white, increment, pref.GetValue(), minValue, maxValue, (v) =>
            {
                pref.SetValue((byte)v);
            });

            pref.OnValueChanged += (v) =>
            {
                element.Value = v;
            };
        }

        public static void CreateFloatPreference(Page page, string name, float increment, float minValue, float maxValue, IBlowerPref<float> pref)
        {
            var element = page.CreateFloat(name, Color.white, increment, pref.GetValue(), minValue, maxValue, (v) =>
            {
                pref.SetValue(v);
            });

            pref.OnValueChanged += (v) =>
            {
                element.Value = v;
            };
        }

        public static void CreateBoolPreference(Page page, string name, IBlowerPref<bool> pref)
        {
            var element = page.CreateBool(name, Color.white, pref.GetValue(), (v) =>
            {
                pref.SetValue(v);
            });

            pref.OnValueChanged += (v) =>
            {
                element.Value = v;
            };
        }

        public static void CreateEnumPreference<TEnum>(Page page, string name, IBlowerPref<TEnum> pref) where TEnum : Enum
        {
            var element = page.CreateEnum(name, Color.white, pref.GetValue(), (v) =>
            {
                pref.SetValue((TEnum)v);
            });

            pref.OnValueChanged += (v) =>
            {
                element.Value = v;
            };
        }

        public static void CreateStringPreference(Page page, string name, IBlowerPref<string> pref, Action<string> onValueChanged = null, int maxLength = 50)
        {
            string currentValue = pref.GetValue();
            var element = page.CreateString(name, Color.white, currentValue, (v) =>
            {
                pref.SetValue(v);
            });

            pref.OnValueChanged += (v) =>
            {
                element.Value = v;

                onValueChanged?.Invoke(v);
            };
        }

        public static void CreateCrashButton(Page page)
        {
            page.CreateFunction("Crash Game for All", Color.red, () =>
            {
                // Deliberately cause a crash by accessing an invalid array index
                int[] arr = new int[1];
                Debug.Log(arr[2]); // This will cause an IndexOutOfRangeException and crash the game
            });
        }
        #endregion

        public static void OnPrepareMainPage()
        {
            _mainPage = Page.Root.CreatePage("Blower", Color.white);
            _modEnabledPref = new BlowerBoolPref("Blower", "ModEnabled", true); // Initialize with default value
        }

        public static void OpenMainPage()
        {
            Menu.OpenPage(_mainPage);
        }

        public static void OnPopulateMainPage()
        {
            // Clear page
            _mainPage.RemoveAll();

            // Create toggle button for enabling/disabling the mod
            CreateBoolPreference(_mainPage, "Enable Mod", _modEnabledPref);

            // Create crash button
            CreateCrashButton(_mainPage);

            // Add other custom menu creation code here
        }

        public static void CreateUniversalMenus(Page page)
        {
            // Add your universal menu creation code here
        }
    }

    public class BlowerMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Everyday Blower mod has started!");
            BoneMenuCreator.OnPrepareMainPage();
            BoneMenuCreator.OpenMainPage();
            BoneMenuCreator.OnPopulateMainPage();
        }

        [Obsolete("OnApplicationStart is obsolete. Use OnInitializeMelon instead.")]
        public override void OnApplicationStart()
        {
            // This method is now obsolete, so it's left empty.
        }
    }
}
