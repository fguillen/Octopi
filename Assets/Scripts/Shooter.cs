using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    public abstract void InRange();
    public abstract void OutOfRange();
}
