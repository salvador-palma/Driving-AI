using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public static int[] CarNetworkSize = new int[3]{3,14,2};
    public float[] Sensors;

    public float speedDef = 5f;

    public float MaxSpeed;
    public float acc;
    public float steering;

    public float Fitness=0f;

    public bool Alive = false;
    Rigidbody2D rb;

    public NeuralNetwork network;
    public GameObject dest;
    void Start() {
        dest = GameObject.Find("Goal");
        Sensors = new float[3]{0f,0f,0f};
        rb = GetComponent<Rigidbody2D>();

    }

    void setupNetwork(float[] values){
        network = new NeuralNetwork(values,CarNetworkSize[0],CarNetworkSize[1],CarNetworkSize[2]);
        Alive=false;
    }

    void FixedUpdate() {

        if(Alive){
            for(int i=0; i!= Sensors.Length; i++){
                if(Sensors[i] == 0){Die(); return;}
            }
            float[] output = network.Predict(Sensors);


            if(output[0] > output[1]){
                Turn(0.3f * steering);
            }else{
                Turn(-0.3f * steering);
            }
            
            rb.velocity = transform.right * Time.deltaTime * speedDef;
            Fitness+=Time.deltaTime;

        }

        //if(Vector2.Distance(dest.transform.position, transform.position) < 1.5f){
         //   onCheck();
        //}
        
        
    }
    public void Die(){
        Alive = false;
        rb.velocity = Vector2.zero;
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        Color c = new Color(1f, 0.0914f, 0f, 0.133f);
        sp.color = c;
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
    }

    public void Turn(float angle){
        Vector3 rotationToAdd = new Vector3(0, 0, angle);
        transform.Rotate(rotationToAdd);
    }

    public static Car Create(float[] values){
        GameObject go = Instantiate(Resources.Load("Prefabs/Individual")) as GameObject;
        Car car = go.GetComponent<Car>();
        car.setupNetwork(values);
        return car;
    }

    public static Car Create(float[] father, float[] mother){
        int n = father.Length;
        int point= (int)Random.Range(0,n);
        float[] cur = father;
        float[] newValues = new float[n];
        for(int i =0; i!= n; i++){
            newValues[i] = cur[i];
            if(i==point){cur=mother;}
        }




        if(Random.Range(0f,1f) <= 0.2){
            
            int ind =(int)(Random.Range(0,n));
            int ind2 =(int)(Random.Range(0,n));
            float temp = newValues[ind];
            newValues[ind]=newValues[ind2];
            newValues[ind2]=temp;
        }
 


        return Car.Create(newValues);
        
    }

    public void onCheck(){
        if(rb.velocity.x > 0 ){
            Fitness += (1/Fitness) * 120;
        }
        
        Die();
    }
    
}
