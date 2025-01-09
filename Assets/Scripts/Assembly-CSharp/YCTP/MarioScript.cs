using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MarioScript : MonoBehaviour
{
    public new Rigidbody2D rigidbody;

    public float speed;
    public float accel;
    public float jump;
    public float gravityScale;

    public Vector2 input;
    public Vector2 velocity;
    public bool grounded;
    public bool isJumping;

    public MathGameScript mgs;

    Coroutine coro;
    public void Complete()
    {
        coro = StartCoroutine(wait());
    }
    public void Uncomplete()
    {
        if (coro != null)
            StopCoroutine(coro);
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        mgs.Deactivate();
    }

    private void Update()
    {
        grounded = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity);
        if (hit)
            grounded = !hit.collider.CompareTag("Mario") && hit.distance <= 1.5f ;
        //Debug.Log(hit.distance);

        // input handling
        int x = Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveRight) ? 1 : 0;
        x -= Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveLeft) ? 1 : 0;
        int jumping = Singleton<InputManager>.Instance.GetActionKeyDown(InputAction.MoveForward) ? 1 : 0;
        jumping = Singleton<InputManager>.Instance.GetActionKeyDown(InputAction.LookBehindJump) ? 1 : jumping;
        //Debug.Log(jumping);
        input = new Vector2(x, jumping);

        if (grounded)
        {
            if (input.y == 1)
                isJumping = true;
        }
        if (input.x != 0)
            transform.rotation = Quaternion.Euler(0, input.x > 0 ? 0 : 180f, 0);
    }

    private void FixedUpdate()
    {
        // movement
        velocity.x += input.x * speed;
        velocity.x *= accel * Time.fixedDeltaTime;
        velocity += Physics2D.gravity * gravityScale;
        if (grounded)
            velocity.y = Physics2D.gravity.y * gravityScale;
        if (isJumping)
        {
            velocity.y = jump;
            isJumping = false;
            grounded = false;
        }
        rigidbody.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal == Vector2.down && velocity.y > 0f)
            velocity.y = 0;
    }

    public void MakeBig()
    {
        GetComponent<RectTransform>().localScale = new Vector3(1, 2, 1);
    }
}
