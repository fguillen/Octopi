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

    public static int AddNoise(int value)
    {
        return AddNoise(value, value / 2);
    }

    public static int AddNoise(int value, int delta)
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

    public static Vector3 PositionInCircumference(Vector3 center, float radius, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);
        Vector3 position = new Vector3(x, y, 0);
        position *= radius;
        position += center;

        return position;
    }
}
