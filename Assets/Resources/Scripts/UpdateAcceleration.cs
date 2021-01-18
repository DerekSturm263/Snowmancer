using UnityEngine;
using UnityEngine.VFX;

public class UpdateAcceleration : MonoBehaviour
{
    private Cloth cloth;
    private VisualEffect wind;

    private void Awake()
    {
        cloth = GetComponent<Cloth>();
        wind = FindObjectOfType<VisualEffect>();
    }

    private void Update()
    {
        if (wind == null)
            return;

        Vector3 windDir = wind.GetVector3("Wind");

        cloth.externalAcceleration = windDir;
        RenderSettings.skybox.SetFloat("_Speed", windDir.magnitude / 600f);
    }
}
