using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMoveSystem : MonoBehaviour
{
    private PlayerCoreSystem coreSystem;
    private Rigidbody playerRigid;
    
    private Vector2 currentInput;
    private Quaternion targetRotate;
    [SerializeField] private float speed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float maxSpeed;

    private bool isRotating;
    private bool onRightDirection;
    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        playerRigid = GetComponent<Rigidbody>();
        onRightDirection = true;
    }
    private void FixedUpdate()
    {
        HorizontalMove();
        FlipSprite();
    }
    private void HorizontalMove()
    {
        Vector2 input = coreSystem.inputSystem.GetMoveInput();
        input.y = 0;
        
        Vector3 acceleration = speed * input;
        playerRigid.velocity += acceleration * Time.fixedDeltaTime;
        playerRigid.velocity = Vector3.ClampMagnitude(playerRigid.velocity, maxSpeed);
        if(playerRigid.velocity.x < 0 && onRightDirection)
        {
            isRotating = true;
            onRightDirection = false;
            targetRotate = Quaternion.Euler(0, 180, 0);
        }
        else if(playerRigid.velocity.x > 0 && !onRightDirection)
        {
            isRotating= true;
            onRightDirection = true;
            targetRotate = Quaternion.Euler(0, 0, 0);
        }
    }
    private void FlipSprite()
    {
        if(!isRotating) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotate, rotatingSpeed * Time.fixedDeltaTime);
        if(Quaternion.Angle(transform.rotation, targetRotate) < 0.1f)
        {
            isRotating = false;
        }
    }

}
