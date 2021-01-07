using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private readonly string filePath = "Materials/Enemy/";
    private readonly string filePath2 = "Prefabs/Enemy Weapons/";
    [HideInInspector] public Transform hand;

    public enum ElementType
    {
        Fire, Ice, Electric, Wind
    }

    public enum AttackType
    {
        Melee, Magic, Summoner
    }

    private Material[] elementMaterials = new Material[8];
    private GameObject[] weapons = new GameObject[8];

    public bool randomize = false;

    [Header("Enemy Stats")]
    public float spawnRange;
    public float maxHealth;
    public float health;
    public ElementType enemyType;
    public AttackType enemyAttackType;
    public float damage;
    public float chargeTime;
    public float attackSize;
    public float intervalBetweenLongRangeAttacks;
    public float magicAttackSpeed;
    public float magicAttackLifeTime;
    public bool moveWhileAttacking;
    public GameObject summonerEnemy;
    public List<GameObject> drops;

    [Space(10)]
    public GameObject spawnParticles;

    private void Awake()
    {
        hand = GetComponentInChildren<Hand>().transform;

        // Get the materials from the Materials folder and assign them spots in the array.
        try
        {
            elementMaterials[0] = Resources.Load<Material>(filePath + "Enemy Red Coat");
            elementMaterials[1] = Resources.Load<Material>(filePath + "Enemy Blue Coat");
            elementMaterials[2] = Resources.Load<Material>(filePath + "Enemy Yellow Coat");
            elementMaterials[3] = Resources.Load<Material>(filePath + "Enemy White Coat");
            elementMaterials[4] = Resources.Load<Material>(filePath + "Enemy Red Fluff");
            elementMaterials[5] = Resources.Load<Material>(filePath + "Enemy Blue Fluff");
            elementMaterials[6] = Resources.Load<Material>(filePath + "Enemy Yellow Fluff");
            elementMaterials[7] = Resources.Load<Material>(filePath + "Enemy White Fluff");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        // Get the materials from the Materials folder and assign them spots in the array.
        try
        {
            weapons[0] = Resources.Load<GameObject>(filePath2 + "Enemy Red Sword");
            weapons[1] = Resources.Load<GameObject>(filePath2 + "Enemy Blue Sword");
            weapons[2] = Resources.Load<GameObject>(filePath2 + "Enemy Yellow Sword");
            weapons[3] = Resources.Load<GameObject>(filePath2 + "Enemy White Sword");
            weapons[4] = Resources.Load<GameObject>(filePath2 + "Enemy Red Wand");
            weapons[5] = Resources.Load<GameObject>(filePath2 + "Enemy Blue Wand");
            weapons[6] = Resources.Load<GameObject>(filePath2 + "Enemy Yellow Wand");
            weapons[7] = Resources.Load<GameObject>(filePath2 + "Enemy White Wand");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        if (randomize) enemyType = (ElementType) Random.Range(0, 4);

        // Go through each object in the enemy GameObject and assign it the correct material if necessary.
        foreach (SkinnedMeshRenderer renderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            try
            {
                renderer.materials.ToList().ForEach(x =>
                {
                    if (x.name.Contains("Fluff"))
                        x.SetColor("_Tint", elementMaterials[(int)enemyType + 4].GetColor("_Tint"));
                    else if (x.name.Contains("Coat"))
                        x.SetColor("_Tint", elementMaterials[(int)enemyType].GetColor("_Tint"));
                });
            } catch { }
        }

        // Instantiate the weapon in the hand of the enemy based on the enemy element type and attack type.
        int weaponNum = -1;

        if (enemyType == ElementType.Fire)
        {
            if (enemyAttackType == AttackType.Melee)
                weaponNum = 0;
            else
                weaponNum = 4;
        }
        else if (enemyType == ElementType.Ice)
        {
            if (enemyAttackType == AttackType.Melee)
                weaponNum = 1;
            else
                weaponNum = 5;
        }
        else if (enemyType == ElementType.Electric)
        {
            if (enemyAttackType == AttackType.Melee)
                weaponNum = 2;
            else
                weaponNum = 6;
        }
        else
        {
            if (enemyAttackType == AttackType.Melee)
                weaponNum = 3;
            else
                weaponNum = 7;
        }

        Debug.Log(hand.name);
        Instantiate(weapons[weaponNum], hand);
    }

    private void OnDestroy()
    {
        foreach (GameObject item in drops)
        {
            Instantiate(item, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }
    }
}
