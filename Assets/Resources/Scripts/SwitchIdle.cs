using UnityEngine;
using System.Collections.Generic;

public class SwitchIdle : MonoBehaviour // Really sorry for calling it SwitchIdle despite the fact that it also controls the footprints.
{
    private Animator anim;
    public Mesh footprintMesh;
    public Material footprintMaterial;

    public List<GameObject> footprints;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SwitchIdlePose(string s)
    {
        anim.SetFloat("Idle Animation", Random.Range(-5, 4));
    }

    public void SetFootPrint(int footNum)
    {
        GameObject footPrint = new GameObject("Footprint " + footNum);
        MeshFilter filter = footPrint.AddComponent<MeshFilter>();
        MeshRenderer renderer = footPrint.AddComponent<MeshRenderer>();
        footprints.Add(footPrint);

        filter.mesh = footprintMesh;
        renderer.material = footprintMaterial;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        switch (footNum)
        {
            case 0:
                footPrint.transform.position = IKController.lFootGoal.position;
                footPrint.transform.forward = -IKController.lFootGoal.up;
                break;
            case 1:
                footPrint.transform.position = IKController.rFootGoal.position;
                footPrint.transform.forward = -IKController.rFootGoal.up;
                break;
        }
    }

    private void Update()
    {
        foreach (GameObject footprint in footprints)
        {
            footprint.transform.localScale = footprint.transform.localScale * (-Time.deltaTime + 1f);

            if (footprint.transform.localScale.magnitude < 0.1f)
            {
                footprints.Remove(footprint);
                Destroy(footprint); // Is it really that bad to use Destroy and Instantiate?
            }
        }
    }
}
