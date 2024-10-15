using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public struct StarCalculationValues
{
    public float percentHealth;
    public float percentEnergy;
    public bool allAlife;
    public int maxTrashCount;
    public int collectedTrash;

    public float SuccessQualityPercent
    {
        get
        {
            return (percentHealth + percentEnergy) / 2;
        }
    }

    public bool Success
    {
        get
        {
            return allAlife && maxTrashCount <= collectedTrash;
        }
    }
}

public static class StarCalculator
{
    public static int CalculateStarAmount(StarCalculationValues values, bool ignoreSuccess = false)
    {
        if (!ignoreSuccess && !values.Success)
        {
            return -1;
        }

        int stars = 0;

        float successQualityPercent = values.SuccessQualityPercent;

        if (successQualityPercent >= 0.25)
            stars++;
        if (successQualityPercent >= 0.5)
            stars++;
        if (successQualityPercent >= 0.75)
            stars++;

        return stars;
    }
}
