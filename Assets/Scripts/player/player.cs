using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Para manipulação de cenas

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private bool jumpPressed;
    private AudioSource audioSource;
    private bool win;
    private bool hasPlayedWinSound; // Variável para evitar som duplicado
    private GameObject cameraPos, inicialPos;

    public float speed;
    public int maxJumps = 1; // Número máximo de pulos
    private int remainingJumps; // Pulos restantes
    public bool isGrounded;
    public float jumpForce;
    public AudioClip jumpSound;
    public int bublee;
    public TextMeshProUGUI text_bublee;
    public AudioClip winSound;
    public GameObject panelWin;
    public float speedwin;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        remainingJumps = maxJumps; // Inicializar com o máximo de pulos
        win = false;
        hasPlayedWinSound = false; // Inicializar como falso
        cameraPos = GameObject.Find("Main Camera");
        inicialPos = GameObject.Find("InicialPos");

        // Inicializa o painel fora da tela
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        Move(moveX);

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }

        text_bublee.text = bublee.ToString();
    }

    void FixedUpdate()
    {
        if (jumpPressed)
        {
            if (isGrounded || remainingJumps > 0)
            {
                Jump();
            }
            jumpPressed = false;
        }
        winGame();
    }

    void Move(float moveX)
    {
        rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        if (moveX > 0)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            anim.SetBool("IsRun", true);
        }
        else if (moveX < 0)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            anim.SetBool("IsRun", true);
        }
        else
        {
            anim.SetBool("IsRun", false);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetBool("IsJump", true);

        if (!isGrounded)
        {
            remainingJumps--; // Reduzir apenas se não estiver no chão
        }
        audioSource.PlayOneShot(jumpSound);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            anim.SetBool("IsJump", false);
            remainingJumps = maxJumps; // Resetar o número de pulos ao tocar o chão
        }
        if (collision.gameObject.CompareTag("win"))
        {
            win = true;
        }

        // Verifica se o personagem colidiu com o colisor de reinício
        if (collision.gameObject.CompareTag("Reset"))
        {
            RestartGame(); // Reinicia o jogo
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    private void winGame()
    {
        if (win && !hasPlayedWinSound)
        {
            audioSource.PlayOneShot(winSound);
            rb.linearVelocity = Vector2.zero;
            hasPlayedWinSound = true;
            panelWin.transform.position = new Vector2(panelWin.transform.position.x, Screen.height - 500);

            // Obtém a posição da câmera e converte para coordenadas da tela
            Vector3 targetPosition = cameraPos.transform.position;

            // Move o painel até a posição da câmera (relativo à tela)
            panelWin.transform.position = Vector2.MoveTowards(
                panelWin.transform.position,
                new Vector2(targetPosition.x, targetPosition.y), // A posição do painel é ajustada para a posição da câmera
                speedwin * Time.deltaTime
            );

            // Verifica se o painel chegou perto da posição da câmera
            if (Vector2.Distance(panelWin.transform.position, new Vector2(targetPosition.x, targetPosition.y)) < 0.1f)
            {
                // Garante que o painel pare exatamente na posição da câmera
                panelWin.transform.position = new Vector2(targetPosition.x, targetPosition.y);
            }
        }
    }

    // Função para reiniciar o jogo
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recarrega a cena atual
    }
}
