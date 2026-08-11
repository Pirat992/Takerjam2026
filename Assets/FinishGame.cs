using UnityEngine;

namespace Game
{
    public class FinishGame : MonoBehaviour
    {
        [SerializeField] private StoryInkDialogUI dialog;
        [SerializeField] private WindowFinish finish;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
                if (player.Gun.gameObject.activeSelf) 
                    finish.Show();
                else 
                    dialog.gameObject.SetActive(true);
        }
    }
}