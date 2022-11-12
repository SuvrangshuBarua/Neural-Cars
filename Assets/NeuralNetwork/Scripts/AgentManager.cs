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
    public int numberOfAgentActive = 0;
    public float evolutionThreshold = 100f;

    [SerializeField]
    private Vector3 rotationInitials;

    public float framePassed = 0;

    private void Update()
    {
        if (_isSimulated)
        {
            if(framePassed < evolutionThreshold)
            {
                framePassed += Time.deltaTime;
            }
            else
            {
                _isSimulated = false;
                AllAgentsDeactivated();
            }
        }
            
    }

    private void Start()
    {
        GenerateWorld();
        Time.timeScale = Time.timeScale * 10;
    }

    private void GenerateWorld()
    {
        agentScores = new float[numberOfAgents];
        GenerateAgents();
        SimulateAllAgents();
    }

    private void SimulateAllAgents()
    {
        Debug.Log("Simulated All Agents");
        for (int i = 0; i < agentScores.Length; i++)
        {
            agentScores[i] = 0;
        }

        foreach (Agent agent in agents)
        {
            agent.transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);//move to start point
            agent.InitializeAgent();
            agent.StartAgent();
        }

        _isSimulated = true;
        numberOfAgentActive = agents.Length;
    }
    private void GenerateAgents()
    {
        agents = new Agent[numberOfAgents];
        for (int i = 0; i < numberOfAgents; i++)
        {
            Agent agent = Instantiate(agentPrefab, startPoint.position, startPoint.rotation, transform);
            agent.InitializeAgent();
            agent.id = i;
            agent.onGoalReached.AddListener(OnGoalReached);
            agent.onFailed.AddListener(OnFailed);
            agents[i] = agent;
            
        }
    }

    private void OnGoalReached(int id)
    {
        numberOfAgentActive--;
        agentScores[id] = 5000 - framePassed;//give high reward for creatures reaching goal.

        //start new generation if all dead
        if (numberOfAgentActive <= 0)
        {
            AllAgentsDeactivated();
        }
    }

    private void OnFailed(int id)
    {
        numberOfAgentActive--;
        agentScores[id] = agents[id].transform.position.magnitude;//when failed, creature will get score equal to time they survived.

        //start new generation if all dead
        if (numberOfAgentActive <= 0)
        {
            AllAgentsDeactivated();
        }


    }

    private void AllAgentsDeactivated()
    {
        framePassed = 0;
        _isSimulated = false;
        float[][] parents = new float[numberOfAgents][];

        //get gene of parents
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i] = agents[i].GetBrainData();
        }
        //get offspring genes
        float[][] offsprings =
          GeneticAlgorithm.GetOffsprings(parents, agentScores, new[] {0.4f, 0.2f, 0.2f, 0.1f, 0.1f}, .1f, numberOfAgents);
        //create ofsprings
        for (int i = 0; i < numberOfAgents; i++)
        {
            agents[i].SetBrainData(offsprings[i]);
            //agents[i].onGoalReached.RemoveListener(OnGoalReached);
            //agents[i].onFailed.RemoveListener(OnFailed);
        }

        //run offsprings
        SimulateAllAgents();
    }
}

