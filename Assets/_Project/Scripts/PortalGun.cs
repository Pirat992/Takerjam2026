using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // 1. Добавляем пространство имен нового Input System

public class PortalGun : MonoBehaviour
{
    [Header("Точка и дистанция выстрела")]
    [Tooltip("Трансформ дула/ствола пушки, откуда вылетает луч")]
    [SerializeField] private Transform firePoint;

    [Tooltip("Максимальная дальность полёта луча")]
    [SerializeField] private float shootDistance = 50f;

    [Tooltip("Слои стен, на которых разрешено создавать портал")]
    [SerializeField] private LayerMask shootableLayers;

    [Header("Визуальный эффект луча (Beam FX)")]
    [Tooltip("Префаб луча (с компонентом LineRenderer)")]
    [SerializeField] private LineRenderer beamPrefab;

    [Tooltip("Время отображения луча в секундах")]
    [SerializeField] private float beamDuration = 0.15f;

    [Header("Префаб Портала")]
    [SerializeField] private Portal portalPrefab;
    [SerializeField] private Portal portalOutput;

    private Portal activePortal;

    private void Update()
    {
        // 2. Новая проверка нажатия левой кнопки мыши (ЛКМ)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        Transform launchPoint = firePoint != null ? firePoint : transform;
        Ray ray = new Ray(launchPoint.position, launchPoint.forward);

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, shootDistance, shootableLayers))
        {
            targetPoint = hit.point;

            Vector3 spawnPosition = hit.point + hit.normal * 0.02f;
            Quaternion spawnRotation = Quaternion.LookRotation(hit.normal);

            if (activePortal == null)
            {
                activePortal = Instantiate(portalPrefab, spawnPosition, spawnRotation);
                activePortal.OutputPortal(portalOutput);
                portalOutput.OutputPortal(activePortal);
            }
            else
            {
                activePortal.gameObject.SetActive(false);
                activePortal.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
                activePortal.gameObject.SetActive(true);
            }

            activePortal.OpenPortal();
        }
        else
        {
            targetPoint = launchPoint.position + launchPoint.forward * shootDistance;
        }

        CreateBeamEffect(launchPoint.position, targetPoint);
    }

    private void CreateBeamEffect(Vector3 startPos, Vector3 endPos)
    {
        if (beamPrefab == null) return;

        var beamInstance = Instantiate(beamPrefab);

        beamInstance.transform.GetChild(0).position = startPos;
        
            beamInstance.SetPosition(0, startPos);
            beamInstance.SetPosition(1, endPos);
            Destroy(beamInstance.gameObject, beamDuration);
        

    }

    private void OnDrawGizmosSelected()
    {
        Transform launchPoint = firePoint != null ? firePoint : transform;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(launchPoint.position, launchPoint.forward * shootDistance);
    }
}