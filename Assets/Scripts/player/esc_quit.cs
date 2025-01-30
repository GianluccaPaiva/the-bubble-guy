using UnityEngine;

public class ExitGame : MonoBehaviour
{
    void Update()
    {
        // Verifica se a tecla ESC foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame(); // Chama o método para fechar o jogo
        }
    }

    // Método para fechar o jogo
    void QuitGame()
    {
        // Fecha o jogo no Editor do Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
