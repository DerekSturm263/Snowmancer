using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StatusVignette : MonoBehaviour
{
    private Vignette camVignette;
    private PlayerMovement player;

    private void Awake()
    {
        GetComponent<Volume>().profile.TryGet(out camVignette);
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        switch (player.statusEffect)
        {
            case Movement.StatusEffect.Burnt:
                camVignette.color.value = Color.Lerp(camVignette.color.value, Color.red, Time.deltaTime * 5f);
                camVignette.smoothness.value = Mathf.Lerp(camVignette.smoothness.value, 1f, Time.deltaTime * 5f);
                break;

            case Movement.StatusEffect.Frozen:
                camVignette.color.value = Color.Lerp(camVignette.color.value, Color.cyan, Time.deltaTime * 5f);
                camVignette.smoothness.value = Mathf.Lerp(camVignette.smoothness.value, 1f, Time.deltaTime * 5f);
                break;

            case Movement.StatusEffect.Shocked:
                camVignette.color.value = Color.Lerp(camVignette.color.value, Color.yellow, Time.deltaTime * 5f);
                camVignette.smoothness.value = Mathf.Lerp(camVignette.smoothness.value, 1f, Time.deltaTime * 5f);
                break;

            default:
                camVignette.color.value = Color.Lerp(camVignette.color.value, Color.black, Time.deltaTime * 5f);
                camVignette.smoothness.value = Mathf.Lerp(camVignette.smoothness.value, 0.25f, Time.deltaTime * 5f);
                break;
        }
    }
}
