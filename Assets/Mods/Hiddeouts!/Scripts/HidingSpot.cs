using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] bool canHide;
    [SerializeField] bool hidden;
    [SerializeField] GameObject player;
    public AudioSource audioSource;
    public GameObject text;
    public Image screenCover;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canHide && !hidden)
            {
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY
                    | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                player.GetComponent<Collider>().enabled = false;
                audioSource.Play();
                StartCoroutine(FadeToBlackAndHidePlayer());
                hidden = true;
            }
            else if (hidden)
            {
                audioSource.Play();
                StartCoroutine(FadeToBlackAndHidePlayer());
                player.GetComponent<Collider>().enabled = true;
                player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX
                    | RigidbodyConstraints.FreezeRotationZ;

                hidden = false;
                player.GetComponentInChildren<Renderer>().enabled = true;
            }
        }
    }

    // Corrutina para la transición a negro
    IEnumerator FadeToBlackAndHidePlayer()
    {
        float duration = 0.5f; // Duración de la transición
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            screenCover.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }

        screenCover.color = new Color(0, 0, 0, 1);

        player.GetComponentInChildren<Renderer>().enabled = false;

        yield return new WaitForSeconds(0.5f); // Esperar 0.5 segundos

        // Transición de negro a transparente
        startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            screenCover.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        screenCover.color = new Color(0, 0, 0, 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canHide = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canHide = false;
    }
}
