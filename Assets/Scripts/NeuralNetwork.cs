using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public List<float[,]> weightList;

    public int numInput;
    public int numHidden;
    public int numOutput;

    public NeuralNetwork(int numInput, int numHidden, int numOutput)
    {
        this.numInput = numInput;
        this.numHidden = numHidden;
        this.numOutput = numOutput;

        weightList = new List<float[,]>();
        weightList.Add(new float[numInput + 1, numHidden]);
        weightList.Add(new float[numHidden + 1, numOutput]);
    }

    public float[] Compute(float[] inputs)
    {
        float[] hidden = new float[numHidden];
        // Mat mult
        for (int i = 0; i < numHidden; i++)
        {
            for (int j = 0; j < numInput; j++)
            {
                hidden[i] += inputs[j] * weightList[0][j, i];
            }
            hidden[i] += weightList[0][numInput, i];
        }
        // Sigmoid 
        hidden = Sigmoid(hidden);

        float[] output = new float[numOutput];
        // Mat mult
        for (int i = 0; i < numOutput; i++)
        {
            for (int j = 0; j < numHidden; j++)
            {
                output[i] += hidden[j] * weightList[1][j, i];
            }
            output[i] += weightList[1][numHidden, i];
        }
        // Sigmoid 
        output = Sigmoid(output);

        return output;
    }

    private static float[] Sigmoid(float[] values)
    {
        float[] res = new float[values.Length];
        for (int i = 0; i < res.Length; i++)
        {
            res[i] = 1.0f / (1.0f + Mathf.Exp(values[i]));
        }
        return res;
    }
}
