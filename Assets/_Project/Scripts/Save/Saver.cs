using NaughtyAttributes;
using Game.Preferences;
using Game.ReadOnly;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

namespace Game.Save
{
    public class Saver : MonoBehaviour
    {
        [Header("Preferences Game")]
        [SerializeField, Tooltip("Specify the file extension separated by a period.")] private string nameFile;

        [SerializeField] private PreferencesGameData data;
        [field: SerializeField] public DTO DTO { get; private set; }
        [SerializeField] private int indexSceneToMenu;
        [SerializeField] private string defaultPath;
        [SerializeField] private bool isLoad = false;

        public bool IsLoaded { get; private set; } = false;
        
        private void OnValidate()
        {
            defaultPath = Application.persistentDataPath;
        }

        private void Awake()
        {
            if (isLoad)
            {
                Load();
                return;
            }
            
            if (SceneManager.GetActiveScene().buildIndex != indexSceneToMenu) return;
            
            DTO.SetPreferences(new ()
            {
                sensitivity =  data.Sensitivity,
                resolution = data.Resolution,
                screenMode =  data.FullScreenMode,
                audio =  data.Audio,
            });
        }

        public void Save()
        {
            var str = JsonUtility.ToJson(DTO.PreferencesGame);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, nameFile), str);
        }

        public void Load()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, nameFile)))
                return;
            
            var str = File.ReadAllText(Path.Combine(Application.persistentDataPath, nameFile));
            DTO.SetPreferences(JsonUtility.FromJson<PreferencesGame>(str));
            
            IsLoaded = true;
        }

        [Button("Delete save file")]
        private void Delete()
        {
            if (!File.Exists(Path.Combine(Application.persistentDataPath, nameFile)))
                return;
            
            File.Delete(Path.Combine(Application.persistentDataPath, nameFile));
        }
    }
}