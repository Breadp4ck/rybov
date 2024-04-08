using UnityEngine;

namespace SoundPlayers
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        protected void PlaySound(AudioClip clip, bool loop = false)
        {
            if (clip == null)
            {
                Debug.LogError("Clip is null.");
                return;
            }
            
            _audioSource.Stop();
            _audioSource.clip = clip;
            _audioSource.loop = loop;
            _audioSource.Play();
        }
    }
}