using RealityCollective.ServiceFramework.Services;
using ServiceFrameworkNavigator.Interfaces;
using UnityEngine;
using static ServiceFrameworkNavigator.SceneLoader;

/// <summary>
/// Basic UX integration script to unload the About scene on pressing of a button.
/// </summary>
public class UnloadScene : MonoBehaviour
{
    // Update is called once per frame
    public void UnloadSceneAbout()
    {
        ServiceManager.Instance.GetService<ISceneLoader>().UnloadScene(SceneList.AboutScene);
    }
}
