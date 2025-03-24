using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
public class PlayerItemPickUp : MonoBehaviour
{
    [SerializeField] private Transform _itemJoint;
    [SerializeField] private Transform _cameraObj;
    [SerializeField] private float _pickUpDistance = 2f;
    [SerializeField] private LayerMask _layerMask;

    private PlayerInput _input;
    private PickableItem _raycastHitItem;
    private PickableItem _grabbedItem;
    private ControlUI _controlUI;

    bool _hitted = false;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _controlUI = FindAnyObjectByType<ControlUI>();
    }


    private void Update()
    {
        if (!_grabbedItem)
        {
            CheckRaycast();
        }
        

        if (_input.Interaction)
        {
            if (_grabbedItem != null)
            {
                DropItem();
            } else
            {
                TryPickupObject();
            } 
        }
        
    }


    private void CheckRaycast()
    {
        bool hitted = false;
        if (Physics.Raycast(_cameraObj.position, _cameraObj.forward, out RaycastHit hit, _pickUpDistance, _layerMask))
        {
            if (hit.transform.TryGetComponent(out PickableItem item))
            {
                _raycastHitItem = item;
                hitted = true;
            } 
        } 
        if (!hitted)
        {
            _raycastHitItem = null;
        }
        if (_hitted != hitted)
        {
            _controlUI.ShowPickUpButton(hitted);
            _hitted = hitted;
        }
            
    }

    private void TryPickupObject()
    {

        if (_raycastHitItem != null)
        {
                _grabbedItem = _raycastHitItem;
                _grabbedItem.PickUp(_itemJoint);
        }
    }


    private void DropItem()
    {
        _grabbedItem.Drop();
        _grabbedItem = null;
    }

}
