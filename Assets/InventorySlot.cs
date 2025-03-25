using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _amountText;

    private ItemData _itemData;
    private int _amount;

    public ItemData CurrentItem { get { return _itemData; } }
   

    public bool Empty { get { return (_itemData == null); } }


    public void InitSlot(ItemData data)
    {
        _itemData = data;
        _image.enabled = true;
        _image.sprite = _itemData.IMG;
    }

    public ItemData ClearSlot()
    {
        if (_itemData != null)
        {
            _image.sprite = null;
            _image.enabled = false;
            ItemData tmp = _itemData;
            _itemData = null;
            return tmp;
        }
        return null;
    }


}
