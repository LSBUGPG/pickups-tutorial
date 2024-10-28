using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody body;
    public float force = 2;

    int coins;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Vector3.right * force, ForceMode.Impulse);
        }
    }

    public void AddCoin()
    {
        coins++;
        Debug.Log($"Coins collected: {coins}");
    }
}
