using AZE.AdvancedFirstPerson;
using UnityEngine;
using System;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public event Action OnPlayerDeathEv;
        
        [SerializeField] private PlayerMovementStateMachine movement;
        [SerializeField] private CharacterController controller;
        
        [field: SerializeField] public  float Health { get; private set; } = 100f;

        public void SetDamage(float damage)
        {
            if (Health <= 0f) return;
            
            Health += damage;

            if (Health < 0f)
            {
                controller.enabled = false;
                movement.Animator.SetTrigger("Death");
                OnPlayerDeathEv?.Invoke();
            }
        }
    }
}