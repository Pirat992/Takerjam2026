using Game.Abstraction;
using UnityEngine;

namespace Game
{
    public class Portal : MonoView
    {
        [SerializeField] private Portal portal;
        [SerializeField] private Camera camera;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Shader shader;
        [SerializeField] private int size = 2048;
        [SerializeField] private int depth = 24;
        [SerializeField] private bool isTeleport = false;

        public Camera Camera => camera;

        private void Start()
        {
            SetPortal(portal);
        }

        public void SetPortal(Portal portal)
        {
            this.portal = portal;
            this.portal.Camera.targetTexture = new(size, size, depth);
            meshRenderer.materials = new Material[]{(new (shader))};
            meshRenderer.sharedMaterial.mainTexture = portal.Camera.targetTexture;
        }

        private void Update()
        {
            Quaternion difference = transform.rotation * Quaternion.Inverse(portal.transform.rotation * Quaternion.Euler(0,180,0));
            camera.transform.rotation = difference * Camera.main.transform.rotation;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isTeleport || !collision.gameObject.TryGetComponent(out Rigidbody rb)) return;
            
            var magnitude = rb.velocity.magnitude;
            rb.velocity = Vector3.zero;
            rb.transform.position = portal.transform.position;
            rb.transform.localScale = Vector3.one * 5f;
            rb.AddForce(portal.transform.forward * magnitude, ForceMode.Impulse);
        }
    }
}