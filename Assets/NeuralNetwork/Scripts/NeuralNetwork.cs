using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomValue = UnityEngine.Random;

public class NeuralNetwork
{
    private int inputLayerNodeCount, hiddenLayerNodeCount, outputLayerNodeCount;
    private float[][] weightInputHiddenArray, weightHiddenOutputArray;
    private float[] hiddenBias, outputBias;

    public NeuralNetwork(int inputNodeCount, int outputNodeCount, int hiddenNodeCount)
    {
        //Nodes Count Initialization

        inputLayerNodeCount = inputNodeCount;
        hiddenLayerNodeCount = hiddenNodeCount;
        outputLayerNodeCount = outputNodeCount;

        //Bias Initialization
        hiddenBias = new float[hiddenLayerNodeCount];
        outputBias = new float[outputLayerNodeCount];

        weightInputHiddenArray = new float[inputLayerNodeCount][];
        for (int i = 0; i < inputLayerNodeCount; i++)
        {
            weightInputHiddenArray[i] = new float[hiddenLayerNodeCount];
        }

        weightHiddenOutputArray = new float[hiddenLayerNodeCount][];
        for (int i = 0; i < hiddenLayerNodeCount; i++)
        {
            weightHiddenOutputArray[i] = new float[outputLayerNodeCount];
        }
    }

    // Radomize weights and biases from values ranging from -1 to 1
    public void RandomizeValues()
    {
        // Hidden Layer Bias Randomization
        for (int i = 0; i < hiddenBias.Length; i++)
        {
            hiddenBias[i] = RandomValue.Range(-1f, 1f);
        }

        // Output Layer Bias Randomization
        for (int i = 0; i < outputBias.Length; i++)
        {
            outputBias[i] = RandomValue.Range(-1f, 1f);
        }

        //weightInputHiddenArray.Length retruns number of Rows
        for (int i = 0; i < weightInputHiddenArray.Length; i++)
        {
            //weightInputHiddenArray[i].Length returns Length of each Row
            for (int j = 0; j < weightInputHiddenArray[i].Length; j++)
            {
                weightInputHiddenArray[i][j] = RandomValue.Range(-1f, 1f);
            }
        }

        for (int i = 0; i < weightHiddenOutputArray.Length; i++)
        {
            for (int j = 0; j < weightHiddenOutputArray[i].Length; j++)
            {
                weightHiddenOutputArray[i][j] = RandomValue.Range(-1f, 1f);
            }
        }
    }
    public float[] ProcessInput(float[] input)
    {
        if (input.Length != inputLayerNodeCount) return null;

        float[] hiddenLayerValues = new float[hiddenLayerNodeCount]; //Hidden Layer Value Storage
        float[] outputValues = new float[outputLayerNodeCount]; //Output Layer Value Storage

        // calculate value for each nodes in hidden layer
        for (int i = 0; i < hiddenLayerNodeCount; i++)
        {
            float summation = 0;
            //add Weight*nodeValue
            for (int j = 0; j < inputLayerNodeCount; j++)
            {
                summation += weightInputHiddenArray[j][i] * input[j];
            }

            // add bias
            summation += hiddenBias[i];

            // apply Activation function
            hiddenLayerValues[i] = ActivationFunction(summation);
        }

        // calculate value for each nodes in output layer
        for (int i = 0; i < outputLayerNodeCount; i++)
        {
            float summation = 0;
            //add (Weight*nodeValue) for all conected node
            for (int j = 0; j < hiddenLayerNodeCount; j++)
            {
                summation += weightHiddenOutputArray[j][i] * hiddenLayerValues[j];
            }

            //add bias of this node
            summation += outputBias[i];

            //apply Activation function
            outputValues[i] = ActivationFunction(summation);
        }

        return outputValues;
    }

    //use sigmoid as activation function
    private float ActivationFunction(float value)
    {
        return (float)(1 / (1 + Mathf.Exp(-value)));
    }

    public void SetValues(float[] geneSequence)
    {
        if (geneSequence.Length !=
          inputLayerNodeCount * hiddenLayerNodeCount + hiddenLayerNodeCount + hiddenLayerNodeCount * outputLayerNodeCount + outputLayerNodeCount) return;

        int currentIndex = 0; //index of array element to be filled

        // first add weights of ip-> hl starting from connection of node 0 of hidden layer
        for (int i = 0; i < weightInputHiddenArray.Length; i++)
        {
            for (int j = 0; j < weightInputHiddenArray[i].Length; j++)
            {
                weightInputHiddenArray[i][j] = geneSequence[currentIndex];
                currentIndex++;
            }
        }

        // add weight of bias of hidden layer
        for (int i = 0; i < hiddenBias.Length; i++)
        {
            hiddenBias[i] = geneSequence[currentIndex];
            currentIndex++;
        }

        // add weights of hl-> op starting from connection of node 0 of output layer
        for (int i = 0; i < weightHiddenOutputArray.Length; i++)
        {
            for (int j = 0; j < weightHiddenOutputArray[i].Length; j++)
            {
                weightHiddenOutputArray[i][j] = geneSequence[currentIndex];
                currentIndex++;
            }
        }

        //add weight of bias of output layer
        for (int i = 0; i < outputBias.Length; i++)
        {
            outputBias[i] = geneSequence[currentIndex];
            currentIndex++;
        }

    }

    public float[] GetGeneSequence()
    {
        float[] geneSequence =
          new float[inputLayerNodeCount * hiddenLayerNodeCount + hiddenLayerNodeCount + hiddenLayerNodeCount * outputLayerNodeCount + outputLayerNodeCount];

        int currentIndex = 0;

        // get weights from ip->hl
        for (int i = 0; i < weightInputHiddenArray.Length; i++)
        {
            for (int j = 0; j < weightInputHiddenArray[i].Length; j++)
            {
                geneSequence[currentIndex] = weightInputHiddenArray[i][j];
                currentIndex++;
            }
        }

        // get bias from hl
        for (int i = 0; i < hiddenBias.Length; i++)
        {
            geneSequence[currentIndex] = hiddenBias[i];
            currentIndex++;
        }

        // get weights from hl->op
        for (int i = 0; i < weightHiddenOutputArray.Length; i++)
        {
            for (int j = 0; j < weightHiddenOutputArray[i].Length; j++)
            {
                geneSequence[currentIndex] = weightHiddenOutputArray[i][j];
                currentIndex++;
            }
        }

        // get bias from op
        for (int i = 0; i < outputBias.Length; i++)
        {
            geneSequence[currentIndex] = outputBias[i];
            currentIndex++;
        }

        return geneSequence;
    }
}
