using UnityEngine;


namespace Game.Abstraction
{
    public abstract class InteractableObject : MonoBehaviour
    {
        [SerializeField] private float radiusDetect;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Color colorGizmos = Color.yellow;

        private protected Player Player;
        
        public abstract void Execute();

        public void TryExecute()
        {
            if (Player)
                Execute();
        }

        private protected virtual void PlayerEntered() { }
        private protected virtual void PlayerExited() { }

        private void Update()
        {
            var colliders = Physics.OverlapSphere(transform.position, radiusDetect, layerMask);

            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent(out Player player) && Player == null)
                    {
                        Player = player;
                        PlayerEntered();
                        return;
                    }
                }
                return;
            }
            
            PlayerExited();
            Player = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = colorGizmos;
            Gizmos.DrawWireSphere(transform.position, radiusDetect);
        }
    }
}