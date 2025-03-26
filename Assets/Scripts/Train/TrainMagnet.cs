using UnityEngine;

public class TrainMagnet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(this.gameObject.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
