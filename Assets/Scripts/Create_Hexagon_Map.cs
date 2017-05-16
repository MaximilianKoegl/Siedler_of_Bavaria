using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public GameObject hexPrefab;

    //Amount of hex tiles
    int width = 80;
    int height = 80;

    float xOffset = 1.8f;
    float zOffset = 1.55f;
	// Use this for initialization
	void Start () {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xPos = x*xOffset;
                if( y % 2 == 1)
                {
                    xPos += xOffset/2;

                }

               GameObject hex_go = (GameObject)Instantiate(hexPrefab, new Vector3(xPos, 0, y*zOffset), Quaternion.identity);

                hex_go.name = "Hex_" + x + "_" + y;

                hex_go.transform.SetParent(this.transform);

            }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
