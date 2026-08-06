using Ink.UnityIntegration;
using UnityEngine;
using Game.UI;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private InkDialogUI ui;
    [SerializeField] private InkFile inkFile;
    [SerializeField] private string startKnot;

    public void Interact()
    {
        if (inkFile == null)
            return;

        ui.StartStory(inkFile, startKnot);
    }

    private void Reset()
    {
        SetTrigger();
    }

    private void OnValidate()
    {
        SetTrigger();
    }

    private void SetTrigger()
    {
        if (TryGetComponent(out Collider col))
            col.isTrigger = true;
    }
}