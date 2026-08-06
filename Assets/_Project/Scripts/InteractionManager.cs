using UnityEngine.InputSystem;
using UnityEngine;
using Game.UI;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private InkDialogUI ui;
    [SerializeField] private Camera cam;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private InputActionReference interactAction;

    private Interactable current;
    
    private void Update()
    {
        if (ui.IsOpen)
        {
            SetCurrent(ui, null);
            return;
        }

        Interactable target = FindTarget();
        SetCurrent(ui, target);

        if (target != null && interactAction.action.IsPressed())
            target.Interact();
    }

    private Interactable FindTarget()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            return hit.collider.GetComponentInParent<Interactable>();
        return null;
    }

    private void SetCurrent(InkDialogUI dialog, Interactable target)
    {
        if (current == target)
            return;
        current = target;
        dialog.ShowInteractionPrompt(target != null);
    }
}