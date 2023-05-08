using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{

    private float[] hiddenB;
    private float[] outputB;
    private float[,] InputHiddenW;
    private float[,] HiddenOutputW;

    public float[] NeuralValues;
    public NeuralNetwork(float[] values, int Input, int Hidden, int Output){

        if(values.Length != (Input*Hidden + Hidden*Output + Hidden + Output)){
            Debug.LogError("NeuralNetwork value size differ: " + values.Length + " " + (Input*Hidden + Hidden*Output + Hidden + Output));
        }

        NeuralValues = values;

        hiddenB = new float[Hidden];
        outputB = new float[Output];
        InputHiddenW = new float[Input, Hidden];
        HiddenOutputW = new float[Hidden, Output];
        int V = 0;

        for(int i = 0; i != Input; i++ ){
            for(int j = 0; j != Hidden; j++ ){
                InputHiddenW[i,j] = values[V];
                V++;
            }
        }
        for(int i = 0; i != Hidden; i++ ){
            hiddenB[i] = values[V];
            V++;
        }
        for(int i = 0; i != Hidden; i++ ){
            for(int j = 0; j != Output; j++ ){
                HiddenOutputW[i,j] = values[V];
                V++;
            }
        }
        for(int i = 0; i != Output; i++ ){
            outputB[i] = values[V];
            V++;
        }
    }

    public float[] Predict(float[] inputs){
        float[] hiddenZ = new float[hiddenB.Length];
        for (int j = 0; j < hiddenZ.Length; j++)
        {
            float sum = 0f;
            for (int i = 0; i != inputs.Length; i++)
            {
                sum += inputs[i] * InputHiddenW[i, j];
            }
            sum += hiddenB[j];
            hiddenZ[j] = Sigmoid(sum);
        }
        float[] outputZ = new float[outputB.Length];
        for (int i = 0; i < outputB.Length; i++)
        {
            float sum = 0f;
            for (int j = 0; j < hiddenZ.Length; j++)
            {
                sum += hiddenZ[j] * HiddenOutputW[j,i];
            }
            sum += outputB[i];
            outputZ[i] = sum;
        }
        return softMax(outputZ);
    }

    static float[] softMax(float[] output){
        float[] res = new float[output.Length];
        float sum = 0;
        for (int i = 0; i < output.Length; i++)
        {
            sum+=output[i];
        }
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = output[i]/sum;
        }
        return res;
    }

    public static float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }
}
