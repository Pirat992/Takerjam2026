using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Game.Abstraction;
using UnityEngine.UI;
using Game.ReadOnly;
using System.Linq;
using UnityEngine;
using Game.Save;
using System;
using TMPro;

namespace Game.UI
{
    public class WindowSettings : MonoView
    {
        [SerializeField] private PreferencesGameData data;
        [SerializeField] private Saver saver;
        [SerializeField] private Canvas self;
        [SerializeField] private int indexSceneToMenu;
        [Header("Display")] [SerializeField] private TMP_Dropdown screen;
        [SerializeField] private TMP_Dropdown resolution;
        [Header("Audio")] [SerializeField] private Slider generalVolume;
        [SerializeField] private Slider musicVolume;
        [SerializeField] private Slider soundVolume;
        
        private List<Resolution> _resolutions;

        private void Start()
        {
            var options = Enum.GetNames(typeof(FullScreenMode)).ToList();
            screen.AddOptions(options);
            screen.value = (int)data.FullScreenMode;

            options.Clear();
            var index = 0;
            var curentRes = Screen.currentResolution;
            curentRes.width = data.Resolution.x;
            curentRes.height = data.Resolution.y;
            _resolutions = new(Screen.resolutions.Length);

            foreach (var resolution in Screen.resolutions)
            {
                if (options.Count > 0 &&
                    Screen.resolutions[options.Count -1].width == resolution.width)
                    continue;
                options.Add($"{resolution.width} x {resolution.height}");
                _resolutions.Add(resolution);
            }

            resolution.AddOptions(options);

            if (saver.IsLoaded)
            {
                curentRes.width = saver.DTO.PreferencesGame.resolution.x;
                curentRes.height = saver.DTO.PreferencesGame.resolution.y;
                screen.value = (int)saver.DTO.PreferencesGame.screenMode; 
            }

            foreach (var resolution in Screen.resolutions)
            {
                if (curentRes.width == resolution.width && curentRes.height == resolution.height) break;
                index++;
            }

            resolution.value = index;
            Debug.Log($"load resolution {curentRes.width}, {curentRes.height}, {(FullScreenMode)screen.value}");

            generalVolume.value = saver.DTO.PreferencesGame.audio.general;
            musicVolume.value = saver.DTO.PreferencesGame.audio.music;
            soundVolume.value = saver.DTO.PreferencesGame.audio.sound; 

            if (SceneManager.GetActiveScene().buildIndex != indexSceneToMenu) return;
            Screen.SetResolution(curentRes.width, curentRes.height, (FullScreenMode)screen.value);
        }

        public void Apply()
        {
            var resolutionSelected = _resolutions[resolution.value];
            saver.DTO.SetPreferences(new()
            {
                sensitivity = Vector2.one,
                resolution = new(resolutionSelected.width, resolutionSelected.height),
                screenMode = (FullScreenMode)screen.value,
                audio = new()
                {
                    general = generalVolume.value,
                    music = musicVolume.value,
                    sound = soundVolume.value
                }
            });

            saver.Save();

            Screen.SetResolution(saver.DTO.PreferencesGame.resolution.x, saver.DTO.PreferencesGame.resolution.y,
                saver.DTO.PreferencesGame.screenMode);
        }

        public override void Show()
        {
            self.enabled = true;
        }

        public override void Hide()
        {
            self.enabled = false;
        }
    }
}