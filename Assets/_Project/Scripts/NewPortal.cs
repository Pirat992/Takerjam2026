using AZE.AdvancedFirstPerson;
using UnityEngine;

public class NewPortal : MonoBehaviour
{

    public NewPortal targetPortal;

    [HideInInspector]
    public bool cooldown;


    public void Teleport(GameObject gameObject)
    {

        gameObject.transform.position = targetPortal.transform.position;


        Quaternion rotation = gameObject.transform.rotation;

        rotation = Quaternion.Euler(rotation.eulerAngles.x, targetPortal.transform.eulerAngles.y + 180, rotation.eulerAngles.z); ;

        gameObject.transform.rotation = rotation;


        targetPortal.cooldown = true;

    }


    private void OnTriggerEnter(Collider other)
    {

        if (cooldown) { return; }

        if (other.tag != "Player") { return; }

        Teleport(other.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag != "Player") { return; }

        cooldown = false;

    }

}
