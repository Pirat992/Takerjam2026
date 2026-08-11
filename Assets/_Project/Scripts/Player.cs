using AZE.AdvancedFirstPerson;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private WindowFinish lose;
        [SerializeField] private PlayerMovementStateMachine movement;
        [SerializeField] private CharacterController controller;
        
        [field: SerializeField] public PortalGun Gun { get; private set; }
        [field: SerializeField] public  float Health { get; private set; } = 100f;

        public void SetDamage(float damage)
        {
            if (Health <= 0f) return;
            
            Health += damage;

            if (Health < 0f)
            {
                controller.enabled = false;
                movement.Animator.SetTrigger("Death");
                lose.Show();
            }
        }
    }
}