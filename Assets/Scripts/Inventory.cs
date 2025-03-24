using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private int _capacity = 20;


    private static Inventory _instance;
    public static Inventory Instance { get { return _instance; } }

    private List<StorableItem> _items;


    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        _items = new();
        _inventoryUI.SpawnSlots(_capacity);
    }

    public bool AddItem(StorableItem item)
    {
        if (_capacity <= _items.Count)
        {
            return false;
        }

        _items.Add(item);
        //_inventoryUI.AddItem();
        return true;
    }

}
