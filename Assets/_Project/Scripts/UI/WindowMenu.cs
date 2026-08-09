using UnityEngine.SceneManagement;
using Game.Abstraction;
using UnityEngine;
using System;

namespace Game.UI
{
    public class WindowMenu : MonoView
    {
        [SerializeField] private Canvas self;
        [SerializeField] private WindowSettings windowSettings;
        [SerializeField] private CursorLockMode cursorMode;

        private bool menuFlag = false;
        
        private void Start()
        {
            Cursor.lockState = cursorMode;
        }

        public override void Show()
        {
            self.enabled = true;
        }

        public override void Hide()
        {
            self.enabled = false;
        }

        public void GoTo(int indexScene)
        {
            SceneManager.LoadScene(indexScene);
        }

        public void Exit()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuFlag = !menuFlag;
                self.enabled = menuFlag;
                if (!menuFlag)
                    windowSettings.Hide();
                cursorMode = self.enabled ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.lockState = cursorMode;
                Time.timeScale = Convert.ToSingle(!self.enabled);//Временное решение.
            }
        }
    }
}