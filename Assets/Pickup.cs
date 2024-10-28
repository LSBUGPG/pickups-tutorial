using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float speed = 360;
    public AudioClip clink;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            AudioSource.PlayClipAtPoint(clink, Camera.main.transform.position);
            player.AddCoin();
            gameObject.SetActive(false);
        }
    }
}
