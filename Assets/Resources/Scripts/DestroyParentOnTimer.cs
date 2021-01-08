using UnityEngine;

public class DestroyParentOnTimer : MonoBehaviour
{
    public float delay;

    private void Awake()
    {
        Invoke("Delete", delay);
    }

    private void Delete()
    {
        Destroy(transform.parent);
    }
}
