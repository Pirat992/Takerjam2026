using UnityEngine;
using UnityEngine.Rendering;

public class MainCamera : MonoBehaviour
{
    private Portal[] portals;
    private Camera cam;

    void Awake()
    {
        portals = FindObjectsOfType<Portal>();
        cam = GetComponent<Camera>();
    }

    void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    }

    // В URP это аналог OnPreCull
    void OnBeginCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
    {
        // Выполняем логику порталов ТОЛЬКО когда рендерится сама Main Camera
        if (renderingCamera != cam) return;

        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].PrePortalRender();
        }

        for (int i = 0; i < portals.Length; i++)
        {
            // Передаем контекст URP в метод рендера
            portals[i].RenderPortal(context);
        }

        for (int i = 0; i < portals.Length; i++)
        {
            portals[i].PostPortalRender();
        }
    }
}