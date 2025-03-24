using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private DynamicGridSpawner _grid;
    [SerializeField] private GameObject _slotPrefab; // ItemSlot


    public void Open()
    {
        _panel.SetActive(true);
    }

    public void Close()
    {
        _panel.SetActive(false);
    }

    public void SpawnSlots(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            _grid.SpawnObject(_slotPrefab);
        }
    }
}
