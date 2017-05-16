using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

    public GameObject forrest;
    public GameObject house;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.transform.gameObject;

            if (Input.GetMouseButtonDown(0))
            {
                MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
                GameObject parentHitObject = ourHitObject.transform.parent.gameObject;

                GameObject forrest_go = (GameObject)Instantiate(forrest, parentHitObject.transform.position, Quaternion.identity);

                Debug.Log(ourHitObject.transform.parent.gameObject.name);
                Debug.Log(ourHitObject.transform.position);
                Debug.Log(forrest_go.transform.position);

                forrest_go.name = "Forrest_"+hitInfo.point;

                forrest_go.transform.parent = parentHitObject.transform;

                mr.material.color = Color.red;

            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log(Input.GetMouseButtonDown(1));
                MeshRenderer mr1 = ourHitObject.GetComponentInChildren<MeshRenderer>();
                GameObject parentHitObject = ourHitObject.transform.parent.gameObject;

                GameObject house_go = (GameObject)Instantiate(house, parentHitObject.transform.position, Quaternion.identity);

                house_go.name = "House_" + hitInfo.point;

                house_go.transform.parent = parentHitObject.transform;

                mr1.material.color = Color.blue;

            }


        }

	}
}
