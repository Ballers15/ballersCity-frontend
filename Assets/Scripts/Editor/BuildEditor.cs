#if UNITY_ANDROID
using System;
using System.Globalization;
using GooglePlayServices;
using UnityEditor;
using UnityEngine;

namespace SinSity.Extensions {
    public static class BuildEditor {

        #region CONSTANTS

        private const string APP_TITLE = "EcoClicker";
        private const string PASSWORD = "123456Qq";
        private const string MAIN_SCENE_PATH = "Assets/Scenes/GameScene.unity";

        #endregion

        public static string GetAndroidBuildPath(string version, bool aabBundle = false) {
            var endWord = aabBundle ? "aab" : "apk";
            var projectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
            var path = $"{projectPath}/Builds/{APP_TITLE} v.{version}.{endWord}";
            return path;
        }

        #region Buttons

        [MenuItem("Builds/Build and Run Development")]
        public static void RunDevelopmentBuildAndroid() {
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.SetScriptingBackend(
                BuildTargetGroup.Android,
                ScriptingImplementation.Mono2x
            );
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            
            PlayServicesResolver.Resolve(() => {

                UpdateVersion();
                const BuildOptions options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
                var version = $"{Application.version}d";
                var filePath = GetAndroidBuildPath(version);
                Build(options, filePath);

            });
        }


        [MenuItem("Builds/Build and Run Release APK")]
        public static void BuildAndRunAPK() {
            BuildAPK(true);
        }

        [MenuItem("Builds/Build Release APK")]
        public static void BuildAPK() {
            BuildAPK(false);
        }

        public static void BuildAPK(bool alsoRunIt) {
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,
                ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            PlayServicesResolver.Resolve(() => {

                UpdateVersion();
                BuildOptions options = alsoRunIt ? BuildOptions.None | BuildOptions.AutoRunPlayer : BuildOptions.None;
                var version = $"{Application.version}";
                var filePath = GetAndroidBuildPath(version);
                Build(options, filePath);

            });
        }


        [MenuItem("Builds/Build Release AAB")]
        public static void BuildAAB() {
            EditorUserBuildSettings.buildAppBundle = true;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,
                ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures =
                AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            PlayerSettings.Android.bundleVersionCode++;


            PlayServicesResolver.Resolve(() => {

                // UpdateVersion();
                BuildOptions options = BuildOptions.None;
                var version = $"{Application.version}";
                var filePath = GetAndroidBuildPath(version, true);
                Build(options, filePath);

            });
        }

        #endregion
        

        private static void UpdateVersion() {
            double version = GetCurrentVersion();
            double newVersion = Math.Round(version + 0.01f, 2);
            SetNewVersion(newVersion);
        }

        private static double GetCurrentVersion() {
            CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            return double.Parse(PlayerSettings.bundleVersion, NumberStyles.Any, ci);
        }

        private static void SetNewVersion(double newVersion) {
            PlayerSettings.bundleVersion =
                newVersion.ToString("0.00", CultureInfo.InvariantCulture);
        }


        private static void Build(BuildOptions buildOptions, string path) {
            PreparePasswords();

            var levels = new[] {
                MAIN_SCENE_PATH
            };

            var message = BuildPipeline.BuildPlayer(
                levels,
                path,
                BuildTarget.Android,
                buildOptions
            );
            Debug.Log($"Android build complete: {message}");
        }

        private static void PreparePasswords() {
            PlayerSettings.runInBackground = false;

            PlayerSettings.keystorePass = PASSWORD;
            PlayerSettings.keyaliasPass = PASSWORD;
        }

    }
}
#endif