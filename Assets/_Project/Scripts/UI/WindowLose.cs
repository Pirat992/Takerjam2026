using UnityEngine.SceneManagement;
using Game.Abstraction;
using UnityEngine;

namespace Game
{
    public class WindowLose : MonoView
    {
        [SerializeField] private Transform root;

        public override void Show()
        {
            root.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            root.gameObject.SetActive(false);
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