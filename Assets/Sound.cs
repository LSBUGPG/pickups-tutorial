using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip clip;

    public void Play()
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}
