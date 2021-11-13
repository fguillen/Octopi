using UnityEngine;

public class AutoDestroyOnWordLimitsController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("WordLimit"))
        {
            Destroy(this.gameObject);
        }
    }
}
