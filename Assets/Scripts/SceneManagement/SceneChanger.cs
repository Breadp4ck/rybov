using System.Threading.Tasks;
using GlobalState.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    namespace SceneManagement
    {
        public static class SceneChanger
        {
            public static async Task ChangeSceneAsync(Constants.SceneType sceneType)
            {
                string sceneName = Constants.GetNextSceneString(sceneType);

                if (sceneName == null)
                {
                    Debug.LogError("Scene can`t be found.");
                    return;
                }

                Scene previousScene = SceneManager.GetActiveScene();

                AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
                while (loadSceneOperation.isDone == false)
                {
                    await Task.Yield();
                }
            }

            public static async Task ReloadCurrentScene()
            { 
                await ChangeSceneAsync(Level.Instance.CurrentSceneType);
            }
        }
    }
}