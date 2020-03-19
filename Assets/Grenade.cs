using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject explosion;
    public GameObject grenade;

    AudioSource audioSource;
    public AudioClip[] audio_Explosions;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
            }

    void OnTriggerEnter(Collider other)
    {
        GameObject _explosion = Instantiate(explosion, transform.position, transform.rotation);
        audioSource.PlayOneShot(audio_Explosions[Random.Range(0, audio_Explosions.Length)]);
        Destroy(_explosion.gameObject, 5f);
        grenade.SetActive(false);
    }
}
