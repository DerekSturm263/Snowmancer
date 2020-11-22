using UnityEngine;

public class AnimationTesting : MonoBehaviour
{
    private Animator anim;
    private float size = 1f;

    public float scaler = 1f;

    public float snowCount = 0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("LMB", Input.GetMouseButton(0));
        anim.SetBool("CollectingSnow", Input.GetMouseButton(2));
        anim.SetFloat("SnowballSize", size);
        anim.SetFloat("RemainingSnow", Mathf.Clamp(snowCount, 0f, 10f));



        if (Input.GetMouseButtonDown(0)) size = 1f;

        if (Input.GetMouseButton(0) && snowCount > 0f)
        {
            size += Time.deltaTime * scaler;
            snowCount -= Time.deltaTime * scaler;
        }
        if (Input.GetMouseButton(1))
        {
            snowCount += Time.deltaTime * scaler;
        }
    }
}
