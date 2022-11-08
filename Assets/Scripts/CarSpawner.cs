using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarSpawner : MonoBehaviour
{
    public int numberOfCars = 50;
    public GameObject prefab;
    public List<GameObject> cars;
    [SerializeField]
    private int generationSurvivalTime = 20;
    [SerializeField]
    private float generationStartTime = 0;
    public int generation = 0;

    public TMPro.TextMeshProUGUI generationText;
    void Start()
    {
        for (int i = 0; i < numberOfCars; i++)
        {
            GameObject car = Instantiate(prefab, transform.position, transform.rotation);
            AIController aiRef = car.GetComponent<AIController>();
            aiRef.steeringSensitivity = Random.Range(0.01f, 0.03f);
            aiRef.lookAhead = Random.Range(18.0f, 22.0f);
            aiRef.maxTorque = Random.Range(180.0f, 220.0f);
            aiRef.maxSteerAngle = Random.Range(50.0f, 70.0f);
            aiRef.maxBrakeTorque = Random.Range(4500.0f, 5500.0f);
            aiRef.accelCornerMax = Random.Range(18.0f, 22.0f);
            aiRef.brakeCornerMax = Random.Range(3.0f, 7.0f);
            aiRef.accelVelocityThreshold = Random.Range(18.0f, 22.0f);
            aiRef.brakeVelocityThreshold = Random.Range(8.0f, 12.0f);
            aiRef.antiroll = Random.Range(4500.0f, 5500.0f);
            cars.Add(car);
        }
        Time.timeScale = 5;
        generationText.text = $"Generation: {generation}";
    }

    private GameObject GeneCrossover(AIController parent1, AIController parent2)
    {
        GameObject child = Instantiate(prefab, transform.position, transform.rotation);
        AIController aiRef = child.GetComponent<AIController>();
        aiRef.steeringSensitivity = (parent1.steeringSensitivity + parent2.steeringSensitivity) / 2.0f;
        aiRef.lookAhead = (parent1.lookAhead + parent2.lookAhead) / 2.0f;
        aiRef.maxTorque = (parent1.maxTorque + parent2.maxTorque) / 2.0f;
        aiRef.maxSteerAngle = (parent1.maxSteerAngle + parent2.maxSteerAngle) / 2.0f;
        aiRef.maxBrakeTorque = (parent1.maxBrakeTorque + parent2.maxBrakeTorque) / 2.0f;
        aiRef.accelCornerMax = (parent1.accelCornerMax + parent2.accelCornerMax) / 2.0f;
        aiRef.brakeCornerMax = (parent1.brakeCornerMax + parent2.brakeCornerMax) / 2.0f;
        aiRef.accelVelocityThreshold = (parent1.accelVelocityThreshold + parent2.accelVelocityThreshold) / 2.0f;
        aiRef.brakeVelocityThreshold = (parent1.brakeVelocityThreshold + parent2.brakeVelocityThreshold) / 2.0f;
        aiRef.antiroll = (parent1.antiroll + parent2.antiroll) / 2.0f;

        return child;
    }

    private void Selection()
    {
        generationStartTime = Time.realtimeSinceStartup;
        List<GameObject> fitnessSortedCars = cars.OrderByDescending(o => o.GetComponent<AIController>().fitness).ToList();
        int cutoffList = (int)(fitnessSortedCars.Count / 2.0f);
        cars.Clear();
        for (int i = 0; i < cutoffList; i++)
        {
            cars.Add(GeneCrossover(fitnessSortedCars[i].GetComponent<AIController>(),
                                   fitnessSortedCars[i + 1].GetComponent<AIController>()));
            cars.Add(GeneCrossover(fitnessSortedCars[i + 1].GetComponent<AIController>(),
                                   fitnessSortedCars[i].GetComponent<AIController>()));
        }
        for (int i = 0; i < fitnessSortedCars.Count; i++)
        {
            Destroy(fitnessSortedCars[i]);
        }
        fitnessSortedCars.Clear();
        generation++;
        generationText.text = $"Generation: {generation}";
    }
    private void Update()
    {
        if(Time.realtimeSinceStartup > generationStartTime + generationSurvivalTime)
        {
            Selection();
        }
    }

}
