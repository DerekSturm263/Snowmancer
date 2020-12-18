using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlare : MonoBehaviour
{
    public Material flareMaterial;

    public Texture[] flareTextures = new Texture[16];
    public Texture ringTexture;

    private Transform camTransform;
    public Transform mainLightTransform;

    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

    private void OnGUI()
    {
        float flareStrength = Vector3.Dot(camTransform.forward, mainLightTransform.forward);

        Vector2 sunPosOnScreen = Camera.main.WorldToScreenPoint(mainLightTransform.position);
        Vector2 lookPosFromSun = new Vector2(Screen.width, Screen.height) / 2f;

        Vector2 sunDifference = sunPosOnScreen - lookPosFromSun;

        if (flareStrength < -0.4f)
        {
            Rect ringRect = new Rect(sunPosOnScreen - new Vector2(250, 250), new Vector2(500, 500));
            Graphics.DrawTexture(ringRect, ringTexture, flareMaterial);

            for (int i = 0; i < flareTextures.Length; i++)
            {
                Vector2 flarePos = sunPosOnScreen + (sunDifference * -flareStrength * i / 6f) + new Vector2(sunDifference.x, 0f);
                Rect flareRect = new Rect(flarePos - new Vector2(100, 100), new Vector2(200, 200));

                Debug.Log(sunDifference);
                Graphics.DrawTexture(flareRect, flareTextures[i], flareMaterial);
            }
        }
    }
}
