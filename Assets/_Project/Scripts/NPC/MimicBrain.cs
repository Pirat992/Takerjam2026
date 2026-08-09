using System.Collections;
using UnityEngine;
using Pathfinding; // Обязательно для твоего AIPath

public class MimicBrain : MonoBehaviour
{
    [Header("Настройки")]
    public string npcTag = "NPC";
    public string playerTag = "Player";
    public float eatDistance = 2f;
    public float eatPause = 3f;

    private AIPath aiPath;
    private MimicSpace.Mimic mimicLegs;
    private Transform playerTransform;
    private bool isEating = false;

    void Start()
    {
        // Хватаем твои компоненты со скрина
        aiPath = GetComponent<AIPath>();
        mimicLegs = GetComponent<MimicSpace.Mimic>();

        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p) playerTransform = p.transform;

        // Запускаем мозговой процесс
        StartCoroutine(BrainLoop());
    }

    void Update()
    {
        // Передаем скорость в щупальца, чтобы они красиво перебирались
        if (mimicLegs != null)
        {
            mimicLegs.velocity = isEating ? Vector3.zero : aiPath.velocity;
        }
    }

    // Это цикл, который проверяет обстановку 5 раз в секунду
    IEnumerator BrainLoop()
    {
        while (true)
        {
            if (isEating)
            {
                yield return null;
                continue;
            }

            // Ищем NPC. Если нет - берем игрока
            Transform target = FindNearestNPC();
            if (target == null) target = playerTransform;

            if (target != null)
            {
                // Говорим AIPath, куда бежать
                aiPath.destination = target.position;

                // Проверяем, можно ли сожрать
                if (target.CompareTag(npcTag) && Vector3.Distance(transform.position, target.position) <= eatDistance)
                {
                    yield return StartCoroutine(EatRoutine(target.gameObject));
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator EatRoutine(GameObject npc)
    {
        isEating = true;
        aiPath.canMove = false; // Бьем по тормозам

        Destroy(npc); // Ам-ням

        yield return new WaitForSeconds(eatPause); // Жуем 3 секунды

        aiPath.canMove = true; // Отпускаем тормоза
        isEating = false;
    }

    Transform FindNearestNPC()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var npc in npcs)
        {
            if (npc != null && npc.activeInHierarchy)
            {
                float dist = Vector3.Distance(transform.position, npc.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = npc.transform;
                }
            }
        }
        return nearest;
    }
}