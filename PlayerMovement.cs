using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    public bool debugOn = false;
    public bool autoWallRun = true;
    public Camera cam;
    private float camZoom;
    public float normalSpeed = 150;
    public float wallRunSpeed = 250;
    public float speedMultiplier = 10.0f;
    public float rotationSpeed = 50;
    public float jumpForce = 500;
    private Rigidbody body;
    private float vertical;
    private float horizontal;

    public bool isGrounded;
    public bool onWall;
    public bool wallRunning;
    public bool wallJump; //after a walljump
    public bool extraJump = false;
    private float extraJumpDelay = 0.25f;

    private Animator anim;
    private float speed;
    

    //Tells the code which way to jump off;
    private enum jumpOffDirection
    {
        Right,
        Left,
        Back,
        None
    }
    private jumpOffDirection wallJumpDir;

    // Start is called before the first frame update
    void Start()
    {
        camZoom = cam.fieldOfView;
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        wallJumpDir = jumpOffDirection.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugOn)
        {
            debugMethod();
        }

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        anim.SetFloat("Vertical", vertical);
        anim.SetFloat("Horizontal", horizontal);

        gunZoomFunction();

        jumpFunction();
        movementFunction();
        rotationFunction();
        animationModule();

    }

    //Methods
    private void rotationFunction()
    {
        if (!wallRunning)
        {
            if (isGrounded)
            {
                transform.Rotate((transform.up * horizontal) * rotationSpeed * Time.fixedDeltaTime);
            }
            if(wallJump && extraJump)
            {
                transform.Rotate((transform.up * horizontal) * rotationSpeed * Time.fixedDeltaTime);
            }
            
        }
    }

    private void gunZoomFunction()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            cam.fieldOfView = camZoom * 0.5f;
        }
        else
        {
            cam.fieldOfView = camZoom;
        }
    }
          
    private void movementFunction() //Handles the movement and speed of the player in various states
    {
        if (!wallRunning && !wallJump) //Normal Movement
        {
            if (Input.GetKey(KeyCode.LeftShift) && isGrounded && !wallRunning && !wallJump)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", true);
                speed = normalSpeed * speedMultiplier;
            }
            else
            {
                if (isGrounded)
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isRunning", false);
                    speed = normalSpeed;
                }

            }
            Vector3 velocity = (transform.forward * vertical) * speed * Time.fixedDeltaTime;
            velocity.y = body.velocity.y;
            body.velocity = velocity;

            if (vertical == 0)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
            }
            
        }
        else if (!wallJump) //Wall running
        {
            if (wallJumpDir != jumpOffDirection.Back) //Wall running
            {
                Debug.Log("wall running");
                Vector3 velocity = (transform.forward) * wallRunSpeed * Time.fixedDeltaTime;
                body.velocity = velocity;
            }
            else //Running up a wall
            {
                Debug.Log("Wall up");
                Vector3 velocity = transform.up * normalSpeed * Time.fixedDeltaTime;
                body.velocity = velocity;
            }

        }
        else //Wall Jumping
        {
            Vector3 velocity = (transform.forward) * normalSpeed * Time.fixedDeltaTime;
            velocity.y = body.velocity.y;
        }
    }

    private void jumpFunction() //Handles the jump function of the player in various states
    {
        //If grounded, you jump. Is grounded gets turned to false;
        //Handles kinds of jumps
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //If player is on ground
            if (Physics.Raycast(transform.position, transform.up * -1, 1.1f) && !wallRunning)
            {
                body.AddForce(transform.up * jumpForce);
                isGrounded = false;

            }
            if (wallRunning)
            {
                wallJump = true;
                wallRunning = false;
                isGrounded = false;
                body.useGravity = true;

                if (wallJumpDir == jumpOffDirection.Right)
                {
                    anim.SetBool("wallJumpR", true);
                    body.AddForce(transform.right * jumpForce);
                    body.AddForce(transform.up * (jumpForce / 1.0f));
                }
                else if (wallJumpDir == jumpOffDirection.Left)
                {
                    anim.SetBool("wallJumpL", true);
                    body.AddForce(transform.right * -1 * jumpForce);
                    body.AddForce(transform.up * (jumpForce / 1.0f));
                }
                else if (wallJumpDir == jumpOffDirection.Back)
                {
                    //anim.SetBool("wallJumpB", true);
                    body.AddForce(transform.forward * -1 * jumpForce);
                    body.AddForce(transform.up * (jumpForce / 1.0f));
                }
                else
                {
                    Debug.Log("Not jumping in any direction");
                }
                //Allow player to jump once in the air
                StartCoroutine("DelayExtraJump");

            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJump)
        {
            anim.SetBool("secondJump", true);
            body.velocity = Vector3.zero;
            body.AddForce(transform.up * jumpForce * 1.5f);
            body.AddForce(transform.forward * jumpForce); //NEEDS TO BE CHANGED TO CAMERA DIRECTION
            extraJump = false;
        }
    }

    private IEnumerator DelayExtraJump() //Place where ever extr
    {
        yield return new WaitForSeconds(extraJumpDelay);
        extraJump = true;
    }

    private void wallRunCheck(Collision collision) //Determines if the player is entering or exiting wallrun
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && onWall && !wallRunning && !isGrounded)
        {
            Debug.Log("Begin Wall Run");
            
            onWall = false;
            wallRunning = true;
            isGrounded = true;
            determineWallRun(collision);
            body.useGravity = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && wallRunning)
        {
            Debug.Log("End wall run");
            isGrounded = false;
            wallRunning = false;
            body.useGravity = true;
        }
    }

    private void determineWallRun(Collision collision) //Determines which direction the player will wallrun 
    {
        //Get the normal vector, change player to have their side to the wall
        Vector3 wallForward = transform.TransformVector(collision.transform.forward);
        Vector3 playerRight = transform.TransformVector(transform.right);
        float dot = Vector3.Dot(playerRight.normalized, wallForward.normalized);

        if (dot > (1.0f / 4.0f) && dot <= 1) //Left side run
        {
            anim.SetBool("wallRunningR", true);
            wallJumpDir = jumpOffDirection.Right;
            transform.forward = collision.transform.right * -1;
        }
        else if (dot < (-1.0f / 4.0f) && dot >= -1) //Right side run
        {
            anim.SetBool("wallRunningL", true);
            wallJumpDir = jumpOffDirection.Left;
            transform.forward = collision.transform.right;
        }
        else //Running forward
        {
            wallJumpDir = jumpOffDirection.Back;
            transform.forward = collision.transform.forward * -1;
        }
    }

    //Collision Events
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            extraJump = false;
            Debug.Log("On the ground");
            anim.SetBool("onGround", true);
            anim.SetBool("inAir", false);
            anim.SetBool("secondJump", false);
            isGrounded = true;
            wallJump = false;
        }
        if (collision.gameObject.tag == "Wall")
        {
            onWall = true;

            Debug.Log("On the Wall");
        }
        if (collision.gameObject.tag == "Wall" && wallJump)
        {
            extraJump = false;
            wallJump = false;
            if (autoWallRun)
            {
                Debug.Log("Begin Wall Run");
                onWall = false;
                wallRunning = true;
                isGrounded = true;
                determineWallRun(collision);
                body.useGravity = false;
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("off the ground");
            anim.SetBool("onGround", false);
            anim.SetBool("inAir", true);
        }
        if (collision.gameObject.tag == "Wall" && !wallRunning)
        {
            onWall = false;

            Debug.Log("Off the Wall and not wall running");

        }
        else if (collision.gameObject.tag == "Wall" && wallRunning)
        {
            Debug.Log("Off the Wall and was wall running");
            wallRunning = false;
            isGrounded = false;
            body.useGravity = true;

        }

    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {

        }

        if(collision.gameObject.tag == "Wall")
        {
            //Detect if the player wants to start or stop a wallrun
            wallRunCheck(collision);
            if (!wallRunning)
            {
                onWall = true;
            }
        }
    }

    //Misc Methods
    private void animBoolTurnOn(string animName)
    {
        //If the anim is on, dont turn it on again
        Debug.Log(anim.GetBool(animName));
        if (!anim.GetBool(animName))
        {
            anim.SetBool(animName, true);
        }
    }

    private void animBoolTurnOff(string animName)
    {
        if (anim.GetBool(animName))
        {
            anim.SetBool(animName, false);
        }
    }


    //Maybe use
    public void animationModule()
    {
        if (wallRunning)
        {
            anim.SetBool("inAir", false);
            anim.SetBool("wallJumpL", false);
            anim.SetBool("wallJumpR", false);
            anim.SetBool("secondJump", false);
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);

        }
        else
        {
            anim.SetBool("wallRunningL", false);
            anim.SetBool("wallRunningR", false);

        }
        if (isGrounded)
        {
            anim.SetBool("inAir", false);
            anim.SetBool("wallJumpL", false);
            anim.SetBool("wallJumpR", false);
            anim.SetBool("secondJump", false);
        }
        
    }

    //==========================================================================================
    //Debug
    private void debugMethod()
    {
        if (!isGrounded)
        {
            Debug.DrawRay(transform.position, transform.up * -1.1f, Color.green);
        }
        Debug.DrawRay(transform.position, transform.right * -2.0f, Color.yellow);
        Debug.DrawRay(transform.position, transform.right * 2.0f, Color.red);
        Debug.DrawRay(transform.position, transform.up * -1.1f, Color.blue);
        if (wallJumpDir == jumpOffDirection.Right)
        {
            Debug.DrawRay(transform.position, transform.right * jumpForce, Color.cyan);
        }
        if (wallJumpDir == jumpOffDirection.Left)
        {
            Debug.DrawRay(transform.position, transform.right * -1 * jumpForce, Color.cyan);
        }


        if (isGrounded && !onWall && !wallRunning)
        {
            anim.SetBool("onGround", true);
            anim.SetBool("inAir", false);
            anim.SetBool("onWall", false);
            //anim.SetBool("wallRunning", false);
            anim.SetBool("wallRunningR", false);
            anim.SetBool("wallRunningL", false);
            anim.SetBool("inAir", false);
            anim.SetBool("wallJump", false);
        }
        if (onWall)
        {
            anim.SetBool("onGround", false);
            anim.SetBool("inAir", false);
            //anim.SetBool("wallRunning", false);
            anim.SetBool("wallRunningR", false);
            anim.SetBool("wallRunningL", false);
            anim.SetBool("onWall", true);
            anim.SetBool("inAir", false);
            anim.SetBool("wallJump", false);
        }
        if (wallRunning)
        {
            anim.SetBool("onGround", false);
            anim.SetBool("inAir", false);
            //anim.SetBool("wallRunning", true);
            anim.SetBool("onWall", false);
            anim.SetBool("inAir", false);
            anim.SetBool("wallJump", false);
        }
        if(wallJump)
        {
            anim.SetBool("onGround", false);
            anim.SetBool("inAir", false);
            //anim.SetBool("wallRunning", false);
            anim.SetBool("wallRunningR", false);
            anim.SetBool("wallRunningL", false);
            anim.SetBool("onWall", false);
            anim.SetBool("inAir", false);
            anim.SetBool("wallJump", true);
        }
        if (!isGrounded && !onWall && !wallRunning && !wallJump) //In air
        {
            anim.SetBool("onGround", false);
            anim.SetBool("inAir", false);
            //anim.SetBool("wallRunning", false);
            anim.SetBool("wallRunningR", false);
            anim.SetBool("wallRunningL", false);
            anim.SetBool("onWall", false);
            anim.SetBool("inAir", true);
            anim.SetBool("wallJump", false);
        }

    }
}
