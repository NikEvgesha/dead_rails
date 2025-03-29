using UnityEngine;
using System.Collections.Generic;

public class StationGenerator : MonoBehaviour
{
    [Header("Настройки генерации зданий")]
    [SerializeField] private Transform[] _buildingSpawnPoints;
    [SerializeField] private GameObject[] _buildingPrefabs;

    [Header("Настройки магазина")]
    [SerializeField] private bool _hasShops = true;
    [SerializeField] private float _shopProbability = 0.3f;

    // Порядковый номер станции (например, 0 - обучающая, последний - финальная, остальные - промежуточные)
    private int _stationIndex = 0;

    /// <summary>
    /// Инициализирует генерацию станции, принимая её порядковый номер.
    /// </summary>
    /// <param name="stationIndex">Номер станции</param>
    public void InitializeStation(int stationIndex)
    {
        _stationIndex = stationIndex;
        GenerateStation();
    }

    /// <summary>
    /// Генерирует здания на станции.
    /// Логика может меняться в зависимости от _stationIndex.
    /// </summary>
    private void GenerateStation()
    {
        // Пример: если это обучающая станция, можно изменить вероятности или типы зданий
        List<Transform> spawnPoints = new List<Transform>(_buildingSpawnPoints);
        ShuffleList(spawnPoints);

        foreach (Transform spawnPoint in spawnPoints)
        {
            int prefabIndex = Random.Range(0, _buildingPrefabs.Length);
            GameObject buildingPrefab = _buildingPrefabs[prefabIndex];

            GameObject building = Instantiate(buildingPrefab, spawnPoint.position, spawnPoint.rotation, transform);

            if (_hasShops)
            {
                BuildingController buildingController = building.GetComponent<BuildingController>();
                if (buildingController != null)
                {
                    // Пример: для обучающей станции (_stationIndex == 0) магазины могут быть отключены
                    bool enableShop = (_stationIndex != 0) && (Random.value < _shopProbability);
                    buildingController.SetShopActive(enableShop);
                }
            }
        }
    }

    /// <summary>
    /// Перемешивает список объектов (алгоритм Fisher-Yates).
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
