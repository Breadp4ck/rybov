using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    namespace SceneManagement
    {
        public static class SceneChanger
        {
            public static async Task ChangeSceneAsync(Constants.SceneType sceneType, LoadSceneMode loadMode = LoadSceneMode.Single)
            {
                if (Constants.Scenes.TryGetValue(sceneType, out string sceneName) == false)
                {
                    Debug.LogError($"Scene {sceneType} not found. Check Constants.cs");
                    return;
                }

                Scene previousScene = SceneManager.GetActiveScene();

                AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
                while (loadSceneOperation.isDone == false)
                {
                    await Task.Yield();
                }

                if (loadMode == LoadSceneMode.Single)
                {
                    return;
                }
                
                AsyncOperation unloadSceneOperation = SceneManager.UnloadSceneAsync(previousScene);
                while (unloadSceneOperation.isDone == false)
                {
                    await Task.Yield();
                }
            }
        }
    }
}