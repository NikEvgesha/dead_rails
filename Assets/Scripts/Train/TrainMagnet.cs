using UnityEngine;

public class TrainMagnet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "GravityPlatform")
        other.transform.SetParent(this.gameObject.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "GravityPlatform")
            other.transform.SetParent(null);
    }
}
