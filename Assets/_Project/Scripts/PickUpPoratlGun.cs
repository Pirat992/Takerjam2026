using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class PickUpPoratlGun : MonoBehaviour
    {
        [SerializeField] private float distance = 0.5f;
        [SerializeField] private float duration = 1.5f;
        [SerializeField] private Ease easeType = Ease.InOutSine;

        private Tween _tween;

        private void Start()
        {
            _tween = transform.DOLocalMoveY(transform.localPosition.y + distance, duration)
                .SetEase(easeType)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.Gun.gameObject.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}