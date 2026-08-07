using UnityEngine;

public class PortalTraveler : MonoBehaviour
{
    [Header("Graphics & Slice Settings")]
    public GameObject graphicsObject;
    public Vector3 previousOffset { get; set; }

    protected GameObject graphicsClone;
    protected Material[] originalMaterials;
    protected Material[] cloneMaterials;

    public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        // 1. Корректная обработка CharacterController в Unity 2022
        CharacterController cc = GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        transform.position = toPortal.position;
        transform.rotation = toPortal.rotation;

        Physics.SyncTransforms(); // Синхронизация физического пространства

        if (cc != null) cc.enabled = true;
        
    }

    // Вызывается, когда объект подходит к порталу (входит в триггер)
    public virtual void EnterPortalThreshold()
    {
        if (graphicsObject == null) return;

        // Создаем графический клон объекта
        if (graphicsClone == null)
        {
            graphicsClone = Instantiate(graphicsObject, graphicsObject.transform.parent);
            graphicsClone.transform.localScale = graphicsObject.transform.localScale;

            // Сохраняем ссылки на материалы для передачи параметров среза (Slice)
            var origRenderers = graphicsObject.GetComponentsInChildren<Renderer>();
            var cloneRenderers = graphicsClone.GetComponentsInChildren<Renderer>();

            originalMaterials = GetMaterials(origRenderers);
            cloneMaterials = GetMaterials(cloneRenderers);
        }
        else
        {
            graphicsClone.SetActive(true);
        }
    }

    // Вызывается, когда объект полностью покидает зону портала
    public virtual void ExitPortalThreshold()
    {
        if (graphicsClone != null)
        {
            graphicsClone.SetActive(false);
        }
        ResetSliceParams();
    }

    // Обновление положения клона и передача параметров среза в HLSL-шейдер URP
    public void UpdateSliceAndClone(Portal portal, Portal targetPortal)
    {
        if (graphicsClone == null || !graphicsClone.activeSelf) return;

        // 1. Позиционируем клон относительно противоположного портала
        Matrix4x4 m = targetPortal.transform.localToWorldMatrix
                     * Matrix4x4.Rotate(Quaternion.Euler(0, 180, 0))
                     * portal.transform.worldToLocalMatrix
                     * graphicsObject.transform.localToWorldMatrix;

        graphicsClone.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        // 2. Передаем плоскости среза в URP Slice Шейдер
        // Для основного объекта — отсекаем часть за входящим порталом
        SetSliceShaderParams(originalMaterials, portal.transform.position, portal.transform.forward);

        // Для клона — отсекаем часть за выходящим порталом
        SetSliceShaderParams(cloneMaterials, targetPortal.transform.position, -targetPortal.transform.forward);
    }

    void SetSliceShaderParams(Material[] materials, Vector3 sliceCenter, Vector3 sliceNormal)
    {
        if (materials == null) return;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i] != null)
            {
                // Имена _SliceCenter и _SliceNormal соответствуют нашему URPSliceShader
                materials[i].SetVector("_SliceCenter", sliceCenter);
                materials[i].SetVector("_SliceNormal", sliceNormal);
            }
        }
    }

    void ResetSliceParams()
    {
        // Выход за пределы среза (убираем отсечение)
        SetSliceShaderParams(originalMaterials, Vector3.zero, Vector3.zero);
        SetSliceShaderParams(cloneMaterials, Vector3.zero, Vector3.zero);
    }

    Material[] GetMaterials(Renderer[] renderers)
    {
        var matList = new System.Collections.Generic.List<Material>();
        foreach (var r in renderers)
        {
            foreach (var m in r.materials)
            {
                matList.Add(m);
            }
        }
        return matList.ToArray();
    }
}