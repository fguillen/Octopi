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

    public static Quaternion Rotation2DTowards(Transform origin, Vector3 target, bool flip = false)
    {
        Vector3 angle = target - origin.position;
        angle.Normalize();

        float rotation = Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;

        if(flip)
            rotation -= 180;

        return Quaternion.Euler(0f, 0f, rotation - 180);
    }
}
