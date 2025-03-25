using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int _capacity = 20;
    [SerializeField] private int _available = 10;
    [SerializeField] private int _quickSlotsCapacity = 5;

    private static Inventory _instance;
    public static Inventory Instance { get { return _instance; } }

    private List<PickableItem> _items;


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
        InventoryUI.Instance.SpawnSlots(_capacity, _quickSlotsCapacity);
    }



    public bool AddItem(PickableItem item)
    {
/*        if (item.Data.Stackable)
        {
            if (_items.Contains(item)) 
            {
                    
            }
        }*/

        if (_available <= _items.Count)
        {
            return false;
        }

        _items.Add(item);
        item.transform.SetParent(transform);

        InventoryUI.Instance.AddItem(item.Data);
        Debug.Log(item.Data.Name);
        return true;
    }

}
