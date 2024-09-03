using ServiceFrameworkNavigator.Interfaces;
using RealityCollective.ServiceFramework.Services;

namespace ServiceFrameworkNavigator
{
    /// <summary>
    /// Application Settings Service is a simple service that provides access to the application settings.
    /// Settings are stateful across the application and unaffected by scene loading, as well as being accessible from anywhere.
    /// </summary>
    [System.Runtime.InteropServices.Guid("8074500d-c749-47c0-b40a-a16994cbbc19")]
    public class ApplicationSettings : BaseServiceWithConstructor, IApplicationSettings
    {
        private ApplicationSettingsProfile profile;

        public ApplicationSettings(string name, uint priority, ApplicationSettingsProfile profile)
            : base(name, priority)
        {
            this.profile = profile;
        }

        #region Public Properties
        public bool DebugEnabled => profile.DebugEnabled;
        public string Configuration => profile.configuration;
        public string AppID => profile.appID;
        public ImageTrackingAsset[]  TrackingAssets => profile.TrackingAssets;
        #endregion Public Properties

        // Below are the Unity events that are replicated by the Service Framework, simply delete any that are not required.
        #region MonoBehaviour callbacks
        /// <inheritdoc />
        public override void Initialize()
        {
            // Initialize is called when the Service Framework first instantiates the service.  ( during MonoBehaviour 'Awake')
            // This is called AFTER all services have been registered but before the 'Start' call.
        }

        /// <inheritdoc />
        public override void Start()
        {
            // Start is called when the Service Framework receives the "Start" call on loading of the Scene it is attached to.
            // If "Do Not Destroy" is enabled on the Root Service Profile, this is received only once on startup, Else it will occur for each scene load with a Service Framework Instance.
        }

        /// <inheritdoc />
        public override void Reset()
        {
            // Whenever the Service Framework is forcibly "Reset" whilst running, each service will also receive the "Reset" call to request they reinitialize.
        }

        /// <inheritdoc />
        public override void Update()
        {
            // The Unity "Update" MonoBehaviour, this is called when the Service Manager Instance receives the Update Event.
        }

        /// <inheritdoc />
        public override void LateUpdate()
        {
            // The Unity "LateUpdate" MonoBehaviour, this is called when the Service Manager Instance receives the LateUpdate Event.
        }

        /// <inheritdoc />
        public override void FixedUpdate()
        {
            // The Unity "FixedUpdate" MonoBehaviour, this is called when the Service Manager Instance receives the FixedUpdate Event.
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            // The Unity "Destroy" MonoBehaviour, this is called when the Service Manager Instance receives the Destroy Event.
        }

        /// <inheritdoc />
        public override void OnApplicationFocus(bool isFocused)
        {
            // The Unity "OnApplicationFocus" MonoBehaviour, this is called when Unity generates the OnFocus event on App start or resume.
        }

        /// <inheritdoc />
        public override void OnApplicationPause(bool isPaused)
        {
            // The Unity "OnApplicationPause" MonoBehaviour, this is called when Unity generates the OnPause event on App pauses or is about to suspend.
        }
        #endregion MonoBehaviour callbacks
    }
}
