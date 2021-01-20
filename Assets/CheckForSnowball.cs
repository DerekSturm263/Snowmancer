using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForSnowball : MonoBehaviour
{
    public BossBehavior bossAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Snowball"))
            bossAI.SwatSnowball();
    }
}
