using UnityEngine;

namespace AZE.AdvancedFirstPerson
{
    public class CharacterPushInteraction : MonoBehaviour
    {
        [Header("Configurações de Força")]
        [SerializeField] private float pushPower = 2.0f;
        [SerializeField] private float weightBasedPush = 1.0f;
        [SerializeField] private float heightIgnoreObject = -.3f;

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body;

            if ((body = hit.rigidbody) == null || body.isKinematic ||
                body.mass < weightBasedPush || hit.moveDirection.y < heightIgnoreObject)
            {
                return;
            }

            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            body.velocity = pushDir * pushPower;
        }
    }
}