using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public LayerMask terrainLayerMask;
    public LayerMask ignoreLayerMask;
    float SensorDistance=15f;
    Car parent;


    float[] SensorDistances = new float[2]{1.5f, 0.5f};
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.GetComponent<Car>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = transform.right;
        RaycastHit2D hitFront = Physics2D.Raycast(transform.position, direction, SensorDistance, terrainLayerMask & ~ignoreLayerMask);

        float distance = hitFront.distance;
        if (distance < SensorDistances[1]){
            Debug.DrawRay(transform.position, direction * hitFront.distance, Color.red); 
        }else if (distance < SensorDistances[0]){
            Debug.DrawRay(transform.position, direction * hitFront.distance, Color.yellow); 
        }else{
             Debug.DrawRay(transform.position, direction * hitFront.distance, Color.green); 
        }
       
        
        if (hitFront.collider != null) {
            parent.Sensors[transform.GetSiblingIndex()] = hitFront.distance;
        }
    }
}
