using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockBall : MonoBehaviour
{
    public float uptime;
    float timeUp = 1f;
    private void Start()
    {
        timeUp = uptime;
    }
    private void Update()
    {
        timeUp -= Time.deltaTime;
        if (timeUp <= 0)
        {
            Destroy(this.gameObject);
        }

    }
}
