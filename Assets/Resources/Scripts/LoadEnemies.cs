using System.Collections.Generic;
using UnityEngine;

public class LoadEnemies : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        enemies.ForEach(x =>
        {
            x.SetActive(true);
            x.GetComponent<Animator>().enabled = true;
            x.GetComponent<EnemyMovement>().enabled = true;
        });


        gameObject.SetActive(false);
    }
}
