using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManaorHealth : MonoBehaviour
{
    Player p;
    void Start()
    {
        p = GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Mana")
        {
            p.mana += 20;
        }
        if (collision.collider.tag == "Health")
        {
            p.health += 10;
        }
    }
}
