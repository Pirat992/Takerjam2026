using Random = UnityEngine.Random;
using UnityEngine;
using System;

namespace Game.ReadOnly
{
    public enum SoundType
    {
        AMBIENT,
        FOOTSTEPS,
        FIRE,
        SNOW,
        WIND,
        STORM
    }

    [CreateAssetMenu(menuName = "Game/Config/Storages/AudioStorages")]
    public class StorageAudioClips : ScriptableObject
    {
        [SerializeField] private SoundList[] soundlist;

#if UNITY_EDITOR
        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(SoundType));
            Array.Resize(ref soundlist, names.Length);
            for (int i = 0; i < soundlist.Length; i++)
            {
                soundlist[i].name = names[i];
            }
        }
#endif

        public AudioClip GetRandomClip(SoundType sound)
        {
            if ((int)sound >= soundlist.Length)
                return null;
            var clips = soundlist[(int)sound];
            if (clips.Length == 0)
                return null;

            return clips.Get(Random.Range(0, clips.Length));
        }

        [Serializable]
        public struct SoundList
        {
            [HideInInspector] public string name;
            [SerializeField] private AudioClip[] sounds;

            public int Length => sounds.Length;
            public AudioClip Get(int indexClip) => sounds[indexClip];
        }
    }
}