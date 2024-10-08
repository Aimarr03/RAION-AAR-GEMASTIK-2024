using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveSystem : MonoBehaviour
{
    private PlayerCoreSystem coreSystem;
    private Rigidbody2D playerRigid;
    
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
        coreSystem.OnDead += CoreSystem_OnDead;
        coreSystem.OnDisabled += CoreSystem_OnDisabled;
        WeightSystem.OnOverweight += WeightSystem_OnOverweight;
        ExpedictionManager.Instance.OnDoneExpediction += Instance_OnDoneExpediction;
        DialogueEditor.ConversationManager.OnConversationStarted += OnConverstaionStarted;
        DialogueEditor.ConversationManager.OnConversationEnded += OnConverstaionFinished;
    }

    

    private void OnDisable()
    {
        PlayerInputSystem.InvokeMoveSoundAction -= PlayerInputSystem_InvokeMoveSoundAction;
        coreSystem.OnDisabled -= CoreSystem_OnDisabled;
        WeightSystem.OnOverweight -= WeightSystem_OnOverweight;
        ExpedictionManager.Instance.OnDoneExpediction -= Instance_OnDoneExpediction;
        coreSystem.OnDead -= CoreSystem_OnDead;
        DialogueEditor.ConversationManager.OnConversationStarted -= OnConverstaionStarted;
        DialogueEditor.ConversationManager.OnConversationEnded -= OnConverstaionFinished;
    }

    private void Instance_OnDoneExpediction(bool obj, PlayerCoreSystem coreSystem)
    {
        playerRigid.velocity = Vector3.zero;
    }
    private async void CoreSystem_OnDisabled(bool obj)
    {
        if (obj) return;
        playerRigid.velocity = Vector3.zero;
        float y_value = onRightDirection ? 0 : 180;
        Quaternion targetRotaion = Quaternion.Euler(0f, y_value, 0f);
        canBeUsed = false;
        await Task.Delay(800);
        while (Quaternion.Angle(transform.rotation, targetRotaion) > 0.05)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotaion, rotatingSpeedOnYAxis);
            await Task.Yield();
        }
        await Task.Delay(300);
        transform.rotation = targetRotaion;
        ResetPlayerOrientation();
        Debug.Log(transform.rotation);
        canBeUsed = true;
    }
    private void ResetPlayerOrientation()
    {
        playerRigid.velocity = Vector3.zero;
        playerRigid.angularVelocity = 0;
        transform.rotation = Quaternion.Euler(0f, onRightDirection ? 0f : 180f, 0f);
        //Debug.Log("Player orientation reset.");
    }
    private void PlayerInputSystem_InvokeMoveSoundAction(bool obj)
    {
        //Debug.Log("Is Attempt Moving " + obj);
        if(obj) OnStartDriveSound();
        else OnStopDrivingSound();
    }

    private void Awake()
    {
        coreSystem = GetComponent<PlayerCoreSystem>();
        playerRigid = GetComponent<Rigidbody2D>();
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
            //Debug.Log("One Energy has been used");
            currentDistanceUse = 0;
            OnUseOneEnergy?.Invoke();
        }
        if (playerRigid.velocity.x < 0 && onRightDirection && currentInput.x < 0)
        {
            onRightDirection = false;
            targetRotate = Quaternion.Euler(0, 180, transform.localRotation.eulerAngles.z);
        }
        else if (playerRigid.velocity.x > 0 && !onRightDirection && currentInput.x > 0)
        {
            onRightDirection = true;
            targetRotate = Quaternion.Euler(0, 0, transform.localRotation.eulerAngles.z);
        }
        if(playerRigid.velocity.x < 1.5f && playerRigid.velocity.x > -1.5f) transform.rotation = targetRotate;
    }
    #region HorizontalLogic
    private void HorizontalMove()
    {
        Vector2 input = currentInput;
        if(InvokeHorizontalBrake(input)) return;
        input.y = 0;
        Vector2 outputVelocity = linearSpeed * Time.fixedDeltaTime * input;
        if (isSlowed)
        {
            outputVelocity = SlowAction(outputVelocity, slowedMultiplier);
        }
        //Debug.Log("Output Velocity "+outputVelocity);
        playerRigid.velocity += outputVelocity;
        playerRigid.velocity = Vector2.ClampMagnitude(playerRigid.velocity, maxLinearSpeed);
        OnSettingAudioVolume();
        //transform.Translate(input * linearSpeed * Time.fixedDeltaTime);
        onMoving?.Invoke(playerRigid.velocity);
    }
    private bool InvokeHorizontalBrake(Vector2 input)
    {
        if (playerRigid.velocity.x < linearSpeed && playerRigid.velocity.x > -linearSpeed) return false;
        if (input.x == 0) return false;
        bool checkInput = (input.x > 0 && playerRigid.velocity.x < 0) || (input.x < 0 && playerRigid.velocity.x > 0);
        if (checkInput)
        {
            Debug.Log("On Brake!");
            playerRigid.velocity /= 1.5f;
        }
        return true;
    }
    #endregion
    #region VerticalLogic
    private void VerticalMove()
    {
        float y_input = currentInput.y;
        if(y_input != 0)
        {
            InvokeVerticalBrake(new Vector2(0, y_input));
            Vector2 outputVelocity = Vector3.up * linearSpeed * Time.fixedDeltaTime * y_input;
            playerRigid.velocity += outputVelocity;
        }
    }
    private void OnRotatingOnZAxis(float zInput)
    {
        // Calculate the rotation amount
        float zRotation = zInput * rotatingSpeedOnZAxis * Time.fixedDeltaTime;

        // Get the current global rotation on the Z axis
        float currentZRotation = transform.eulerAngles.z;

        // Calculate the new rotation on the global Z axis
        float newZRotation = currentZRotation + zRotation;

        // Clamp the rotation to the desired limits
        if (newZRotation > 180) newZRotation -= 360;
        if (newZRotation < -180) newZRotation += 360;
        newZRotation = Mathf.Clamp(newZRotation, -rotateDegreeLimit, rotateDegreeLimit);

        // Apply the new rotation
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);
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
    public void AddSuddenForce(float force) => playerRigid.AddForce(NonZeroValueInput * force, ForceMode2D.Impulse);
    public void AddSuddenForce(Vector3 direction, float force) => playerRigid.AddForce(direction * force, ForceMode2D.Impulse);

    public void AddSuddenForce(Vector3 direction, float force, ForceMode2D mode) => playerRigid.AddForce(direction * force, mode);
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
    private void CoreSystem_OnDead()
    {
        OnDead();
    }
    private async void OnDead()
    {
        playerRigid.velocity = new Vector3(0.2f, 0.2f, 0.2f);
        await Task.Delay(800);
        playerRigid.velocity = Vector3.zero;
    }
    private void OnConverstaionFinished()
    {
        playerRigid.velocity = Vector2.zero;
        canBeUsed = true;
    }
    private void OnConverstaionStarted()
    {
        playerRigid.velocity = Vector2.zero;
        canBeUsed = false;
    }
    public void Stop()
    {
        playerRigid.velocity = Vector2.zero;
    }
}
