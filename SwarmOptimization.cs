﻿using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SwarmOptimization
{
    CommonMethods CM = new CommonMethods();


    Vector3 direction;
    float velocity;
    
    float omega = 0.5f;         // Inertia
    float phiAgent = 0.8f;      // Acceleration Coefficients
    float phiSwarm = 0.9f;


    float agentBest;
    List<float> AgentBests;
    List<Vector3> AgentBestPositions;

    public float SwarmBest;
    public Vector3 SwarmBestPosition;


    public SwarmOptimization(List<Vector3> AgentStartPositions, int _NumberAgents, float _velocity, float AreaMin, float AreaMax)
    {
        direction = Vector3.zero;
        this.velocity = _velocity;

        agentBest = Single.PositiveInfinity;
        AgentBests = Enumerable.Repeat(agentBest, _NumberAgents).ToList();
        AgentBestPositions = AgentStartPositions;

        SwarmBest = Single.PositiveInfinity;
        SwarmBestPosition = CM.RandomPosition(AreaMin, AreaMax);
    }






    ////////////////////////   SWARM OPTIMIZATION METHODS  ////////////////////////

    // FitnessFunctions: Functions that calculate an agent's fitness -> objective Functions
    public float TestFunction(Vector3 position)
    {
        float fitness = Mathf.Pow(position.x, 2) + Mathf.Pow(position.z, 2) + 1;
        return fitness;
    }

    public float BoothFunction(Vector3 position)
    {
        float x = position.x;
        float z = position.x;
        float fitness = Mathf.Pow((x + (2 * z) - 7), 2) + Mathf.Pow(((2 * x) + z - 5), 2);
        return fitness;
    }

    public float EasomFunction(Vector3 position)
    {
        float x = position.x;
        float z = position.x;
        float pi = Mathf.PI;
        float cosX = Mathf.Cos(x);
        float cosZ = Mathf.Cos(z);
        float fitness = -cosX * cosZ * Mathf.Exp(-(Mathf.Pow(x - pi, 2) + Mathf.Pow(z - pi, 2)));
        return fitness;
    }

    public float HimmelblauFunction(Vector3 position)
    {
        float x = position.x;
        float z = position.x;
        float x2 = Mathf.Pow(x, 2);
        float z2 = Mathf.Pow(z, 2);
        float fitness = Mathf.Pow((x2 + z - 11), 2) + Mathf.Pow((x + z2 - 7), 2);
        return fitness;
    }

    public float AckleyFunction(Vector3 position)
    {
        float pi = Mathf.PI;
        float x = position.x;
        float z = position.x;
        float x2 = Mathf.Pow(x, 2);
        float z2 = Mathf.Pow(z, 2);
        float fitness = -20 * Mathf.Exp(-0.2f * Mathf.Sqrt(0.5f * (x2 + z2))) - Mathf.Exp(0.5f * (Mathf.Cos(2 * pi * x) + Mathf.Cos(2 * pi * z))) + (float) Math.E + 20;
        return fitness;
    }


    // SetBest: Checks each agent's fitness and compares it to both its own previous fitness and the Swarm Best. If current fitness is better than both the  
    //          previous one and the swarm best, the agent's best position and the swarm best position are updated to the current agent position.
    public void SetBest(List<GameObject> listAgents, Func<Vector3, float> FitnessFunction)
    {
        for (int i = 0; i < listAgents.Count; i++)
        {
            Vector3 agentPosition = listAgents[i].transform.position;
            float bestFitnessCandidate = FitnessFunction(agentPosition);

            if (AgentBests[i] > bestFitnessCandidate)
            {
                AgentBests[i] = bestFitnessCandidate;
                AgentBestPositions[i] = agentPosition;
            }

            if (SwarmBest > bestFitnessCandidate)
            {
                SwarmBest = bestFitnessCandidate;
                SwarmBestPosition = agentPosition;
            }
        }
    }


    // MoveAgents: Moves agents while updating the direction for each iteration of the optimization
    public void MoveAgents(List<GameObject> listAgents)
    {
        for (int i = 0; i < listAgents.Count; i++)
        {
            float randomAgent = UnityEngine.Random.Range(0f, 1f);
            float randomSwarm = UnityEngine.Random.Range(0f, 1f);
            Vector3 agentPosition = listAgents[i].transform.position;
            Vector3 newDirection = (omega * direction) + (phiAgent * randomAgent) * (AgentBestPositions[i] - agentPosition) + (phiSwarm * randomSwarm) * (SwarmBestPosition - agentPosition);
            direction = newDirection;

            listAgents[i].transform.Translate(direction.normalized * velocity);
        }
    }
}