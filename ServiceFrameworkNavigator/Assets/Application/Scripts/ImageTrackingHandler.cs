using System;
using System.Collections.Generic;
using RealityCollective.ServiceFramework.Services;
using RealityCollective.Utilities.Logging;
using RealityToolkit.SpatialPersistence;
using RealityToolkit.SpatialPersistence.Interfaces;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A client side component to integrate with the Spatial Persistence service to track images and place assets in the scene.
/// 
/// It is responsible for registering events from the Spatial Persistence service, and submitting images for detection, as well as matching the corresponding image with the desired prefab when detected.
/// The Unified approach with the Spatial Persistence service means that the same operation can be used with ANY PROVIDER that is integrated with the service as a Module.
/// (See the Azure Spatial Anchors module for an example of a cloud-based provider)
/// </summary>
public class ImageTrackingHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectsToDisableOnImageFound;

    [Header("Events")]
    public UnityEvent<string, Color> OnImageTrackingNotification;

    public UnityEvent<bool> OnImageTrackingStarted;

    #region Spatial Persistence wiring
    private ISpatialPersistenceService anchorService;

    private readonly Dictionary<Guid, ImageTrackingAsset> registeredAssetsForTracking = new();

    // Start is called before the first frame update
    void Start()
    {
        if (ServiceManager.Instance.TryGetService(out anchorService))
        {
            anchorService.CreateAnchorSucceeded += SpatialPersistenceSystem_CreateAnchorSucceeded;
            anchorService.CreateAnchorFailed += SpatialPersistenceSystem_CreateAnchorFailed;
            anchorService.SpatialPersistenceStatusMessage += SpatialPersistenceSystem_SpatialPersistenceStatusMessage;
            anchorService.AnchorLocated += SpatialPersistenceSystem_AnchorLocated;
            anchorService.AnchorUpdated += SpatialPersistenceSystem_AnchorUpdated;
            anchorService.SpatialPersistenceError += AnchorService_SpatialPersistenceError;
            // Start the Spatial Persistence Service to begin tracking (by default, it should be set to "Manual Start" if using multiple scenes, e.g. a Base Scene for services)
            anchorService.StartSpatialPersistenceService();

            OnImageTrackingStarted?.Invoke(true);

        }
        else
        {
            OnImageTrackingStarted?.Invoke(false);

            UpdateStatusText("Anchor Service not found.", Color.red);
            this.enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (anchorService != null)
        {
            anchorService.CreateAnchorSucceeded -= SpatialPersistenceSystem_CreateAnchorSucceeded;
            anchorService.CreateAnchorFailed -= SpatialPersistenceSystem_CreateAnchorFailed;
            anchorService.SpatialPersistenceStatusMessage -= SpatialPersistenceSystem_SpatialPersistenceStatusMessage;
            anchorService.AnchorLocated -= SpatialPersistenceSystem_AnchorLocated;
            anchorService.AnchorUpdated -= SpatialPersistenceSystem_AnchorUpdated;
            anchorService.SpatialPersistenceError -= AnchorService_SpatialPersistenceError;
        }
    }

    private void AnchorService_SpatialPersistenceError(string obj)
    {
        UpdateStatusText($"Anchor System Error: {obj}", Color.red);
    }

    private void SpatialPersistenceSystem_AnchorUpdated(Guid trackedImageGuid, GameObject @object)
    { }

    private void SpatialPersistenceSystem_AnchorLocated(Guid trackedImageGuid, GameObject @object)
    {
        // Get Detected Explorer Asset from Guid
        if (registeredAssetsForTracking.TryGetValue(trackedImageGuid, out var asset))
        {
            UpdateStatusText($"Anchor located [{trackedImageGuid}] for asset [{asset.AssetName}]", Color.green);
#if DEBUG
            var debugAnchor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            debugAnchor.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            debugAnchor.transform.SetParent(@object.transform);
#endif
            // Instantiate the asset prefab as a child of the detected anchor
            var placedObject = Instantiate(asset.AssetPrefab);
            placedObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            placedObject.transform.SetParent(@object.transform);
        }
        else
        {
            UpdateStatusText($"Anchor located [{trackedImageGuid}] but no corresponding asset found.", Color.yellow);
        }
    }

    private void SpatialPersistenceSystem_SpatialPersistenceStatusMessage(string spatial_persistence_message)
    {
        var message = $"Anchor System Status: {spatial_persistence_message}";
        UpdateStatusText(message, Color.yellow);
    }

    private void SpatialPersistenceSystem_CreateAnchorFailed()
    {
        var message = $"Anchor creation failed";
        UpdateStatusText(message, Color.red);
    }

    private void SpatialPersistenceSystem_CreateAnchorSucceeded(Guid guid, GameObject @object)
    {
        var message = $"Anchor created [{guid}]";
        UpdateStatusText(message, Color.green);
    }
    #endregion Spatial Persistence wiring

    public void RegisterAssetForTracking(ImageTrackingAsset asset)
    {
        if (anchorService == null)
        {
            UpdateStatusText("Anchor Service not found.", Color.red);
            return;
        }

        if (asset.AssetId == Guid.Empty)
        {
            asset.AssetId = Guid.NewGuid();
        }

        if (registeredAssetsForTracking.ContainsKey(asset.AssetId))
        {
            UpdateStatusText($"Asset [{asset.AssetName}] already registered for tracking.", Color.yellow);
            return;
        }

        if (asset.ImageTrackingTexture == null)
        {
            UpdateStatusText($"Asset [{asset.AssetName}] does not have an Image Tracking Texture.", Color.red);
            return;
        }
        if (asset.ImageTrackingTexture != null)
        {
            SearchForAsset(asset);
        }
    }

    private void SearchForAsset(ImageTrackingAsset asset)
    {
        var searchItem = new SpatialPersistenceAnchorArgs(asset.AssetId, asset.ImageTrackingTexture);
        anchorService.TryFindAnchors(searchItem);
        UpdateStatusText($"Searching for anchor [{asset.AssetName}], registered with Guid [{asset.AssetId}]", Color.yellow);

        registeredAssetsForTracking.Add(asset.AssetId, asset);
    }

    private void UpdateStatusText(string message, Color color)
    {
        StaticLogger.Log(message);
        OnImageTrackingNotification?.Invoke(message, color);
    }
}
