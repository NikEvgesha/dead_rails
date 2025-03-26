using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed = 15;
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameObject _visualObj;
    public bool Grabbed { get; private set; }

    private Outline _outline;
    private GravityChecker _gravityChecker;
    private Rigidbody _rb;
    private BoxCollider _collider;
    private Transform _itemPoint;
    public bool _inGravitySource;

    

    public ItemData Data { get { return _itemData; } }

    private void OnEnable()
    {
        _outline = GetComponent<Outline>();
        _gravityChecker = GetComponentInChildren<GravityChecker>();
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
        _gravityChecker.GravityChanged += OnGravityChanged;
    }

    private void Start()
    {
        
    }

    private void OnDisable()
    {
        _gravityChecker.GravityChanged -= OnGravityChanged;
    }


    public void OnFocus(bool focus)
    {
        _outline.enabled = focus;
    }

    public void PickUp(Transform point)
    {
        
        Grabbed = true;
        _itemPoint = point;
        _rb.useGravity = false;
        _rb.freezeRotation = true;
    }

    public void Drop()
    {
        Grabbed = false;
        _itemPoint = null;
        _rb.freezeRotation = false;
        if (_inGravitySource)
        {
            _rb.useGravity = true;
        } else
        {
            //_rb.velocity = Vector3.zero;
        }
        
        
    }


    private void FixedUpdate()
    {
        if (_itemPoint != null)
        {

            Vector3 direction = _itemPoint.position - transform.position;
            _rb.velocity = direction * _lerpSpeed;


            /*            Vector3 newPosition = Vector3.Lerp(transform.position, _itemPoint.position, Time.deltaTime * _lerpSpeed);
                        _rb.MovePosition(newPosition);*/
        }
    }


    private void OnGravityChanged(bool inGravitySource)
    {
        _inGravitySource = inGravitySource;
        if (!_inGravitySource) {
            _rb.useGravity = false;
            //_rb.velocity = Vector3.zero;
        } else
        {
            _rb.useGravity = true;
        }
    }


    public void PutToInventory()
    {
        if (Inventory.Instance.AddItem(this))
        {
            //SetVisibility(false);
            gameObject.SetActive(false);
        }
    }

    public void DropOutFromInventory(Transform dropOutPoint)
    {
        gameObject.SetActive(true);
        transform.SetParent(null); // TODO:  Objects Parent
        transform.position = dropOutPoint.position;
    }


    /*    public void SetVisibility(bool visible)
        {
            _visualObj.SetActive(visible);
            _rb.isKinematic = !visible;
            _rb.detectCollisions = visible;
            _collider.enabled = visible;
            _gravityChecker.enabled = visible;
        }*/

}
