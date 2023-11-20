using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    float horizontalInput;
    public float moveSpeed = 5f;
    bool facingRight = true;
    Vector3 movement;

    //Collision Variables
    private Rigidbody rb;
    private SphereCollider playerCollider;

    private void Start()
    {
        //Get Player Components on game startup
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        //Get Input (A and D)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //Transfer Movement into Vector
        movement = new Vector3(horizontalInput * moveSpeed, 0f, 0f);
        //PlayerMovement
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        //Flip Playermodel depending on Walk direction (Once per)
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }

    //Change Look Direction of Player by Scale inversion
    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
