
using RealityCollective.ServiceFramework.Interfaces;
using static ServiceFrameworkNavigator.SceneLoader;

namespace ServiceFrameworkNavigator.Interfaces
{
    public interface ISceneLoader : IService
    {
        /// <summary>
        /// An example Interface method that can be called from other services or scripts.
        /// </summary>
        /// <param name="scene">Uses a constructed Scene enum to control what scenes to load</param>
        void LoadScene(SceneList scene);

        /// <summary>
        /// An example Interface method that can be called from other services or scripts.
        /// </summary>
        /// <param name="scene">Uses a constructed Scene enum to control what scenes to load</param>
        void UnloadScene(SceneList scene);
    }
}