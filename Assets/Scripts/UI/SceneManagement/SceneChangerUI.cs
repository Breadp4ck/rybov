using System;
using SceneManagement.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.SceneManagement
{
    namespace UI.SceneManagement
    {
        public class SceneChangerUI : MonoBehaviour
        {
            // TODO: DEBUG ONLY
            private async void Update()
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    await SceneChanger.ChangeSceneAsync(Constants.SceneType.Level, LoadSceneMode.Additive);
                }
            }
        }
    }
}