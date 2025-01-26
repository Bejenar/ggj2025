using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static System.Environment;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

public static class ProjectSetup
{
    [MenuItem("Tools/Setup/Import .unitypackage")]
    public static void ImportEssentials()
    {
        // Assets.ImportAsset("Odin Inspector and Serializer.unitypackage", "Sirenix/Editor ExtensionsSystem");
        // Assets.ImportAsset("Odin Validator.unitypackage", "Sirenix/Editor ExtensionsUtilities");
        // Assets.ImportAsset("Editor Console Pro.unitypackage", "FlyingWorm/Editor ExtensionsSystem");
        Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/Editor ExtensionsAnimation");

        Assets.SetupSymbols();
    }

    [MenuItem("Tools/Setup/Install Essential Packages")]
    public static void InstallPackages()
    {
        Packages.InstallPackages(new[]
        {
            "com.unity.cinemachine",
            "git+https://github.com/Bejenar/jam-bootstrap.git",
            "git+https://github.com/adammyhre/Unity-Utils.git",
            "git+https://github.com/adammyhre/Unity-Improved-Timers.git",
            "https://github.com/mackysoft/Unity-SerializeReferenceExtensions.git?path=Assets/MackySoft/MackySoft.SerializeReferenceExtensions",
            "git+https://gitlab.hotreload.net/root/hot-reload-releases.git#1.12.12",
            "https://github.com/GlitchEnzo/NuGetForUnity.git?path=/src/NuGetForUnity",
            "git+https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"
        });
    }
    
    [MenuItem("Tools/Setup/Reinstall My Packages")]
    public static void Reinstall()
    {
        Packages.ReinstallMyPackages(new[]
        {
            "git+https://github.com/Bejenar/jam-bootstrap.git"
        });
    }

    [MenuItem("Tools/Setup/Create Folders")]
    public static void CreateFolders()
    {
        Folders.Create("_Project",
            "Source",
            "Data",
            "Editor",
            "Resources/Animation",
            "Resources/Materials",
            "Resources/Music",
            "Resources/SFX",
            "Resources/Prefabs",
            "Resources/Art"
        );
        Refresh();
        Folders.Move("_Project", "Scenes");
        Folders.Move("_Project", "Settings");
        Folders.Delete("TutorialInfo");
        Refresh();

        MoveAsset("Assets/InputSystem_Actions.inputactions",
            "Assets/_Project/Settings/InputSystem_Actions.inputactions");
        DeleteAsset("Assets/Readme.asset");
        Refresh();

        // Optional: Disable Domain Reload
        // EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload;
    }

    static class Assets
    {
        public static void ImportAsset(string asset, string folder)
        {
            string basePath;
            if (OSVersion.Platform is PlatformID.MacOSX or PlatformID.Unix)
            {
                string homeDirectory = GetFolderPath(SpecialFolder.Personal);
                basePath = Combine(homeDirectory, "Library/Unity/Asset Store-5.x");
            }
            else
            {
                string defaultPath = Combine(GetFolderPath(SpecialFolder.ApplicationData), "Unity");
                basePath = Combine(EditorPrefs.GetString("AssetStoreCacheRootPath", defaultPath), "Asset Store-5.x");
            }

            asset = asset.EndsWith(".unitypackage") ? asset : asset + ".unitypackage";

            string fullPath = Combine(basePath, folder, asset);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"The asset package was not found at the path: {fullPath}");
            }

            ImportPackage(fullPath, false);
        }

        public static void SetupSymbols()
        {
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Standalone, "UNITASK_DOTWEEN_SUPPORT");
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.WebGL, "UNITASK_DOTWEEN_SUPPORT");
        }
    }

    static class Packages
    {
        static AddRequest request;
        static Queue<string> packagesToInstall = new Queue<string>();
        static ListRequest listRequest;

        public static async void InstallPackages(string[] packages)
        {
            foreach (var package in packages)
            {
                packagesToInstall.Enqueue(package);
            }

            if (packagesToInstall.Count > 0)
            {
                await ListPackages();
                await StartNextPackageInstallation();
            }
        }

        public static async void ReinstallMyPackages(string[] packages)
        {
            foreach (var package in packages)
            {
                packagesToInstall.Enqueue(package);
            }

            if (packagesToInstall.Count > 0)
            {
                listRequest = null;
                await StartNextPackageInstallation();
            }
        }

        static async Task ListPackages()
        {
            listRequest = Client.List();
            while (!listRequest.IsCompleted) await Task.Delay(10);
        }

        static async Task StartNextPackageInstallation()
        {
            var packageName = packagesToInstall.Dequeue();

            if (listRequest != null)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    bool isInstalled = listRequest.Result.Any(pkg => pkg.packageId.Contains(packageName));
                    if (isInstalled)
                    {
                        Debug.Log($"Package already installed: {packageName}");
                        if (packagesToInstall.Count > 0)
                        {
                            await StartNextPackageInstallation();
                        }

                        return;
                    }
                }
                else if (listRequest.Status >= StatusCode.Failure)
                {
                    Debug.LogError(listRequest.Error.message);
                }
            }

            request = Client.Add(packageName);
            while (!request.IsCompleted) await Task.Delay(10);

            if (request.Status == StatusCode.Success) Debug.Log("Installed: " + request.Result.packageId);
            else if (request.Status >= StatusCode.Failure) Debug.LogError(request.Error.message);

            if (packagesToInstall.Count > 0)
            {
                await Task.Delay(1000);
                await StartNextPackageInstallation();
            }
        }
    }

    static class Folders
    {
        public static void Create(string root, params string[] folders)
        {
            var fullpath = Combine(Application.dataPath, root);
            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            foreach (var folder in folders)
            {
                CreateSubFolders(fullpath, folder);
            }
        }

        static void CreateSubFolders(string rootPath, string folderHierarchy)
        {
            var folders = folderHierarchy.Split('/');
            var currentPath = rootPath;

            foreach (var folder in folders)
            {
                currentPath = Combine(currentPath, folder);
                if (!Directory.Exists(currentPath))
                {
                    Directory.CreateDirectory(currentPath);
                }
            }
        }

        public static void Move(string newParent, string folderName)
        {
            var sourcePath = $"Assets/{folderName}";
            if (IsValidFolder(sourcePath))
            {
                var destinationPath = $"Assets/{newParent}/{folderName}";
                var error = MoveAsset(sourcePath, destinationPath);

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError($"Failed to move {folderName}: {error}");
                }
            }
        }

        public static void Delete(string folderName)
        {
            var pathToDelete = $"Assets/{folderName}";

            if (IsValidFolder(pathToDelete))
            {
                DeleteAsset(pathToDelete);
            }
        }
    }
}