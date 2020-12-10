using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private readonly string filePath = "Materials/Enemy ";

    public enum ElementType
    {
        Fire, Ice, Electric, Wind
    }

    public enum AttackType
    {
        Melee, Magic
    }

    private Material[] elementMaterials = new Material[8];

    public bool randomize = false;

    [Header("Enemy Stats")]
    public float maxHealth;
    public float health;
    public ElementType enemyType;
    public AttackType enemyAttackType;
    public float damage;
    public float chargeTime;
    public float intervalBetweenLongRangeAttacks;
    public float magicAttackSpeed;
    public List<GameObject> drops;

    private void Awake()
    {
        // Get the materials from the Materials folder and assign them spots in the array.
        elementMaterials[0] = Resources.Load<Material>(filePath + "Red Coat");
        elementMaterials[1] = Resources.Load<Material>(filePath + "Blue Coat");
        elementMaterials[2] = Resources.Load<Material>(filePath + "Yellow Coat");
        elementMaterials[3] = Resources.Load<Material>(filePath + "White Coat");
        elementMaterials[4] = Resources.Load<Material>(filePath + "Red Fluff");
        elementMaterials[5] = Resources.Load<Material>(filePath + "Blue Fluff");
        elementMaterials[6] = Resources.Load<Material>(filePath + "Yellow Fluff");
        elementMaterials[7] = Resources.Load<Material>(filePath + "White Fluff");

        if (randomize) enemyType = (ElementType) Random.Range(0, 4);

        // Go through each object in the enemy GameObject and assign it the correct material if necessary.
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            SkinnedMeshRenderer renderer = t.gameObject.GetComponent<SkinnedMeshRenderer>();
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
    }

    private void OnDestroy()
    {
        foreach (GameObject item in drops)
        {
            Instantiate(item, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }
    }
}
