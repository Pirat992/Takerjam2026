using Game.Preferences;
using UnityEngine;

namespace Game.ReadOnly
{
    [CreateAssetMenu(menuName = "Game/Config/PreferencesGame")]
    public class PreferencesGameData : ScriptableObject
    {
        [field: SerializeField] public Vector2 Sensitivity { get; private set; } = Vector2.one;
        [field: SerializeField] public FullScreenMode FullScreenMode { get; private set; } = FullScreenMode.ExclusiveFullScreen;
        [field: SerializeField] public AudioGame Audio { get; private set; } = new() { general = .25f, music = 1f, sound = 1f };
        [field: SerializeField] public Vector2Int Resolution { get; private set; } = new(1920, 1080);
    }
}