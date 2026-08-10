using UnityEngine.SceneManagement;
using Game.Abstraction;
using UnityEngine;

namespace Game
{
    public class WindowFinish : MonoView
    {
        [SerializeField] private Canvas self;

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
        }

        public void Exit()
        {
            Application.Quit();
        }
        
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}