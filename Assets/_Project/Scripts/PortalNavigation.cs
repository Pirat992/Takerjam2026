using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class PortalNavigation : MonoBehaviour
    {
        [SerializeField] private Portal output;
        [SerializeField] private Transform[] points;
        [SerializeField] private Player player;

        private List<Transform> _points;

        private void Start()
        {
            _points = new(points);

            output.OnCloseEv += swithcTeleport;
            StartCoroutine(Wait());
        }

        private void swithcTeleport()
        {
            if (_points.Count == 0)
            {
                output.OnCloseEv -= swithcTeleport;
                return;
            }

            _points.RemoveAt(0);
            output.transform.parent = _points[0];
            output.transform.localPosition = Vector3.zero;
            output.transform.localScale = new(2, 4, 2);
            output.OpenPortal();
        }

        private IEnumerator Wait()
        {
            yield return new WaitWhile(() => !player.Gun.gameObject.activeSelf);
            output.gameObject.SetActive(true);
            swithcTeleport();
        }
    }
}