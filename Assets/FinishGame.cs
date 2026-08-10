using UnityEngine;

namespace Game
{
    public class FinishGame : MonoBehaviour
    {
        [SerializeField] private WindowFinish finish;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                finish.Show();
        }
    }
}