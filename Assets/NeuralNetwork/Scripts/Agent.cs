using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Agent : MonoBehaviour
{
    public int id;
    private NeuralNetwork brain;
    private bool isInitialized;
    [SerializeField]
    protected bool isSimulated;

    [SerializeField]
    private int inputs, hiddenLayers, outputs;

    public UnityEvent<int> onGoalReached = new UnityEvent<int>();
    public UnityEvent<int> onFailed = new UnityEvent<int>();
    public UnityAction<float[]> getData;

    public void InitializeAgent()
    {
        if (isInitialized) return;

        brain = new NeuralNetwork(inputs, outputs, hiddenLayers);
        isInitialized = true;
    }

    public void StartAgent()
    {
        isSimulated = true;
    }

    public void StopAgent()
    {
        isSimulated = false;
    }

    protected void SendInputToBrain(float[] inputSignals)
    {
        if (!isInitialized) return;
        GotResultFromBrain(brain.ProcessInput(inputSignals));
    }

    //Get agent chromosome
    public float[] GetBrainData()
    {
        if (!isInitialized) return null;
        return brain.GetGeneSequence();
    }

    //Set agent's chromosome
    public void SetBrainData(float[] gene)
    {
        if (!isInitialized) return;
        brain.SetValues(gene);
    }

    //called when action is received after sending input to brain
    protected virtual void GotResultFromBrain(float[] output)
    {
    }

    protected void GoalReached()
    {
        onGoalReached.Invoke(id);
    }
    protected void OnFailed()
    {
        onFailed.Invoke(id);
    }
}
