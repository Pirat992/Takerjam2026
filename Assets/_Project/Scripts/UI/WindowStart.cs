using Game.Abstraction;
using UnityEngine;

namespace Game
{
    public class WindowStart : MonoView
    {
        [SerializeField] private Canvas self;

        private void Start()
        {
            Show();
        }

        public override void Show()
        {
            self.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        public override void Hide()
        {
            self.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
}