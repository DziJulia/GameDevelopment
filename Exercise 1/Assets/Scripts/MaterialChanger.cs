using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MaterialChanger : MonoBehaviour
{
    public Material testMaterial;
    // Start is called before the first frame update
    void Start()
    {
        //assigning the game object
        testMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //changes the color
        testMaterial.color = Random.ColorHSV();
    }
}
