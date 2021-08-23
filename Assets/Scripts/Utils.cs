using UnityEngine;

public class Utils
{
    public static float AddNoise(float value)
    {
        return AddNoise(value, value / 2.0f);
    }

    public static float AddNoise(float value, float delta)
    {
        return value + Random.Range(-delta, delta);
    }
}
