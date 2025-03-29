using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private TrainController _trainController;

    [Header("Настройки спавна (ScriptableObject)")]
    [SerializeField] private StationSpawnSettings _stationSpawnSettings;
    [SerializeField] private IslandsSpawnSettings _islandsSpawnSettings;
    [SerializeField] private MeteorsSpawnSettings _meteorsSpawnSettings;

    private List<GameObject> _spawnedWorldObjects = new List<GameObject>();
    private float _nextSpawnDistance = 0f;
    private int _stationIndex = 0;

    // Флаг, сигнализирующий, что для текущего порога уже спавнился сегмент
    private bool _segmentSpawnedForThreshold = false;

    void Start()
    {
        if (_trainController == null)
        {
            _trainController = FindObjectOfType<TrainController>();
        }
        if (_trainController == null)
        {
            Debug.LogError("TrainController не найден на сцене!");
            return;
        }

        // Спавним первую станцию вдоль оси X
        SpawnStation(_nextSpawnDistance);
        _nextSpawnDistance += _stationSpawnSettings.StationLength;

        // Заполняем мир вперед до заданного расстояния
        while (_nextSpawnDistance < _trainController.TotalDistanceTraveled + _stationSpawnSettings.SpawnDistanceAhead)
        {
            SpawnNextSegment();
        }
    }

    void Update()
    {
        // Если условие спавна выполнено и для него еще не был заспавнен сегмент,
        // то спавним следующий сегмент и ставим флаг.
        if (!_segmentSpawnedForThreshold &&
            _trainController.TotalDistanceTraveled + _stationSpawnSettings.SpawnDistanceAhead >= _nextSpawnDistance)
        {
            SpawnNextSegment();
            _segmentSpawnedForThreshold = true;
        }
        // Если условие уже не выполняется, сбрасываем флаг.
        else if (_trainController.TotalDistanceTraveled + _stationSpawnSettings.SpawnDistanceAhead < _nextSpawnDistance)
        {
            _segmentSpawnedForThreshold = false;
        }

        // Удаляем объекты, которые находятся позади поезда (по оси X)
        for (int i = _spawnedWorldObjects.Count - 1; i >= 0; i--)
        {
            if (_spawnedWorldObjects[i].transform.position.x < _trainController.TotalDistanceTraveled - _stationSpawnSettings.DespawnDistanceBehind)
            {
                Destroy(_spawnedWorldObjects[i]);
                _spawnedWorldObjects.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Генерирует следующий сегмент мира: станция, острова и метеоры,
    /// затем увеличивает _nextSpawnDistance.
    /// </summary>
    private void SpawnNextSegment()
    {
        // Спавним станцию, если шанс позволяет
        if (Random.value <= _stationSpawnSettings.SpawnChance)
        {
            SpawnStation(_nextSpawnDistance);
        }

        // Спавним острова согласно логике из IslandsSpawnSettings
        SpawnIslandsBetweenStations(_nextSpawnDistance - _stationSpawnSettings.StationLength, _nextSpawnDistance);

        // Спавним метеоры, если шанс позволяет
        if (Random.value <= _meteorsSpawnSettings.SpawnChance)
        {
            SpawnMeteors(_nextSpawnDistance - _stationSpawnSettings.StationLength, _nextSpawnDistance);
        }

        _nextSpawnDistance += _stationSpawnSettings.StationLength;
    }

    /// <summary>
    /// Создаёт станцию вдоль оси X и передаёт ей порядковый номер.
    /// </summary>
    private void SpawnStation(float distance)
    {
        Vector3 spawnPos = new Vector3(distance, 0f, 0f);
        GameObject station = Instantiate(_stationSpawnSettings.StationPrefab, spawnPos, Quaternion.identity);
        _spawnedWorldObjects.Add(station);

        StationGenerator generator = station.GetComponent<StationGenerator>();
        if (generator != null)
        {
            generator.InitializeStation(_stationIndex);
        }
        _stationIndex++;
    }

    /// <summary>
    /// Создаёт острова в сегменте между станциями.
    /// Острова равномерно распределяются вдоль оси X, а смещение по оси Z берётся из SpawnOffsetRange.
    /// </summary>
    private void SpawnIslandsBetweenStations(float startDistance, float endDistance)
    {
        List<GameObject> islandPrefabs = _islandsSpawnSettings.GetIslandPrefabsForSegment();
        int totalCount = islandPrefabs.Count;
        float segmentLength = endDistance - startDistance;

        // Равномерное распределение: делим сегмент на (totalCount + 1) частей
        for (int i = 0; i < totalCount; i++)
        {
            float x = startDistance + segmentLength * (i + 1) / (totalCount + 1);
            float z = Random.Range(_islandsSpawnSettings.SpawnOffsetRange.x, _islandsSpawnSettings.SpawnOffsetRange.y);
            Vector3 islandPos = new Vector3(x, 0f, z);
            GameObject island = Instantiate(islandPrefabs[i], islandPos, Quaternion.identity);
            _spawnedWorldObjects.Add(island);
        }
    }

    /// <summary>
    /// Создаёт метеоры в заданном сегменте.
    /// Использует MeteorPrefab из _meteorsSpawnSettings.
    /// </summary>
    private void SpawnMeteors(float startDistance, float endDistance)
    {
        int meteorCount = Random.Range(_meteorsSpawnSettings.MinMeteorCount, _meteorsSpawnSettings.MaxMeteorCount + 1);
        for (int i = 0; i < meteorCount; i++)
        {
            float x = Random.Range(startDistance, endDistance);
            float z = Random.Range(_meteorsSpawnSettings.SpawnOffsetRange.x, _meteorsSpawnSettings.SpawnOffsetRange.y);
            float y = Random.Range(_meteorsSpawnSettings.YRange.x, _meteorsSpawnSettings.YRange.y);
            Vector3 meteorPos = new Vector3(x, y, z);
            GameObject meteor = Instantiate(_meteorsSpawnSettings.MeteorPrefab, meteorPos, Quaternion.identity);
            _spawnedWorldObjects.Add(meteor);
        }
    }
}
