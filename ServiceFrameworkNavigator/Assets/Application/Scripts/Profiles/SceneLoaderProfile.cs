
using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Interfaces;
using UnityEngine;

namespace ServiceFrameworkNavigator
{
    [CreateAssetMenu(menuName = "SceneLoaderProfile", fileName = "SceneLoaderProfile", order = (int)CreateProfileMenuItemIndices.ServiceConfig)]
    public class SceneLoaderProfile : BaseServiceProfile<IServiceModule>
    {
        /// <summary>
        /// The names of the scenes to load in order.  Only configurable from the editor.
        /// </summary>
        [SerializeField]
        private string[] sceneNames;

        /// <summary>
        /// Public getter for the scene names, read only so that it cannot be altered.
        /// </summary>
        public string[] SceneNames => sceneNames;
     }
}
