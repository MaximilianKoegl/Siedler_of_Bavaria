using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_Borders : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }
	// Update is called once per frame
	void Update () {
            GameObject[] gos, fos, tos, bos, kos, hos, jos;
            gos = GameObject.FindGameObjectsWithTag("Unterfranken");
            fos = GameObject.FindGameObjectsWithTag("Oberfranken");
            tos = GameObject.FindGameObjectsWithTag("Mittelfranken");
            bos = GameObject.FindGameObjectsWithTag("Oberpfalz");
            kos = GameObject.FindGameObjectsWithTag("Oberbayern");
            hos = GameObject.FindGameObjectsWithTag("Niederbayern");
            jos = GameObject.FindGameObjectsWithTag("Schwaben");




            foreach (GameObject go in gos)
            {
                MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.white;
            }
            foreach (GameObject go in fos)
            {
                MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.magenta;
            }
            foreach (GameObject go in tos)
            {
                MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.black;
            }
            foreach (GameObject go in bos)
            {
                MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.blue;
            }
            foreach (GameObject go in kos)
            {
                MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.green;
            }
            foreach (GameObject go in hos)
            {
                MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.cyan;
            }
            foreach (GameObject go in jos)
            {
                MeshRenderer mr = go.GetComponentInChildren<MeshRenderer>();
                mr.material.color = Color.yellow;
            }
        }
    }
