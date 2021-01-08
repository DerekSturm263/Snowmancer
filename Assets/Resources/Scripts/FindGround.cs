using UnityEngine;

[ExecuteInEditMode]
public class FindGround : MonoBehaviour
{
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);

        if (Physics.Linecast(transform.position + new Vector3(0, 1f, 0), transform.position + new Vector3(0, 1f, 0) + Vector3.down * 100f, out RaycastHit hit))
        {
            transform.position = hit.point + new Vector3(0, 1f, 0);
        }
    }
}
