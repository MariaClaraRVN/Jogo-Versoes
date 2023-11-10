using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMoviment : MonoBehaviour
{
    // Declara��o de vari�veis
    Rigidbody2D rbPlayer;
    [SerializeField] float speed = 5f; // Velocidade do jogador
    [SerializeField] float jumpForce = 10f; // For�a do pulo
    [SerializeField] bool isJump; // Verifica se o jogador est� pulando
    [SerializeField] bool inFloor = true; // Verifica se o jogador est� no ch�o
    [SerializeField] Transform groundCheck; // Ponto de verifica��o se o jogador est� no ch�o
    [SerializeField] LayerMask groundLayer; // Camada do ch�o
    [SerializeField] bool dead = false; // Verifica se o jogador est� morto
    CapsuleCollider2D playerCollider; // Collider do jogador
    //BoxCollider2D playerCollider; // Collider do jogador


    Animator animPlayer; // Componente de anima��o do jogador

    // Fun��o chamada ao carregar o script
    private void Awake()
    {
        animPlayer = GetComponent<Animator>(); // Obt�m o componente Animator
        rbPlayer = GetComponent<Rigidbody2D>(); // Obt�m o componente Rigidbody2D
        playerCollider = GetComponent<CapsuleCollider2D>(); // Obt�m o componente CapsuleCollider2D
        //playerCollider = GetComponent<BoxCollider2D>(); // Obt�m o componente CapsuleCollider2D
    }

    // Fun��o chamada no in�cio do script
    private void Start()
    {
        dead = false; // Define a vari�vel dead como falso
    }

    // Fun��o chamada a cada frame
    private void Update()
    {
        if (dead) return; // Se o jogador estiver morto, sai da fun��o

        // Verifica se o jogador est� no ch�o
        inFloor = Physics2D.Linecast(transform.position, groundCheck.position, groundLayer);
        Debug.DrawLine(transform.position, groundCheck.position, Color.blue);

        // Define a vari�vel de anima��o 'jump' com base se o jogador est� no ch�o
        animPlayer.SetBool("jump", !inFloor);

        // Verifica se o bot�o de pulo foi pressionado e se o jogador est� no ch�o
        if (Input.GetButtonDown("Jump") && inFloor)
        {
            isJump = true; // Define isJump como verdadeiro
        }
        // Reduz a velocidade do jogador ao soltar o bot�o de pulo enquanto ele est� subindo
        else if (Input.GetButtonUp("Jump") && rbPlayer.velocity.y > 0)
        {
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, rbPlayer.velocity.y * 0.5f);
        }
    }

    // Fun��o chamada a cada intervalo fixo de tempo
    private void FixedUpdate()
    {
        Move(); // Chama a fun��o de movimento
        JumpPlayer(); // Chama a fun��o de pulo
    }

    // Fun��o para movimentar o jogador
    void Move()
    {
        if (dead) return; // Se o jogador estiver morto, sai da fun��o
        float xMove = Input.GetAxis("Horizontal"); // Obt�m a entrada do eixo horizontal
        rbPlayer.velocity = new Vector2(xMove * speed, rbPlayer.velocity.y); // Aplica a velocidade ao jogador

        animPlayer.SetFloat("speed", Mathf.Abs(xMove)); // Define a velocidade na anima��o

        // Verifica a dire��o do movimento e ajusta a rota��o do jogador
        if (xMove > 0)
        {
            transform.eulerAngles = new Vector2(0, 0); // Rota��o para a direita
        }
        else if (xMove < 0)
        {
            transform.eulerAngles = new Vector2(0, 180); // Rota��o para a esquerda
        }
    }

    // Fun��o para controlar o pulo do jogador
    void JumpPlayer()
    {
        if (dead) return; // Se o jogador estiver morto, sai da fun��o

        // Verifica se o jogador est� pulando e aplica a for�a do pulo
        if (isJump)
        {
            rbPlayer.velocity = Vector2.up * jumpForce; // Aplica a for�a do pulo
            isJump = false; // Define isJump como falso
        }
    }

    // Fun��o para lidar com a morte do jogador
    public void Death()
    {
        StartCoroutine(DeathCorotine()); // Inicia a co-rotina de morte
    }

    // Co-rotina para controlar a morte do jogador
    IEnumerator DeathCorotine()
    {
        if (!dead)
        {
            dead = true; // Define o jogador como morto
            animPlayer.SetTrigger("Death"); // Ativa a anima��o de morte
            yield return new WaitForSeconds(0.5f); // Aguarda por 0.5 segundos

            rbPlayer.velocity = Vector2.zero; // Define a velocidade do jogador como zero
            rbPlayer.AddForce(Vector2.up * 15f, ForceMode2D.Impulse); // Aplica uma for�a para cima ao jogador
            playerCollider.isTrigger = true; // Define o collider como gatilho
            Invoke("RestartGame", 2.5f); // Reinicia o jogo ap�s 2.5 segundos
        }
    }

    // Fun��o para reiniciar o jogo
    private void RestartGame()
    {
        SceneManager.LoadScene("Fase1"); // Carrega a cena "Fase1"
    }
}
