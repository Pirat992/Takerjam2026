using System.Collections;
using UnityEngine;

namespace Game
{
    public class PortalRoom : MonoBehaviour
    {
        [SerializeField] private Portal portal;
        [SerializeField] private float timeLife;
        [SerializeField] private float rotationDuration = 10f;

        private Portal output;
        
        public void SetPortal(Portal portal)
        {
            output =  portal;
            this.portal.Show();
            this.portal.SetPortal(output);
            output.SetPortal(this.portal);
            StartCoroutine(Delay());
        }

        private IEnumerator Delay()
        {
            yield return RotateRoutine();
            yield return new WaitForSeconds(timeLife);
            Destroy(output.gameObject);
            Destroy(gameObject);
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