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
    private GameObject currentsnowball;
    void Start()
    {
        cam = Camera.main;
        newSnowball.GetComponent<Rigidbody>().useGravity = false;
        line1.positionCount = linesegment;
    }
    public void Update()
    {
        if (collectsnow == true)
        {
            RaycastHit hit;
            Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit, 100f, layer))
            {

                Vector3 vo = CalculateV(hit.point, hand.position, 1f);
                visuize(vo);

                newSnowball.transform.localScale = new Vector3(.5f, .5f, .5f);

                
            }
            if (Input.GetButtonDown("Fire1"))
            {


                ChargeSnowball();

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
    private void ChargeSnowball()
    {

        
        currentsnowball = GameObject.Instantiate(newSnowball);
        currentsnowball.transform.position = new Vector3(1f, 1f, 1f);
        currentsnowball.GetComponent<projectileScript>().size = 1f;
        currentsnowball.transform.position = hand.transform.position;// get the player's hand position
        currentsnowball.GetComponent<Rigidbody>().useGravity = false;
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
        currentsnowball.GetComponent<Rigidbody>().AddForce(transform.forward * ballforce);
        currentsnowball.GetComponent<Rigidbody>().useGravity = true;

        
        StopAllCoroutines();
    }
    void UpdateSnowball()
    {

        newSnowball.transform.position = hand.transform.position;

        newSnowball.transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime;


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
   