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
                string sceneName = Constants.GetNextSceneString(sceneType);

                if (sceneName == null)
                {
                    Debug.LogError("Scene can`t be found.");
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