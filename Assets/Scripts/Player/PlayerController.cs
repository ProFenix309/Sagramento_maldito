using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController character;

    [Header("Movimiento")]
    float horizantalAxis, verticalAxis;
    [SerializeField] float speed;

    [Space]
    [Header("Fuerza jump y Grabedad")]

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    [SerializeField] float jumpForce;

    [Space]
    [Header("Deteccion de suelos")]

    bool isGround;
    [SerializeField] LayerMask layerColision;
    [SerializeField] Transform checketGround;
    [SerializeField] float radiusGround;

    [Space]
    [Header("Animation")]
    [SerializeField] Animator animator;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }
    private void Update()
    {
        MovePlayer();
        Jump();
    }

    void MovePlayer()
    {
        horizantalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizantalAxis + transform.forward * verticalAxis;
        character.Move(move * speed * Time.deltaTime);

        if (horizantalAxis != 0 || verticalAxis != 0)
        {
            //animator.SetBool("IsWalking", true);
        }

        else
        {
            //animator.SetBool("IsWalking", false);
        }
    }

    void Jump()
    {
        isGround = Physics.CheckSphere(checketGround.position, radiusGround, layerColision);

        if (isGround && velocity.y < -2)
        {
            velocity.y = -2;
        }

        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checketGround.position, radiusGround);
    }

}
