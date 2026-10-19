using System;
using System.IO;
using DuelAR.AR;
using DuelAR.Cards;
using DuelAR.UI;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Rendering;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DuelAR.Editor
{
    public static class SprintZeroSetup
    {
        private const string Generated = "Assets/DuelAR/Generated";
        private const string ScenePath = Generated + "/DuelAR.unity";

        [MenuItem("Duel AR/Configure Android")]
        public static void ConfigureAndroid()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;
            VersionControlSettings.mode = "Visible Meta Files";
            PlayerSettings.companyName = "DuelAR";
            PlayerSettings.productName = "Duel AR";
            PlayerSettings.SetApplicationIdentifier(UnityEditor.Build.NamedBuildTarget.Android, "com.duelar.poc");
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.OpenGLES3 });
            var settings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Android);
            if (settings == null || settings.AssignedSettings == null)
                throw new InvalidOperationException("Apri Project Settings > XR Plug-in Management, inizializza le impostazioni Android e ripeti.");
            if (!XRPackageMetadataStore.AssignLoader(settings.AssignedSettings,
                "UnityEngine.XR.ARCore.ARCoreLoader", BuildTargetGroup.Android))
                throw new InvalidOperationException("Impossibile abilitare il loader ARCore. Controlla XR Plug-in Management.");
            settings.InitManagerOnStart = true;
            EditorUtility.SetDirty(settings);
            EditorUtility.SetDirty(settings.AssignedSettings);
            AssetDatabase.SaveAssets();
            Debug.Log("Configurazione Android salvata. Esegui XR Project Validation e passa al target Android.");
        }

        [MenuItem("Duel AR/Create Sprint 0 Scene")]
        public static void CreateScene()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
                throw new InvalidOperationException("La scena esiste già: aprila senza rigenerarla.");
            if (Directory.Exists(Generated) && Directory.GetFiles(Generated, "*", SearchOption.AllDirectories).Length > 0)
                throw new InvalidOperationException("Generated contiene già asset: completa il setup manualmente per preservarli.");
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            Directory.CreateDirectory(Generated);
            AssetDatabase.Refresh();

            var renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
            ResourceReloader.ReloadAllNullIn(renderer, UniversalRenderPipelineAsset.packagePath);
            var background = ScriptableObject.CreateInstance<ARBackgroundRendererFeature>();
            background.name = "AR Background";
            renderer.rendererFeatures.Add(background);
            AssetDatabase.CreateAsset(renderer, Generated + "/MobileRenderer.asset");
            AssetDatabase.AddObjectToAsset(background, renderer);
            var pipeline = UniversalRenderPipelineAsset.Create(renderer);
            pipeline.name = "Duel AR Mobile";
            pipeline.msaaSampleCount = 1;
            pipeline.supportsHDR = false;
            AssetDatabase.CreateAsset(pipeline, Generated + "/MobilePipeline.asset");
            GraphicsSettings.defaultRenderPipeline = pipeline;
            QualitySettings.renderPipeline = pipeline;

            var library = ScriptableObject.CreateInstance<XRReferenceImageLibrary>();
            AssetDatabase.CreateAsset(library, Generated + "/CardImages.asset");
            var catalog = ScriptableObject.CreateInstance<CardCatalog>();
            AssetDatabase.CreateAsset(catalog, Generated + "/CardCatalog.asset");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            new GameObject("AR Session", typeof(ARSession), typeof(ARInputManager), typeof(ARSessionStatus));
            var originObject = new GameObject("XR Origin");
            var origin = originObject.AddComponent<XROrigin>();
            var offset = new GameObject("Camera Offset");
            offset.transform.SetParent(originObject.transform, false);
            origin.CameraFloorOffsetObject = offset;
            var cameraObject = new GameObject("AR Camera", typeof(Camera), typeof(ARCameraManager), typeof(ARCameraBackground));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(offset.transform, false);
            var camera = cameraObject.GetComponent<Camera>();
            camera.nearClipPlane = 0.01f;
            camera.farClipPlane = 20f;
            cameraObject.AddComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
            origin.Camera = camera;
            var manager = originObject.AddComponent<ARTrackedImageManager>();
            manager.referenceLibrary = library;
            manager.requestedMaxNumberOfMovingImages = 1;
            var presenter = originObject.AddComponent<TrackedCardPresenter>();
            var serialized = new SerializedObject(presenter);
            serialized.FindProperty("catalog").objectReferenceValue = catalog;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            var light = new GameObject("Directional Light", typeof(Light)).GetComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50, -30, 0);
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Debug.Log("Scena creata. Aggiungi immagini autorizzate alla library e associa carte/prefab al catalogo. Vedi docs/setup.md.");
        }
    }
}
