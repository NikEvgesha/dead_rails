using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private DynamicGridSpawner _mainInventoryGrid;
    [SerializeField] private DynamicGridSpawner _qickPanelGrid;
    [SerializeField] private InventorySlot _slotPrefab; // ItemSlot

    private static InventoryUI _instance;
    public static InventoryUI Instance { get { return _instance; } }

    private List<InventorySlot> _mainSlots = new();
    private List<InventorySlot> _quickSlots = new();

    private int _mainCapacity;
    private int _quickPanelCapacity;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void Open()
    {
        _panel.SetActive(true);
    }

    public void Close()
    {
        _panel.SetActive(false);
    }

    public void SpawnSlots(int mainCapacity, int quickCapacity)
    {
        _mainCapacity = mainCapacity;
        _quickPanelCapacity = quickCapacity;
        for (int i = 0; i < mainCapacity; i++)
        {
            InventorySlot slot = _mainInventoryGrid.SpawnObject<InventorySlot>(_slotPrefab.gameObject);
            _mainSlots.Add(slot);
        }

        for (int i = 0; i < quickCapacity; i++)
        {
            InventorySlot slot = _qickPanelGrid.SpawnObject<InventorySlot>(_slotPrefab.gameObject);
            _quickSlots.Add(slot);
        }
    }


    public void AddItem(ItemData itemData)
    {
        bool added = false;

        for (int i = 0; i < _quickPanelCapacity; i++)
        {
            if (_quickSlots[i].Empty)
            {
                _quickSlots[i].InitSlot(itemData);
                added = true;
                break;
            }
        }

        if (added) return;

        for (int i = 0; i < _mainCapacity; i++)
        {
            if (_mainSlots[i].Empty)
            {
                _mainSlots[i].InitSlot(itemData);
                added = true;
                break;
            }
        }

    }


}
