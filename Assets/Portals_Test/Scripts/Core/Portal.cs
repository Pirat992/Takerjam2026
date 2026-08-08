using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class Portal : MonoBehaviour
{
    [Header("Portal Links")]
    public Portal linkedPortal;
    public MeshRenderer screen;
    public Camera portalCamera;

    [Header("Advanced Settings")]
    public int recursionLimit = 5;

    [Header("Animation Settings")] 
    [SerializeField] private float openDuration = 0.5f;
    [SerializeField] private Vector3 targetScale = new Vector3(2f, 4f, 2f);
    [SerializeField] private Ease easeType = Ease.OutBack;

    private Camera mainCamera;
    private RenderTexture viewTexture;
    private List<PortalTraveler> trackedTravelers = new List<PortalTraveler>();



    void Awake()
    {
        mainCamera = Camera.main;
        if (portalCamera != null)
        {
            portalCamera.enabled = false; // Отключаем стандартный авто-рендер камеры
        }
    }

    public void OpenPortal()
    {
        transform.localScale = Vector3.zero; // Сбрасываем размер
        transform.DOScale(targetScale, openDuration)
                 .SetEase(easeType)
                 .SetLink(gameObject);
    }

    public void ClosePortal()
    {
        transform.DOScale(Vector3.zero, openDuration)
                 .SetEase(easeType)
                 .SetLink(gameObject);
    }

    private void OnEnable()
    {
        OpenPortal();
    }

    // --- 1. ВЫЗЫВАЕТСЯ ИЗ MainCamera.cs ПЕРЕД РЕНДЕРОМ ---
    public void PrePortalRender()
    {
        // Скрываем экран портала, чтобы камера портала не рендерила собственный экран
        if (screen != null)
        {
            screen.enabled = false;
        }

        // Обновляем позицию клонов и параметры среза шейдера URP
        for (int i = 0; i < trackedTravelers.Count; i++)
        {
            if (trackedTravelers[i] != null)
            {
                trackedTravelers[i].UpdateSliceAndClone(this, linkedPortal);
            }
        }
    }

    // --- 2. ГЛАВНЫЙ МЕТОД РЕНДЕРА (URP) ---
    public void RenderPortal(ScriptableRenderContext context)
    {
        if (linkedPortal == null || screen == null || portalCamera == null) return;

        // Если портал не виден игроку, пропускаем тяжелый рендер
        if (!IsVisibleFrom(screen, mainCamera)) return;

        CreateRenderTexture();

        // Позиционируем камеру портала относительно выходящего портала (с поворотом на 180°)
        Matrix4x4 m = linkedPortal.transform.localToWorldMatrix
                     * Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0))
                     * transform.worldToLocalMatrix
                     * mainCamera.transform.localToWorldMatrix;

        portalCamera.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        // Устанавливаем наклоненную плоскость отсечения (Oblique Matrix)
        SetObliqueNearClipPlane();

        // Запускаем рендеринг одиночной камеры в URP
        UniversalRenderPipeline.RenderSingleCamera(context, portalCamera);
    }

    // --- 3. ВЫЗЫВАЕТСЯ ИЗ MainCamera.cs ПОСЛЕ РЕНДЕРА ---
    public void PostPortalRender()
    {
        // Возвращаем видимость экрана портала для игрока
        if (screen != null)
        {
            screen.enabled = true;
        }
    }

    private void CreateRenderTexture()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null)
            {
                viewTexture.Release();
            }

            viewTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            portalCamera.targetTexture = viewTexture;

            // Передаем текстуру в материал экрана связанного портала
            if (linkedPortal != null && linkedPortal.screen != null)
            {
                linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
            }
        }
    }

    private void SetObliqueNearClipPlane()
    {
        Transform clipPlane = linkedPortal.transform;
        int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, clipPlane.position - portalCamera.transform.position));

        Vector3 camSpacePos = portalCamera.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
        Vector3 camSpaceNormal = portalCamera.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
        Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, -Vector3.Dot(camSpacePos, camSpaceNormal));

        portalCamera.projectionMatrix = mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
    }

    private bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    // --- ЛОГИКА ТЕЛЕПОРТАЦИИ И ФИЗИКИ ---

    void OnTriggerEnter(Collider other)
    {
        var traveler = other.GetComponent<PortalTraveler>();
        if (traveler && !trackedTravelers.Contains(traveler))
        {
            traveler.previousOffset = traveler.transform.position - transform.position;
            trackedTravelers.Add(traveler);
            linkedPortal.trackedTravelers.Add(traveler);
            traveler.EnterPortalThreshold();
            HandleTravelers();
        }
    }

    void OnTriggerExit(Collider other)
    {
        var traveler = other.GetComponent<PortalTraveler>();
        if (traveler && trackedTravelers.Contains(traveler))
        {
            traveler.ExitPortalThreshold();
            trackedTravelers.Remove(traveler);
        }
        ClosePortal();
    }

    public void OutputPortal(Portal portal)
    {
        linkedPortal = portal;
    }

    private void HandleTravelers()
    {
        for (int i = 0; i < trackedTravelers.Count; i++)
        {
            var traveler = trackedTravelers[i];
            if (traveler == null)
            {
                trackedTravelers.RemoveAt(i);
                i--;
                continue;
            }

            Vector3 offset = traveler.transform.position - transform.position;
            int side = System.Math.Sign(Vector3.Dot(offset, transform.forward));
            int prevSide = System.Math.Sign(Vector3.Dot(traveler.previousOffset, transform.forward));

            // Объект пересек плоскость портала
            
            
                Matrix4x4 m = linkedPortal.transform.localToWorldMatrix
                             * Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0))
                             * transform.worldToLocalMatrix
                             * traveler.transform.localToWorldMatrix;

                Vector3 newPos = m.GetColumn(3);
                Quaternion newRot = m.rotation;

                // Перемещаем объект
                traveler.Teleport(transform, linkedPortal.transform, newPos, newRot);

            trackedTravelers.Clear();
            
        }
    }
}