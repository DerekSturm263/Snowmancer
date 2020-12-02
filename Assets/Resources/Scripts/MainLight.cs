using UnityEngine;

public class MainLight : MonoBehaviour
{
    private void Awake()
    {
        Shader.SetGlobalVector("_MainLightDirection", transform.position);
    }
}
