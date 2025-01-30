using UnityEngine;

public class Bublee : MonoBehaviour
{
    public AudioClip bubbleSound; // Som da bolha
    private AudioSource audioSource; // Componente AudioSource

    private void Start()
    {
        // Obtém ou adiciona dinamicamente o componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Verifica se o som foi atribuído e configura o AudioSource
        if (bubbleSound != null)
        {
            audioSource.clip = bubbleSound;
            audioSource.playOnAwake = false; // Garante que o som não toque automaticamente
        }
        else
        {
            Debug.LogWarning("BubbleSound não está atribuído ao objeto " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Incrementa a variável no PlayerController
            collision.GetComponent<PlayerController>().bublee++;

            // Verifica se o som está configurado antes de tocar
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }

            // Desativa a bolha visualmente enquanto o som toca
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            // Destroi o objeto após o som terminar
            Destroy(gameObject, audioSource.clip != null ? audioSource.clip.length : 0f);
        }
    }
}
