using Game.Preferences;
using UnityEngine;

namespace Game.ReadOnly
{
    [CreateAssetMenu(menuName = "Game/Config/DTO")]
    public class DTO : ScriptableObject
    {
        [field: SerializeField] public PreferencesGame PreferencesGame { get; private set; }
        
        public void SetPreferences(PreferencesGame preferencesGame) => PreferencesGame = preferencesGame;
    }
}