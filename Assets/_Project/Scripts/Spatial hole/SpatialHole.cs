using Game.Abstraction;
using UnityEngine;

namespace Game.SpatialHole
{
    public class SpatialHole : MonoView
    {
        [SerializeField] private SpatialHole spatialHole;
        [SerializeField] private Camera camera;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Shader shader;
        [SerializeField] private int size = 2048;
        [SerializeField] private int depth = 24;
        [SerializeField] private bool isTeleport = false;

        public Camera Camera => camera;

        private void Start()
        {
            SetPortal(spatialHole);
        }

        public void SetPortal(SpatialHole spatialHole)
        {
            this.spatialHole = spatialHole;
            this.spatialHole.Camera.targetTexture = new(size, size, depth);
            meshRenderer.materials = new Material[]{(new (shader))};
            meshRenderer.sharedMaterial.mainTexture = spatialHole.Camera.targetTexture;
            Show();
        }

        private void Update()
        {
            Quaternion difference = transform.rotation * Quaternion.Inverse(spatialHole.transform.rotation * Quaternion.Euler(0,180,0));
            camera.transform.rotation = difference * Camera.main.transform.rotation;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!isTeleport || !collision.gameObject.TryGetComponent(out Rigidbody rb)) return;
            
            var magnitude = rb.velocity.magnitude;
            rb.velocity = Vector3.zero;
            rb.transform.position = spatialHole.transform.position;
            rb.transform.localScale = Vector3.one * 5f;
            rb.AddForce(spatialHole.transform.forward * magnitude, ForceMode.Impulse);
        }
    }
}