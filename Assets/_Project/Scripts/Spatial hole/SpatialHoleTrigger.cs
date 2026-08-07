using UnityEngine;
using System;

namespace Game.SpatialHole
{
    public class SpatialHoleTrigger : MonoBehaviour
    {
        public event Action<SpatialHoleTrigger> OnTrigger;

        [SerializeField] private LayerMask mask;

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer) == mask.value)
            {
                OnTrigger?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}