using System.Collections.Generic;
using System.Collections;
using Game.ReadOnly;
using Pathfinding;
using UnityEngine;

namespace Game.NPC
{
    public class Human : MonoBehaviour
    {
        [SerializeField] private HumanData data;
        [SerializeField] private AIDestinationSetter destinationSetter;
        [SerializeField] private AIPath aiPath;
        [SerializeField] private Seeker seeker;
        [SerializeField] private Animator animator;
        [SerializeField] private float findRadius = 100f;

        private float _healt;
        private Transform targetGameObject;
        
        public bool IsAlive => aiPath.canMove;

        private void Start()
        {
            _healt = data.GetHealth();
            aiPath.speed = data.GetSpeed();
            StartCoroutine(Loop());
        }

        public void SetMove(bool canMove)
        {
            aiPath.canMove = canMove;
        }

        private IEnumerator Loop()
        {
            while (aiPath.canMove)
            {
                if (destinationSetter.target == null)
                {
                    var target = GetRandomPointOnBakedNavmesh();
                    if (target.HasValue)
                    {
                        targetGameObject ??= new GameObject($"{name} , target point").transform;
                        targetGameObject.position = target.Value;
                        destinationSetter.target = targetGameObject.transform;
                        aiPath.SearchPath();
                        animator.SetTrigger("Run");
                    }
                }
                
                if (aiPath.hasPath && aiPath.reachedDestination && aiPath.reachedEndOfPath)
                    destinationSetter.target = null;

                yield return null;
            }
            
            animator.SetTrigger("Idle");
        }
        
        public Vector3? GetRandomPointOnBakedNavmesh()
        {
            if (AstarPath.active == null) return Vector3.zero;
            
            var walkableNodes = new List<GraphNode>();
            AstarPath.active.data.graphs[0].GetNodes(node => {
                if (node.Walkable)
                    walkableNodes.Add(node);
            });

            if (walkableNodes.Count == 0) return null;
            
            return PathUtilities.GetPointsOnNodes(walkableNodes, 1)[0];
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, findRadius);
        }
    }
}