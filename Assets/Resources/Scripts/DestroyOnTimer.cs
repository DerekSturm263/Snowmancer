using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    public float delay;

    private void Awake()
    {
        Invoke("Destroy", delay);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
