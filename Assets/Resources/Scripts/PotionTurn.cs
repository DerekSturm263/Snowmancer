using UnityEngine;
public class PotionTurn : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 180, 0) * Time.deltaTime);
    }
}
