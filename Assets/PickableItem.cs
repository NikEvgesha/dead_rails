using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed = 10;
    public bool Grabbed { get; private set; }

    private GravityChecker _gravityChecker;
    private Rigidbody _rb;
    private Transform _itemPoint;
    private bool _inGravitySource;


    private void Start()
    {
        _gravityChecker = GetComponent<GravityChecker>();
        _gravityChecker.GravityChanged += OnGravityChanged;
    }

    private void OnDisable()
    {
        _gravityChecker.GravityChanged -= OnGravityChanged;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
        }
    }
}
