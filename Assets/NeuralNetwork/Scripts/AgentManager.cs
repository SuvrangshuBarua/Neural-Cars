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

    [SerializeField]
    private Vector3 rotationInitials;

    public float framePassed = 0;

    private void Start()
    {
        AgentSimulation();
    }
    private void Update()
    {
        if (_isSimulated)
            framePassed += Time.deltaTime;
    }



    private void AgentSimulation()
    {
        agents = new Agent[numberOfAgents];
        for (int i = 0; i < numberOfAgents; i++)
        {
            GameObject car = Instantiate(agentPrefab.gameObject,transform.position, Quaternion.Euler(rotationInitials), transform);
            agents[i] = car.GetComponent<Agent>();
            agents[i].StartAgent();
        }
        

    }




}
