using BepInEx;
using MonkeModViewer.Tools;
using Photon.Pun;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
// You need to add the following dependencies to your project:  
// 1. BepInEx: For the BaseUnityPlugin and plugin framework functionality.  
// 2. Photon.Pun: For Photon networking functionality.  
// 3. TMPro: For TextMeshPro UI components.  
// 4. UnityEngine: For Unity-specific classes and methods.  

// Ensure these dependencies are referenced in your project.  
// If using a Unity project, you can add them via the Unity Package Manager or import the required DLLs.
using TMPro;
#pragma warning disable IDE0051

namespace MonkeModViewer
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public static bool IsEnabled { get; private set; }
        public static bool initialized = false;
        public static AssetBundle bundle;

        private void OnEnable()
        {
            if (!initialized) return;
            IsEnabled = true;
            MonkeModItems.menu.SetActive(IsEnabled);
        }

        private void OnDisable()
        {
            if (!initialized) return;
            IsEnabled = false;
            MonkeModItems.menu.SetActive(IsEnabled);
        }
        private void Awake()
        {
            GorillaTagger.OnPlayerSpawned(OnGameInitBruv);
            Instance = this;
        }

        public void OnGameInitBruv()
        {
            Logging.Init();
            try
            {
                if (initialized) return;
                initialized = true;
                bundle = AssetLoader.LoadAssetBundle(PluginInfo.Path);
                var randomnum = UnityEngine.Random.Range(0, 9999);
                Instantiate(bundle.LoadAsset<GameObject>("MMV")).name = $"MMV ({randomnum})";
                MonkeModItems.menu = GameObject.Find($"MMV ({randomnum})");
                // Excuse the absolute mess here
                MonkeModItems.modList = MonkeModItems.menu.transform.Find("Canvas/Panel/Groups").gameObject;
                MonkeModItems.menu.transform.Find("Canvas/Panel").position = new Vector3(-68.393f, 12.0883f, -84.4548f);
                MonkeModItems.mmvButton = MonkeModItems.modList.transform.Find("Scroll View/Viewport/Content/Button").gameObject;
                MonkeModItems.modDesc = MonkeModItems.menu.transform.Find("Canvas/Panel/Description").gameObject;

                MonkeModItems.modList.SetActive(true);
                MonkeModItems.menu.SetActive(false);

                MonkeModItems.CheckForNull();
            }
            catch (Exception e)
            {
                Logging.Fatal("Failed to load MonkeModViewer asset bundle!");
                Logging.Exception(e);
                return;
            }

            try // Adding buttons and getting assemblies
            {
                foreach (Metadata metadata in AssemblyManager.FindDllsWithSpecificClasses(Paths.PluginPath)) 
                {
                    var button = Instantiate(MonkeModItems.mmvButton, MonkeModItems.mmvButton.transform.parent);
                    Logging.Info(metadata.GUID, metadata.Name, metadata.Version, metadata.Description);
                    button.name = metadata.Name;
                    button.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = metadata.Name;
                }
                Destroy(MonkeModItems.mmvButton);
            }
            catch (Exception e)
            {
                Logging.Fatal("Failed to initialize MonkeModViewer!");
                Logging.Exception(e);
                return;
            }
        }
    }

    public static class MonkeModItems
    {
        public static GameObject menu;
        public static GameObject mmvButton;
        public static GameObject modList;
        public static GameObject modDesc;

        public static void CheckForNull() {
            if (menu == null)
            {
                Logging.Fatal("MonkeModViewer menu is null! Please ensure the asset bundle is loaded correctly.");
            }
            if (mmvButton == null)
            {
                Logging.Fatal("MonkeModViewer button is null! Please ensure the asset bundle is loaded correctly.");
            }
            if (modList == null)
            {
                Logging.Fatal("MonkeModViewer mod list is null! Please ensure the asset bundle is loaded correctly.");
            }
            if (modDesc == null)
            {
                Logging.Fatal("MonkeModViewer mod description is null! Please ensure the asset bundle is loaded correctly.");
            }
        }
    }

    // Basic mod information
    public class PluginInfo
    {
        internal const string
            GUID = "John.MonkeModViewer",
            Name = "MonkeModViewer",
            Path = "MonkeModViewer/Assets/monkemodviewer",
            Version = "1.0";
    }
}
