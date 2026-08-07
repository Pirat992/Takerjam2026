using System.Collections;
using Game.Abstraction;
using UnityEngine;

namespace Game.SpatialHole
{
    public class SpatialHoleRoom : MonoView
    {
        [SerializeField] private SpatialHole spatialHole;
        [SerializeField] private float timeLife;
        [SerializeField] private float rotationDuration = 10f;

        private SpatialHole output;
        
        public void SetHole(SpatialHole spatialHole)
        {
            output =  spatialHole;
            this.spatialHole.Show();
            this.spatialHole.SetPortal(output);
            output.SetPortal(this.spatialHole);
            Show();
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return RotateRoutine();
            yield return new WaitForSeconds(timeLife);
            // Destroy(output.gameObject);
            // Destroy(portal.gameObject);
        }
        
        private IEnumerator RotateRoutine()
        {
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(-90f, 0f, 0f);
            float elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                elapsedTime += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
                yield return null;
            }

            transform.rotation = targetRotation;
        }
    }
}