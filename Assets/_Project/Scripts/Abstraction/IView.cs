using UnityEngine;

namespace Game.Abstraction
{
    public interface IView
    {
        public void Show();
        public void Hide();
    }
    
    public abstract class MonoView : MonoBehaviour, IView
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}