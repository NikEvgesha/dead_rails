using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SpawnSettings/IslandsSpawnSettings")]
public class IslandsSpawnSettings : ScriptableObject
{
    [Header("Настройки маленьких островов")]
    [Tooltip("Префабы маленьких островов")]
    public List<GameObject> SmallIslandPrefabs;
    [Tooltip("Минимальное количество маленьких островов в сегменте")]
    public int MinSmallIslands = 3;
    [Tooltip("Максимальное количество маленьких островов в сегменте")]
    public int MaxSmallIslands = 4;

    [Header("Настройки средних островов")]
    [Tooltip("Префабы средних островов")]
    public List<GameObject> MediumIslandPrefabs;
    [Tooltip("Количество средних островов в сегменте (здесь всегда 1)")]
    public int MediumIslandCount = 1; // Всегда один средний остров

    [Header("Настройки больших островов")]
    [Tooltip("Префабы больших островов")]
    public List<GameObject> LargeIslandPrefabs;
    [Tooltip("Шанс появления большого острова (0-1)")]
    [Range(0f, 1f)]
    public float LargeIslandChance = 0.5f; // Например, 50%

    [Header("Настройки позиционирования")]
    [Tooltip("Диапазон смещения по оси Z для островов (от центральной линии пути)")]
    public Vector2 SpawnOffsetRange = new Vector2(-50f, 50f);

    /// <summary>
    /// Возвращает список островов для спавна в данном сегменте:
    /// - Всегда 1 средний остров,
    /// - 3–4 маленьких острова,
    /// - и иногда 1 большой остров (с вероятностью LargeIslandChance).
    /// </summary>
    public List<GameObject> GetIslandPrefabsForSegment()
    {
        List<GameObject> islands = new List<GameObject>();

        // Добавляем средний остров (если список не пуст)
        if (MediumIslandPrefabs != null && MediumIslandPrefabs.Count > 0)
        {
            islands.Add(GetRandomPrefab(MediumIslandPrefabs));
        }

        // Добавляем 3-4 маленьких острова
        if (SmallIslandPrefabs != null && SmallIslandPrefabs.Count > 0)
        {
            int smallCount = Random.Range(MinSmallIslands, MaxSmallIslands + 1);
            for (int i = 0; i < smallCount; i++)
            {
                islands.Add(GetRandomPrefab(SmallIslandPrefabs));
            }
        }

        // Иногда добавляем 1 большой остров (с шансом)
        if (LargeIslandPrefabs != null && LargeIslandPrefabs.Count > 0)
        {
            if (Random.value < LargeIslandChance)
            {
                islands.Add(GetRandomPrefab(LargeIslandPrefabs));
            }
        }

        return islands;
    }

    /// <summary>
    /// Возвращает случайный префаб из переданного списка.
    /// </summary>
    private GameObject GetRandomPrefab(List<GameObject> prefabs)
    {
        int index = Random.Range(0, prefabs.Count);
        return prefabs[index];
    }
}
