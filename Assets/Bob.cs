using UnityEngine;

public class Bob : MonoBehaviour
{
    public float amplitude = 1;
    public float frequency = 1;

    Vector3 position;

    void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        transform.position = position + Vector3.up * (1 + Mathf.Sin(Time.time * frequency * Mathf.PI * 2)) * amplitude;
    }
}
