using UnityEngine;

namespace Game.ReadOnly
{
    [CreateAssetMenu(menuName = "Game/Config/Human")]
    public class HumanData : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; } = 5f;
        [SerializeField] private Vector2 randSpeed;
        [field: SerializeField] public float Health { get; private set; } = 100f;
        [SerializeField] private Vector2 randHealth;
        
        public float GetHealth()
        {
            return Random.Range(randHealth.x, randHealth.y);
        }

        public float GetSpeed()
        {
            return Random.Range(randSpeed.x, randSpeed.y);
        }
    }
}