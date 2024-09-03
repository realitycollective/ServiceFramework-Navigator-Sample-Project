
using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine;

namespace ServiceFrameworkNavigator
{
    [CreateAssetMenu(menuName = "ApplicationSettingsProfile", fileName = "ApplicationSettingsProfile", order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class ApplicationSettingsProfile : BaseServiceProfile<IServiceModule>
    {
        [Header("System Settings")]
        public bool DebugEnabled;
        public bool OfflineMode;
        public string configuration;
        public string appID;

        [Header("Asset Configuration")]
        public ImageTrackingAsset[] TrackingAssets;
    }
}
