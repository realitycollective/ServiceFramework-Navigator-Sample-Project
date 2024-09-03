using System.Collections;
using RealityCollective.ServiceFramework.Services;
using RealityCollective.Utilities.Extensions;
using RealityCollective.Utilities.Logging;
using ServiceFrameworkNavigator.Interfaces;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using static ServiceFrameworkNavigator.SceneLoader;

/// <summary>
/// Application Manager is the main entry point for the application.
/// It is responsible for setting up any workflows, services, and managers that are required for the application to run.
/// Its configuration is derived from the ApplicationSettingsService, so that it can be easily configured without changing the code.
/// 
/// It utilizes the ImageTrackingHandler to register assets for tracking with the AR Foundation Image Tracking system using the Reality Collective Spatial Persistence service.
/// </summary>
[RequireComponent(typeof(ImageTrackingHandler))]
public class ApplicationManager : MonoBehaviour
{
    #region Services
    IApplicationSettings applicationSettingsService;
    protected IApplicationSettings ApplicationSettingsService
        => applicationSettingsService ??= ServiceManager.Instance?.GetService<IApplicationSettings>();

    ISceneLoader sceneLoaderService;
    protected ISceneLoader SceneLoaderService
        => sceneLoaderService ??= ServiceManager.Instance?.GetService<ISceneLoader>();
    #endregion Services

    #region Private Properties
    private ImageTrackingHandler imageTrackingHandler;
    #endregion Private Properties


    // Start is called before the first frame update
    void Start()
    {
        // Enable debug if configured
        if (ApplicationSettingsService.DebugEnabled)
        {
            StaticLogger.DebugMode = true;
        }

        // Get the Image Tracking Handler
        imageTrackingHandler = GetComponent<ImageTrackingHandler>();
        if (imageTrackingHandler.IsNull())
        {
            StaticLogger.LogError("Image Tracking Handler not found on Application Manager");
            return;
        }

        // Ensure the device stays awake while exploring
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        imageTrackingHandler.OnImageTrackingStarted.AddListener((isStarted) =>
        {
            if (isStarted)
            {
                StaticLogger.Log("Image Tracking started");
                StartCoroutine(RegisterAssetsWithImageTracking());
            }
            else  // Image Tracking failed to start
            {
                StaticLogger.LogError("Image Tracking failed to start");
            }
        });
    }

    public void OpenAboutScene()
    {
        SceneLoaderService.LoadScene(SceneList.AboutScene);
    }

    private IEnumerator RegisterAssetsWithImageTracking()
    {
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            //#if DEBUG
            Debug.LogWarningFormat("ARSession is not ready yet: {0}", ARSession.state);
            //#endif
            yield return false;
        }

        // Register assets for tracking that have Tracked Image Configuration
        var assets = ApplicationSettingsService.TrackingAssets;
        foreach (var trackingAsset in assets)
        {
            if (trackingAsset.ImageTrackingTexture.IsNotNull())
            {
                StaticLogger.Log($"Registering asset {trackingAsset.AssetName} for tracking");
                imageTrackingHandler.RegisterAssetForTracking(trackingAsset);
                continue;
            }
            else
            {
                StaticLogger.LogWarning($"Asset {trackingAsset.AssetName} does not have a Tracked Image Configuration, Skipping...");
            }
        }
        yield return true;
    }    
}
