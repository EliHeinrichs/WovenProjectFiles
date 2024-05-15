using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Require dependencies for the object
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{






    public Item[] item;


    private Rigidbody2D rb;
    public Animator animator;


    private float horizontal;
    private float vertical;

    public GameObject uiObj;


    private Vector2 moveDirection;

    private bool canDash = true;
    private bool performedDash = false;

    private Vector2 dashForce;

    public Data_PlayerMovement Data;

    public GameObject playerSprite;
    public AudioClip dashAudio;
   
    [SerializeField]
    private float moveSpeed;


    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
 

  
    }


    void Start()
    {
        moveSpeed = Data.moveSpeed + GameManager.Instance.moveSpeed;

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        GameManager.Instance.currentSummons = 0;
    }


    void Update() {
        if(GameManager.Instance.playerIsDead) {
            rb.velocity = new Vector2(0,0);
            return;
        }
        
 

        if(horizontal != 0 || vertical != 0) 
        {
            animator.SetBool("isMoving", true);
            SoundManager.Instance.WalkAudio(true);
           
        } else 
        {
            animator.SetBool("isMoving", false);
            SoundManager.Instance.WalkAudio(false);

        }

   

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }



    void FixedUpdate () 
    {
        if(GameManager.Instance.playerIsDead) {
            return;
        }
  
        HandleMovement();
        HandleDash();
    }

    public void GetMovementValue(InputAction.CallbackContext context) 
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
  

    }

    public void DevKeyInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ItemWorld.SpawnItemWorld(transform.position, item[Random.Range(0, item.Length)]);
        }
    }

    public void GetInventoryInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            uiObj.SetActive(!uiObj.activeInHierarchy);
        }
    }

    public void GetDashInput(InputAction.CallbackContext context) 
    {
        if (context.performed && canDash && GameManager.Instance.stamina >= 5) 
        {
            GameManager.Instance.stamina -= 5;
           performedDash = true;
            SoundManager.Instance.PlayAudio(dashAudio);
        }
    }

    private void HandleMovement() 
    {
        float targetSpeedX = horizontal * moveSpeed;
        float targetSpeedY = vertical * moveSpeed;

        #region Calculate AccelRate
        float accelRateX;
        float accelRateY;

        // Gets an acceleration value based on if we are accelerating or trying to decelerate.
        accelRateX = (Mathf.Abs(targetSpeedX) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        accelRateY = (Mathf.Abs(targetSpeedY) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        #endregion

        // Calculate difference between current velocity and desired velocity
        float speedDifX = targetSpeedX - rb.velocity.x;
        float speedDifY = targetSpeedY - rb.velocity.y;

        // Calculate force along x-axis and y-axis to apply to the player
        float movementX = speedDifX * accelRateX;
        float movementY = speedDifY * accelRateY;

        rb.AddForce(new Vector2(movementX, movementY), ForceMode2D.Force);
    }

    private void HandleDash() 
    {
        // When you perform a dash, add the correct force to dashForce according to the player direction.
        if (performedDash) 
        {
            Debug.Log("dashing");
            canDash = false;
            performedDash = false;

            // Use the player's current velocity direction as the dash force direction
            Vector2 dashDirection = rb.velocity.normalized;
            dashForce = dashDirection * Data.dashPower;

            StartCoroutine("DashCooldown");
        }

        // Add the dash force to the player
        rb.AddForce(dashForce, ForceMode2D.Force);
    }

    private IEnumerator DashCooldown() 
    {
        yield return new WaitForSeconds(Data.dashLength);
        dashForce = Vector2.zero;
        yield return new WaitForSeconds(Data.dashCooldown);
        canDash = true;
    }

   





}