using Game.ReadOnly;
using UnityEngine;
using Game.Save;

namespace Game.Other
{
    public class Audio : MonoBehaviour
    {
        [SerializeField] private Saver saver;
        [field: SerializeField] public AudioSource MusicSource { get; private set; }
        [field: SerializeField] public AudioSource SoundSource { get; private set; }
        [field: SerializeField] public StorageAudioClips StorageClips { get; private set; }
        
        private float _generalVolume;
        private float _musicVolume;
        private float _soundVolume;

        private void Start()
        {
            SetGeneraVolume(saver.DTO.PreferencesGame.audio.general);
            SetMusicVolume(saver.DTO.PreferencesGame.audio.music);
            SetSoundVolume(saver.DTO.PreferencesGame.audio.sound);
        }

        public void SetGeneraVolume(float value)
        {
            _generalVolume = value;
            MusicSource.volume = value * _musicVolume;
            SoundSource.volume = value * _soundVolume;
        }

        public void SetMusicVolume(float value)
        {
            _musicVolume = value;
            MusicSource.volume = _generalVolume * _musicVolume;
        }

        public void SetSoundVolume(float value)
        {
            _soundVolume = value;
            SoundSource.volume = _generalVolume * _soundVolume;
        }
    }
}