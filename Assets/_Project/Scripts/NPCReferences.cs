using UnityEngine;
using Game.NPC;

namespace Game
{
    public class NPCReferences : MonoBehaviour
    {
        [SerializeField] private Human[] npcs;

        public Human GetHuman()
        {
            foreach (var npc in npcs)
            {
                if (npc.IsAlive)
                    return npc;
            }

            return null;
        }
    }
}