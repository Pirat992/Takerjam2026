using UnityEngine;

namespace Game.Abstraction
{
    public interface IView
    {
        public void Show();
        public void Hide();
    }
    
    public class MonoView : MonoBehaviour, IView
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}