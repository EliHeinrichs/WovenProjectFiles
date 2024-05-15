using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Transform aimTransform;
    private Vector2 inputDirection;

    private bool playerAttacked = false;
    private bool canAttack = true;
    public float attackSpeed;

   public WeaponScriptableObject Data;

    private Animator weaponAnimator;

    public AudioClip atkAudio;



    void Start()
    {
        aimTransform = transform.Find("Aim");

        weaponAnimator = aimTransform.GetComponentInChildren<Animator>(); //works only if there is one animator in children of player.
        aimTransform . GetComponentInChildren<WeaponController> ().enabled = true;
        
    }

    void Update()
    {
        if(GameManager.Instance.playerIsDead) {
            return;
        }
        attackSpeed = GameManager.Instance.weaponSlot.attackSpeed;

        HandleWeaponRotation();
        HandleAttack();
    }

    private void HandleWeaponRotation() 
    {
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 direction = inputDirection - playerPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        aimTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void HandleAttack() {
        if(playerAttacked) 
        {
            playerAttacked = false;
            canAttack = false;

            switch (GameManager.Instance.weaponSlot.attackType)
            {
                case WeaponType.Stab:
                    weaponAnimator.SetTrigger("Stab");
                    break;

                case WeaponType.Slash:
                    weaponAnimator.SetTrigger("Slash");
                    break;

                case WeaponType.Slam:
                    weaponAnimator.SetTrigger("Slam");
                    break;

                case WeaponType.Shoot:
                    weaponAnimator.SetTrigger("Shoot");

                    // Convert mouse position to world coordinates
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0f;

                    // Calculate direction towards the mouse
                    Vector3 direction = (mousePos - aimTransform.position).normalized;

                    // Spawn projectile
                    GameObject projectile = Instantiate(GameManager.Instance.weaponSlot.projectile, aimTransform.position, Quaternion.identity);

                    // Set velocity towards the mouse direction
                    projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f;
                    break;


            }

            //Start attack cooldown
            StartCoroutine("AttackCooldown");
            SoundManager.Instance.PlayAudio(atkAudio);
        }
    }

    private IEnumerator AttackCooldown() 
    {
        yield return new WaitForSeconds(1/attackSpeed);
        canAttack = true;
    }

    public void GetInputDirection(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void GetAttackInput(InputAction.CallbackContext context) 
    {
        if(context.performed && canAttack && GameManager.Instance.stamina >= 1) {
            playerAttacked = true;
            GameManager.Instance.stamina -= 1;
        }
    }
}