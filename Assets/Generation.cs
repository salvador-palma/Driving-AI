using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Generation : MonoBehaviour
{
    [SerializeField] List<Car> population;
    List<float[]> selection;
    bool Finished =true;
    public int Generations;
    public int Elements;
    public TMP_Text info;

    public bool AutoPlay = false;
    float transition = 0.5f;
    float transitionTimer = 0.5f;

    float timerCount;
    float timer = 240f;
    public void Generate(){
        Debug.Log("Generated");
        Time.timeScale = 7;
        population = new List<Car>();
        for (int i = 0; i < 100; i++)
        {
            int[] t = Car.CarNetworkSize;
            Car go = Car.Create(GetRandomFloatArray(t[0]*t[1] + t[1]*t[2] + t[1] + t[2],-100f,100f));
            population.Add(go) ;
            
        }
    }

    public void Breed(){
        while(population.Count < 100){
            float[] prev = null;
            foreach(float[] val in selection){
                if(prev==null){
                    prev = val;
                }else{
                    if(population.Count >= 100){
                        return;
                    }
                    population.Add(Car.Create(prev, val));
                    prev = null;
                }
            }
        }
        Generations++;
        if(AutoPlay){
            transitionTimer = transition;
        }
        
    }
    public void AutoPlayToggle(){
        AutoPlay = !AutoPlay;
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.D)){
            Time.timeScale++;
             Debug.Log(Time.timeScale);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            Time.timeScale--;
             Debug.Log(Time.timeScale);
        }
       
        if(!Finished){
            int cur = 0;
            bool check = true;
            foreach (Car individual in population)
            {
                if(individual.Alive){
                    cur++;
                    check = false;
                }
            }
            info.text = "Alive: " + cur + " Gen: " + Generations;
            if(check){
                Finished = true;
                Debug.Log("Over");
                if(AutoPlay){Selection();}
            }
        }
        
        if(AutoPlay && transitionTimer>0){
            transitionTimer-=Time.deltaTime;
            if(transitionTimer<=0){
                Wake();
            }
        }
        if(timerCount>0){
            timerCount-=Time.deltaTime;
            if(timerCount<=0){
                Finished=true;
                if(AutoPlay){Selection();}
            }
        }
        
    }

    public void Selection(){
        selection = new List<float[]>();
        selection.Clear();
        Car max = null;
        int cur = 0;
        foreach(Car individual in population){
            if(cur==5){
                cur=0;
                selection.Add(max.network.NeuralValues);
                max=null;
            }
            if(max != null){
                max = max.Fitness > individual.Fitness ? max : individual;
            }else{
                max = individual;
            }
            cur++;
        }

        foreach(Car c in population){
            Destroy(c.gameObject);
        }
        population.Clear();
        foreach(float[] val in selection){
            population.Add(Car.Create(val));
        } 
        Breed();
                  
        
    }

    public static float[] GetRandomFloatArray(int length, float minVal = 0f, float maxVal = 1f)
    {
        float[] res = new float[length];
        for (int i = 0; i < length; i++)
        {
            res[i] = Random.Range(minVal,maxVal);
        }
        return res;
    }

    public void Wake(){
        foreach (Car c in population)
        {
            c.Alive=true;
        }
        Finished = false;
        timerCount = timer;
    }
}
