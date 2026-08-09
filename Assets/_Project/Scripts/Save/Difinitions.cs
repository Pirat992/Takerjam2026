using UnityEngine;
using System;

namespace Game.Preferences
{
    [Serializable]
    public struct PreferencesGame
    {
        public Vector2 sensitivity;
        public Vector2Int resolution;
        public FullScreenMode screenMode;
        public AudioGame audio;
    }

    [Serializable]
    public struct AudioGame
    {
        public float general;
        public float music;
        public float sound;
    }
}