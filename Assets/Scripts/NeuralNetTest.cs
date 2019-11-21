using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetTest : MonoBehaviour
{
    public NeuralNetwork network;

    private void Start()
    {
        network = new NeuralNetwork(1, 1, 1);
        network.weightList[0][0,0] = 0.5f;
        network.weightList[0][1,0] = 0.2f;
        network.weightList[1][0,0] = -1f;
        network.weightList[1][1,0] = 0f;

        float[] inputs = new float[1];
        inputs[0] = 1f;

        network.Compute(inputs);
    }

}
