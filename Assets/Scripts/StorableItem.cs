using UnityEngine;

public class StorableItem : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    public ItemData Data { get { return _itemData; } }


/*    public void PutToInventory()
    {
        if (Inventory.Instance.AddItem(this))
        {
            Destroy(gameObject);
        }
    }*/
}
