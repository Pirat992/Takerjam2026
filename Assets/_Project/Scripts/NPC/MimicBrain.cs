using System.Collections;
using UnityEngine;
using Pathfinding;

namespace Game.NPC
{
    public class MimicBrain : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private LayerMask mask;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private bool StoryMode = false;
        [SerializeField] private NPCReferences references;
        [Header("Params")]
        [SerializeField] private float eatDistance = 2f;
        [SerializeField] private float radiusDetect = 3f;
        [SerializeField] private float eatPause = 3f;

        [SerializeField] private AIDestinationSetter destinationSetter;
        [SerializeField] private AIPath aiPath;
        [SerializeField] private MimicSpace.Mimic mimicLegs;
        
        private Human _target;

        private void Start()
        {
            StoryInit();

            StartCoroutine(Loop());
        }

        private void StoryInit()
        {
            if (StoryMode && (_target = references.GetHuman()) != null)
            {
                destinationSetter.target = _target.transform;
                aiPath.SearchPath();
            }
        }

        private IEnumerator Loop()
        {
            while (aiPath.canMove)
            {
                if (_target == null)
                {
                    var colliders = Physics.OverlapSphere(transform.position, radiusDetect, mask);

                    foreach (var collider in colliders)
                    {
                        collider.TryGetComponent(out Human human);
                        if (!human.IsAlive) continue;
                        _target = human;
                        destinationSetter.target = human.transform;
                        aiPath.SearchPath();
                    }
                }

                if (!StoryMode && _target == null)
                {
                    destinationSetter.target = _player.transform;
                    aiPath.SearchPath();
                }

                if (destinationSetter.target != null &&
                    Vector3.Distance(destinationSetter.target.position, transform.position) <= eatDistance)
                {
                    if (!StoryMode && Physics.CheckSphere(transform.position, radiusDetect, playerMask))
                    {
                        _player.TryGetComponent(out CharacterController controller);
                        controller.enabled = false;
                        _player?.SetDamage(-1000f);
                    }
                    else if (_target != null)
                    {
                        _target.SetMove(false);
                        yield return new WaitForSeconds(eatPause);
                        if (_target == null) continue;
                        var posH = _target.transform.position;
                        Destroy(_target.gameObject);
                        _target = null;
                        var clone = Instantiate(this);
                        var pos = clone.transform.position;
                        pos.y = transform.position.y;
                        pos.x = posH.x;
                        pos.z = posH.z;
                        yield return new WaitForSeconds(eatPause);
                        StoryInit();
                    }
                }
                
                mimicLegs.velocity = aiPath.velocity;
                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, eatDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radiusDetect);
        }
    }
}