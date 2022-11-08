using UnityEngine;

public static class GeneticAlgorithm
{
    public static float[][] GetOffsprings(float[][] parents, float[] scores, float[] selectionChance, float mutationChance, int offspringsCount)
    {
        int selectedParentCount = selectionChance.Length;

        //Selection
        int[] bestCreatureIndexes = GetBestScoreIndex(selectedParentCount, scores);
        float[][] bestParents = new float[selectedParentCount][];
        for (int i = 0; i < bestCreatureIndexes.Length; i++)
        {
            bestParents[i] = parents[bestCreatureIndexes[i]];
        }

        float total = 0;
        for (int i = 0; i < selectedParentCount; i++)
        {
            total += selectionChance[i];
        }

        //creating offsprings
        float[][] offsprings = new float[offspringsCount][];
        for (int i = 0; i < offspringsCount; i++)
        {
            //crossover
            float rand = Random.Range(0f, total);
            float selectionPoint = selectionChance[0];

            //find parent 1
            float[] parent1 = bestParents[0];
            int parent1Index = 0;
            for (int j = 0; j < selectedParentCount; j++)
            {
                if (rand <= selectionPoint)
                {
                    parent1 = bestParents[j];
                    parent1Index = j;
                    break;
                }

                if (j == selectedParentCount - 1)
                    parent1 = bestParents[selectedParentCount - 1];
                else
                    selectionPoint += selectionChance[j + 1];
            }

            //find parent 2
            rand = Random.Range(0f, total);
            selectionPoint = selectionChance[0];
            float[] parent2 = bestParents[0];

            for (int j = 0; j < selectedParentCount; j++)
            {
                if (j == parent1Index && j != selectedParentCount - 1) continue; //avoid previous parent
                if (rand <= selectionPoint)
                {
                    parent2 = bestParents[j];
                    break;
                }

                if (j == selectedParentCount - 1)
                    parent2 = bestParents[selectedParentCount - 1];
                else
                    selectionPoint += selectionChance[j + 1];
            }

            //create chromosome for offspring
            float[] offspring = new float[parent1.Length];
            for (int j = 0; j < offspring.Length; j++)
            {
                //select gene from parent 1 or 2
                offspring[j] = Random.Range(0f, 1f) > 0.5f? parent1[j] : parent2[j];

                //mutation
                if (Random.Range(0f, 1f) < mutationChance)
                {
                    //mutating from -10 to 10. You can add mutation range to pass variable mutation range
                    offspring[j] = Random.Range(-10f, 10f);
                }
            }

            offsprings[i] = offspring;
        }

        return offsprings;
    }

    //finds index of highest scores. Returns best score's index in descending order
    private static int[] GetBestScoreIndex(int bestPickCount, float[] creatureScore)
    {
        int[] bestCreatureIndex = new int[bestPickCount];
        float[] bestCreatureScores = new float[bestPickCount];

        for (int i = 0; i < creatureScore.Length; i++)
        {
            float score = creatureScore[i];
            for (int j = 0; j < bestPickCount; j++)
            {
                if (score > bestCreatureScores[j])
                {
                    float replacingScore = score;
                    int replacingIndex = i;

                    for (int k = j; k < bestPickCount; k++)
                    {
                        float tempFloat = bestCreatureScores[k];
                        int tempIndex = bestCreatureIndex[k];

                        bestCreatureScores[k] = replacingScore;
                        bestCreatureIndex[k] = replacingIndex;

                        replacingScore = tempFloat;
                        replacingIndex = tempIndex;
                    }
                    break;
                }
            }
        }
        return bestCreatureIndex;
    }
}