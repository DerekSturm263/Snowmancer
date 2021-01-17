using UnityEngine;

[ExecuteInEditMode]
public class FindGround : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 9f, 0f);

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);

        if (Physics.Linecast(transform.position + offset, transform.position + offset + Vector3.down * 1000f, out RaycastHit hit))
        {
            transform.position = hit.point + offset;
        }
    }
}
