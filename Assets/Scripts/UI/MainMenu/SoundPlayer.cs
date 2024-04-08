using UnityEngine;

namespace UI.MainMenu
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public void PlaySound()
        {
            _audioSource.Play();
        }
    }
}