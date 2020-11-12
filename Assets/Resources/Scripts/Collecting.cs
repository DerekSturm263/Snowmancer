using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
public class Collecting : MonoBehaviour
{
    public float ballforce = 500000f;
    public GameObject newSnowball;
    public Transform hand;
    public Camera cam;
    public LayerMask layer;
    public float range = 6f;
    public float wait;
    public LineRenderer line1;
    public int linesegment;
    public bool collectsnow = false;
    void Start()
    {
        cam = Camera.main;
        newSnowball.transform.localScale = new Vector3(.1f, .1f, .1f);
        newSnowball.GetComponent<Rigidbody>().useGravity = false;
        line1.positionCount = linesegment;
    }
    public void Update()
    {
        if (collectsnow == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                RaycastHit hit;
                Ray camRay = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(camRay, out hit, 100f, layer))
                {

                    Vector3 vo = CalculateV(hit.point, hand.position, 1f);
                    visuize(vo);

                    newSnowball.transform.localScale = new Vector3(.5f, .5f, .5f);
                    ChargeSnowball();
                }
             
            }
            if (Input.GetButtonUp("Fire1"))
            {
                ThrowSnowball();
            }
            if (Input.GetButton("Fire1"))
            {
                UpdateSnowball();
            }
        }
        
       




    }


            // Somewhere else in the script...

    private void ChargeSnowball()
    {
        

        newSnowball = GameObject.Instantiate(newSnowball);
        newSnowball.transform.position = hand.transform.position;// get the player's hand position
        newSnowball.GetComponent<Rigidbody>().useGravity = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SnowPile")
        {
            collectsnow = true;
        }
    }
    void ThrowSnowball()
    {
        newSnowball.GetComponent<Rigidbody>().AddForce(transform.forward * ballforce);
        newSnowball.GetComponent<Rigidbody>().useGravity = true;

        
        StopAllCoroutines();
    }
    void UpdateSnowball()
    {
        newSnowball.transform.position = hand.transform.position;

        StartCoroutine(scaling());


    }
    void visuize(Vector3 vo)
    {
        for (int i = 0; i < linesegment; i++)
        {
            Vector3 pos = Calculatepositionline(vo, i / (float)linesegment);
            line1.SetPosition(i, pos);
        }
    }
    IEnumerator scaling()
    {
        yield return new WaitForSeconds(1f);
        newSnowball.transform.localScale += new Vector3(.001f, .001f, .001f);
    }
    Vector3 CalculateV(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float sy = distance.y;
        float to = distance.magnitude;


        float VSX = to * time;
        float VU = (sy / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXZ.normalized;
        result *= VSX;
        result.y = VU;

        return result;

    }
    Vector3 Calculatepositionline(Vector3 vo, float time)
    {
        Vector3 VSX = vo;
        VSX.y = 0f;
        Vector3 result = hand.position + vo * time;
        float sy = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + hand.position.y;
        result.y = sy;
        return result;
    }

}
   