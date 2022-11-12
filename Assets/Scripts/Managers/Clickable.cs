using UnityEngine;

public class Clickable : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Memulai audio ketika player menyentuh layar
    public void PlayAudio()
    {
        audioSource.Play();
    }
}
