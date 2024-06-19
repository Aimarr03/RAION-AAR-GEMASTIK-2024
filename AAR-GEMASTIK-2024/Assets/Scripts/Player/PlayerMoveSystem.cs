using System;
using Unity.VisualScripting;
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
    [SerializeField] private float rotatingSpeedOnZAxis;
    [SerializeField] private float rotatingSpeedOnYAxis;
    [SerializeField] private float rotateDegreeLimit;
    [SerializeField] private float maxDistanceUseForOneEnergy;
    [SerializeField] private float minHorizontalMovement;
    private float currentDistanceUse;

    private bool isRotating;
    private bool onRightDirection;
    private bool canBeUsed;
    private bool isSlowed;
    private float slowedMultiplier;

    [SerializeField] private AudioClip OnDrive;
    [SerializeField] private AudioClip OffDrive;
    [SerializeField] private AudioSource OnDriveSource;

    public static event Action<Vector3> onMoving;

    public event Action OnUseOneEnergy;
    private void Start()
    {
        PlayerInputSystem.InvokeMoveSoundAction += PlayerInputSystem_InvokeMoveSoundAction;
        WeightSystem.OnOverweight += WeightSystem_OnOverweight;
        ExpedictionManager.Instance.OnDoneExpediction += Instance_OnDoneExpediction;
    }

    

    private void OnDisable()
    {
        PlayerInputSystem.InvokeMoveSoundAction -= PlayerInputSystem_InvokeMoveSoundAction;
        WeightSystem.OnOverweight -= WeightSystem_OnOverweight;
        ExpedictionManager.Instance.OnDoneExpediction -= Instance_OnDoneExpediction;
    }

    private void Instance_OnDoneExpediction(bool obj, PlayerCoreSystem coreSystem)
    {
        playerRigid.velocity = Vector3.zero;
    }

    private void PlayerInputSystem_InvokeMoveSoundAction(bool obj)
    {
        Debug.Log("Is Attempt Moving " + obj);
        if(obj) OnStartDriveSound();
        else OnStopDrivingSound();
    }

    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        playerRigid = GetComponent<Rigidbody>();
        onRightDirection = true;
        canBeUsed = true;
        isSlowed = false;
        currentDistanceUse = 0;
    }
    
    private void FixedUpdate()
    {
        if(coreSystem.isDead) return;
        if (coreSystem.onDisabled) return;
        if (!canBeUsed) return;
        InputHandler();
        HorizontalMove();
        VerticalMove();
        CheckMovementStatus();
        FlipSprite();
        
    }
    private void InputHandler()
    {
        currentInput = coreSystem.inputSystem.GetMoveInput();
        if(currentInput != Vector2.zero) NonZeroValueInput = currentInput;
    }
    private void CheckMovementStatus()
    {
        currentDistanceUse += playerRigid.velocity.magnitude * Time.fixedDeltaTime;
        if (currentDistanceUse > maxDistanceUseForOneEnergy)
        {
            Debug.Log("One Energy has been used");
            currentDistanceUse = 0;
            OnUseOneEnergy?.Invoke();
        }

        if (playerRigid.velocity.x < 0 && onRightDirection && currentInput.x < 0)
        {
            isRotating = true;
            onRightDirection = false;
            targetRotate = Quaternion.Euler(0, 180, transform.eulerAngles.z);
        }
        else if (playerRigid.velocity.x > 0 && !onRightDirection && currentInput.x > 0)
        {
            isRotating = true;
            onRightDirection = true;
            targetRotate = Quaternion.Euler(0, 0, transform.eulerAngles.z);
        }
    }
    #region HorizontalLogic
    private void HorizontalMove()
    {
        if (isRotating) return;
        Vector2 input = currentInput;
        InvokeHorizontalBrake(input);
        input.y = 0;
        //Debug.Log(input);
        input = onRightDirection ? input : -input;
        Vector3 outputVelocity = transform.TransformDirection(Vector3.right * (linearSpeed * Time.fixedDeltaTime * input));
        if (isSlowed)
        {
            outputVelocity = SlowAction(outputVelocity, slowedMultiplier);
        }
        playerRigid.velocity += outputVelocity;
        playerRigid.velocity = Vector3.ClampMagnitude(playerRigid.velocity, maxLinearSpeed);
        OnSettingAudioVolume();
        //transform.Translate(input * linearSpeed * Time.fixedDeltaTime);
        onMoving?.Invoke(playerRigid.velocity);
    }
    private void InvokeHorizontalBrake(Vector2 input)
    {
        if (playerRigid.velocity.x < linearSpeed && playerRigid.velocity.x > -linearSpeed) return;
        if (input.x == 0) return;
        bool checkInput = (input.x > 0 && playerRigid.velocity.x < 0) || (input.x < 0 && playerRigid.velocity.x > 0);
        if (checkInput)
        {
            Debug.Log("On Brake!");
            playerRigid.velocity /= 1.5f;
        }
    }
    #endregion
    #region VerticalLogic
    private void VerticalMove()
    {
        if(isRotating) return;
        float z_input = currentInput.y;
        if(z_input != 0)
        {
            InvokeVerticalBrake(new Vector2(0, z_input));
            OnRotatingOnZAxis(z_input);
            if (playerRigid.velocity.x < minHorizontalMovement && playerRigid.velocity.x > -minHorizontalMovement) return;
            Vector3 outputVelocity = Vector3.up * ((linearSpeed * 0.66f) * Time.fixedDeltaTime * z_input);
            playerRigid.velocity += outputVelocity;
        }
        else
        {
            Quaternion targetRotation = onRightDirection ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotatingSpeedOnZAxis);
        }
    }
    private void OnRotatingOnZAxis(float zInput)
    {
        float zRotation = zInput * rotatingSpeedOnZAxis * Time.fixedDeltaTime;

        transform.Rotate(0, 0, zRotation);
        float zValue = transform.rotation.eulerAngles.z;
        if (zValue > 180) zValue -= 360;
        if ((zValue >= rotateDegreeLimit || zValue <= -rotateDegreeLimit))
        {
            transform.Rotate(0, 0, -zRotation);
        }
    }
    private void InvokeVerticalBrake(Vector2 input)
    {
        if (playerRigid.velocity.x < linearSpeed && playerRigid.velocity.x > -linearSpeed) return;
        if (input.x == 0) return;
        bool checkInput = (input.x > 0 && playerRigid.velocity.x < 0) || (input.x < 0 && playerRigid.velocity.x > 0);
        if (checkInput)
        {
            Debug.Log("On Brake!");
            playerRigid.velocity /= 1.5f;
        }
    }
    #endregion
    private void FlipSprite()
    {
        if(!isRotating) return;
        playerRigid.velocity = Vector3.zero;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotate, rotatingSpeedOnYAxis);
        if(Quaternion.Angle(transform.rotation, targetRotate) < 5f)
        {
            transform.rotation = targetRotate;
            isRotating = false;
        }
    }
    public void AddSuddenForce(float force) => playerRigid.AddForce(NonZeroValueInput * force, ForceMode.Impulse);
    public void AddSuddenForce(Vector3 direction, float force) => playerRigid.AddForce(direction * force, ForceMode.Impulse);

    public void AddSuddenForce(Vector3 direction, float force, ForceMode mode) => playerRigid.AddForce(direction * force, mode);
    public void SetCanBeUsed(bool value) => canBeUsed = value;
    public void SetMovement(float linearValue, float rotatingValue)
    {
        linearSpeed = linearValue;
        rotatingSpeedOnZAxis = rotatingValue;
    }
    public void GetMovement(out float linearValue, out float rotatingValue)
    {
        linearValue = linearSpeed;
        rotatingValue = rotatingSpeedOnZAxis;
    }
    public void SetMovement(float linearValue)
    {
        linearSpeed = linearValue;
    }
    public bool GetIsOnRightDirection() => onRightDirection;
    public void SetIsSlowed(bool input, float slowMultiplier)
    {
        isSlowed = input;
        playerRigid.velocity = isSlowed? playerRigid.velocity * slowMultiplier : playerRigid.velocity;
        slowedMultiplier = slowMultiplier;
    }
    private Vector3 SlowAction(Vector3 velocityInput, float slowMultiplier)
    {
        return velocityInput *= slowMultiplier;
    }
    private void OnStartDriveSound()
    {
        OnDriveSource.Stop();
        OnDriveSource.clip = OnDrive;
        OnDriveSource.loop = true;
        OnDriveSource.Play();
    }
    private void OnStopDrivingSound()
    {
        OnDriveSource.Stop();
        OnDriveSource.loop = false;
        OnDriveSource.clip = OffDrive;
        OnDriveSource.Play();
    }
    private void OnSettingAudioVolume()
    {
        float volume = (Mathf.Abs(playerRigid.velocity.x) / maxLinearSpeed);
        OnDriveSource.volume = Mathf.Clamp(volume, 0.1f, 0.5f);
    }
    private void WeightSystem_OnOverweight(bool isOverweight, float arg2)
    {
        if(isOverweight)
        {
            slowedMultiplier = arg2;
        }
        else
        {
            slowedMultiplier = 1;
        }
        isSlowed = isOverweight;
    }
}
