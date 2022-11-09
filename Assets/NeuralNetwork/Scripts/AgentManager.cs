using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public Transform startPoint;
    public int numberOfAgents;
    public Agent[] agents;
    public Agent agentPrefab;
    public float[] agentScores;
    private bool _isSimulated;
    public int numberOfAgentsAlive = 0;

    public float framePassed = 0;

    private void Update()
    {
        if (_isSimulated)
            framePassed += Time.deltaTime;
    }
}
