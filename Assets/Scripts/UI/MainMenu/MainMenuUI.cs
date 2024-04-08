using SceneManagement.SceneManagement;
using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public async void OnPlayButtonClicked()
        {
            await SceneChanger.ChangeSceneAsync(Constants.SceneType.Level0);
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}