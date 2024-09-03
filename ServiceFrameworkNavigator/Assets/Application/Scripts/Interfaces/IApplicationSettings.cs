
using RealityCollective.ServiceFramework.Interfaces;

namespace ServiceFrameworkNavigator.Interfaces
{
    public interface IApplicationSettings : IService
    {
        bool DebugEnabled{ get; }
        
        /// <summary>
        /// JSON configuration string for the application.
        /// </summary>
        string Configuration { get; }
        
        /// <summary>
        /// The AppID for the application.
        /// </summary>
        string AppID { get; }

        /// <summary>
        /// The configured tracking assets for the application.
        /// </summary>
        ImageTrackingAsset[] TrackingAssets { get; }
    }
}