using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : MonoBehaviour
{
    Rigidbody2D rbEyeball; // Refer�ncia ao componente Rigidbody2D do inimigo
    [SerializeField] float speed = 2f; // Velocidade de movimento do inimigo
    [SerializeField] Transform point1, point2; // Pontos usados para detec��o de colis�o
    [SerializeField] LayerMask layer; // Camada usada para detectar colis�es
    [SerializeField] bool isColliding; // Vari�vel para verificar se est� colidindo com algo
    BoxCollider2D ColliderEyeball;
    //CapsuleCollider2D ColliderEyeball;
    Animator animEyeball;

    private void Awake()
    {
        rbEyeball = GetComponent<Rigidbody2D>(); // Obt�m o componente Rigidbody2D ao acordar
        animEyeball = GetComponent<Animator>();
        ColliderEyeball = GetComponent<BoxCollider2D>();
        //ColliderEyeball = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {

    }
    private void FixedUpdate()
    {
        // Define a velocidade do Rigidbody2D do inimigo na dire��o horizontal
        rbEyeball.velocity = new Vector2(speed, rbEyeball.velocity.y);

        // Verifica se est� colidindo com algo entre os pontos point1 e point2
        isColliding = Physics2D.Linecast(point1.position, point2.position, layer);

        // Desenha uma linha na cena para representar a �rea de detec��o
        Debug.DrawLine(point1.position, point2.position, Color.blue);

        // Se estiver colidindo, inverte a dire��o de movimento do inimigo e a escala no eixo X
        if (isColliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            speed *= -1; // Inverte a dire��o de movimento multiplicando a velocidade por -1
        }
    }

    // M�todo Update vazio, que pode ser usado para atualiza��es de l�gica do jogo
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (transform.position.y + 0.5f < collision.transform.position.y)
            {
                collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6,(ForceMode2D)ForceMode.Impulse);
                animEyeball.SetTrigger("Death");
                speed = 0;
                Destroy(gameObject, 0.3f);
                ColliderEyeball.enabled = false;
            }
            else
            {
                FindObjectOfType<Movimento>().Death();

                Eyeball[] eyeball = FindObjectsOfType<Eyeball>();

                for (int i = 0; i < eyeball.Length; i++)
                {
                    eyeball[i].speed = 0;
                    eyeball[i].animEyeball.speed = 0;
                }
            }
        }
    }
}