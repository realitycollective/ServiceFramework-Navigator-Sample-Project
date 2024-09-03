using ServiceFrameworkNavigator.Interfaces;
using RealityCollective.ServiceFramework.Services;
using UnityEngine.SceneManagement;

namespace ServiceFrameworkNavigator
{
    /// <summary>
    /// Scene Loader Service is a simple service that provides access to the scene loading and unloading functionality.
    /// It can be structured to meet any needs for complex scene management, including loading and unloading scenes, and managing scene transitions.
    /// </summary>
    [System.Runtime.InteropServices.Guid("681973ad-d88f-4b94-b8e6-0f5aabba3bb6")]
    public class SceneLoader : BaseServiceWithConstructor, ISceneLoader
    {
        public enum SceneList
        {
            StartScene,
            AboutScene
        }

        /// <summary>
        /// Cached reference to the profile configured for this service when launched.
        /// </summary>
        private SceneLoaderProfile profile;

        public SceneLoader(string name, uint priority, SceneLoaderProfile profile)
            : base(name, priority)
        {
            this.profile = profile;
        }

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
            if (profile.SceneNames.Length > 0)
            {
                SceneManager.LoadScene(profile.SceneNames[0], LoadSceneMode.Additive);
            }
        }

        /// <inheritdoc />
        public void LoadScene(SceneList scene)
        {
            SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
        }

        /// <inheritdoc />
        public void UnloadScene(SceneList scene)
        {
            SceneManager.UnloadSceneAsync(scene.ToString());
        }
        #endregion MonoBehaviour callbacks
    }
}
