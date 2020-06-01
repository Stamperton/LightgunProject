using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeEndScript : MonoBehaviour
{

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("Start", true);
            StartCoroutine(FadeOut());

        }
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }
}
