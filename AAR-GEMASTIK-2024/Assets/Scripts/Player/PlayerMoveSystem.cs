using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMoveSystem : MonoBehaviour
{
    private PlayerCoreSystem coreSystem;
    private Rigidbody playerRigid;
    
    private Vector2 currentInput;
    public Vector2 NonZeroValueInput;
    private Quaternion targetRotate;
    [SerializeField] private float linearSpeed;
    [SerializeField] private float maxLinearSpeed;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float rotateDegreeLimit;
    [SerializeField] private float maxDistanceUseForOneEnergy;
    private float currentDistanceUse;

    private bool isRotating;
    private bool onRightDirection;
    private bool canBeUsed;

    public event Action OnUseOneEnergy;
    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        playerRigid = GetComponent<Rigidbody>();
        onRightDirection = true;
        canBeUsed = true;
        currentDistanceUse = 0;
    }
    private void FixedUpdate()
    {
        if(coreSystem.isDead) return;
        if (!canBeUsed) return;
        InputHandler();
        HorizontalMove();
        VerticalMove();
        FlipSprite();
    }
    private void InputHandler()
    {
        currentInput = coreSystem.inputSystem.GetMoveInput();
        if(currentInput != Vector2.zero) NonZeroValueInput = currentInput;
    }
    private void HorizontalMove()
    {
        if (isRotating) return;
        Vector2 input = currentInput;
        input.y = 0;

        input = onRightDirection ? input : -input;
        playerRigid.velocity += transform.TransformDirection(input * linearSpeed * Time.fixedDeltaTime);
        playerRigid.velocity = Vector3.ClampMagnitude(playerRigid.velocity, maxLinearSpeed);
        //transform.Translate(input * linearSpeed * Time.fixedDeltaTime);
        currentDistanceUse += playerRigid.velocity.magnitude * Time.fixedDeltaTime;
        if(currentDistanceUse > maxDistanceUseForOneEnergy)
        {
            Debug.Log("One Energy has been used");
            currentDistanceUse = 0;
            OnUseOneEnergy?.Invoke();
        }

        if(playerRigid.velocity.x < 0 && onRightDirection && currentInput.x < 0)
        {
            isRotating = true;
            onRightDirection = false;
            targetRotate = Quaternion.Euler(0, 180, transform.eulerAngles.z);
        }
        else if(playerRigid.velocity.x > 0 && !onRightDirection && currentInput.x > 0)
        {
            isRotating= true;
            onRightDirection = true;
            targetRotate = Quaternion.Euler(0, 0, transform.eulerAngles.z);
        }
    }
    private void VerticalMove()
    {
        if(isRotating) return;
        float z_input = currentInput.y;
        float zRotation = z_input * rotatingSpeed * Time.fixedDeltaTime;

        transform.Rotate(0, 0, zRotation);
        float zValue = transform.rotation.eulerAngles.z;
        if (zValue > 180) zValue -= 360;
        if ((zValue >= rotateDegreeLimit || zValue <= -rotateDegreeLimit))
        {
            transform.Rotate(0, 0, -zRotation);
        }
    }
    private void FlipSprite()
    {
        if(!isRotating) return;
        playerRigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotate, rotatingSpeed);
        if(Quaternion.Angle(transform.rotation, targetRotate) < 5f)
        {
            transform.rotation = targetRotate;
            isRotating = false;
        }
    }
    public void AddSuddenForce(float force) => playerRigid.AddForce(NonZeroValueInput * force, ForceMode.Impulse);
    public void SetCanBeUsed(bool value) => canBeUsed = value;
    public void SetMovement(float linearValue, float rotatingValue)
    {
        linearSpeed = linearValue;
        rotatingSpeed = rotatingValue;
    }
    public void SetMovement(float linearValue)
    {
        linearSpeed = linearValue;
    }
    public bool GetIsOnRightDirection() => onRightDirection;
}
