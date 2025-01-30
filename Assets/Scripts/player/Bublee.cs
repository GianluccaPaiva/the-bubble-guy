using UnityEngine;

public class Bublee : MonoBehaviour
{
    public AudioClip bubbleSound; // Som da bolha
    private AudioSource audioSource; // Componente AudioSource

    private void Start()
    {
        // Obt�m ou adiciona dinamicamente o componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Verifica se o som foi atribu�do e configura o AudioSource
        if (bubbleSound != null)
        {
            audioSource.clip = bubbleSound;
            audioSource.playOnAwake = false; // Garante que o som n�o toque automaticamente
        }
        else
        {
            Debug.LogWarning("BubbleSound n�o est� atribu�do ao objeto " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Incrementa a vari�vel no PlayerController
            collision.GetComponent<PlayerController>().bublee++;

            // Verifica se o som est� configurado antes de tocar
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }

            // Desativa a bolha visualmente enquanto o som toca
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Destroi o objeto ap�s o som terminar
            Destroy(gameObject, audioSource.clip != null ? audioSource.clip.length : 0f);
        }
    }
}
