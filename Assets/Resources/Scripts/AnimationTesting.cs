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

        InvokeRepeating("SwitchIdle", 2f, 2f);
    }

    private void Update()
    {
        anim.SetBool("LMB", Input.GetMouseButton(0));
        anim.SetBool("CollectingSnow", Input.GetKey(KeyCode.R));
        anim.SetFloat("SnowballSize", size);
        anim.SetFloat("RemainingSnow", Mathf.Clamp(snowCount, 0f, 10f));

        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            anim.SetLayerWeight(1, 1f);
            anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        }
        else
        {
            anim.SetLayerWeight(1, 0f);
        }

        if (Input.GetMouseButtonDown(0)) size = 1f;

        if (Input.GetMouseButton(0) && snowCount > 0f)
        {
            size += Time.deltaTime * scaler;
            snowCount -= Time.deltaTime * scaler;
        }
        if (Input.GetKey(KeyCode.R))
        {
            snowCount += Time.deltaTime * scaler;
        }
    }

    private void SwitchIdle()
    {
        int random = Random.Range(-6, 4);
        anim.SetFloat("IdleAnimation", random);
    }
}
