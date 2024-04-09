using System;
using SceneManagement.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        private void Start()
        {
            SetMusicMixer();
            SetSfxMixer();
        }

        public async void OnPlayButtonClicked()
        {
            await SceneChanger.ChangeSceneAsync(Constants.SceneType.Level0);
        }

        public void OnExitButtonClicked()
        {
            Application.Quit();
        }

        public void SetMusicMixer()
        {
            _audioMixer.SetFloat("MusicVol", _musicSlider.value);
        }

        public void SetSfxMixer()
        {
            _audioMixer.SetFloat("SFXVol", _sfxSlider.value);
        }
    }
}