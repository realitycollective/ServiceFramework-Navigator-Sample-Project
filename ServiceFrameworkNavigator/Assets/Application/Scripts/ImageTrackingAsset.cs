using System;
using UnityEngine;

/// <summary>
/// A simple scriptable object to define configuration and association for an Image Tracking Texture and a Prefab to launch when it is detected.
/// </summary>
[CreateAssetMenu(menuName = "RealityCollective/NavigatorAssets/CreateTrackingAsset", fileName = "NavigatorTrackingAsset")]
public class ImageTrackingAsset : ScriptableObject
{
    public Guid AssetId;
    public string AssetName;
    public Texture2D ImageTrackingTexture;
    public GameObject AssetPrefab;
}
