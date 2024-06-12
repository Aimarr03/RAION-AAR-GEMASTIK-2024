using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    private float radius = 3;
    private bool isRotating = false;
    private float rotatingSpeed = 8f;
    private float linearSpeed = 2f;
    private float maxSpeed = 4f;
    private bool isOnRightDirection = true;
    private float distanceAggro;
    private Vector3 direction;
    private EnemyBaseState nextState;
    private Transform centerCheckDistance;
    public EnemyChaseState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask, 
        float linearSpeed, float rotatingSpeed, float maxSpeed, float distanceAggro, Transform centerCheckDistance) : base(enemyStateMachine, enemy, playerLayerMask)
    {
        this.linearSpeed = linearSpeed;
        this.rotatingSpeed = rotatingSpeed;
        this.maxSpeed = maxSpeed;
        this.distanceAggro = distanceAggro;
        this.centerCheckDistance = centerCheckDistance;
    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centerCheckDistance.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerCheckDistance.position, distanceAggro);
    }

    public override void OnEnterState()
    {
        
    }

    public override void OnExitState()
    {
        
    }

    public override void OnUpdateState()
    {
        CheckPlayer();
        WithinBiteRange();
    }
    private void CheckPlayer()
    {
        if (playerCoreSystem == null) return;
        direction = (playerCoreSystem.transform.position - enemy.transform.position).normalized;
        HorizontalMove(direction);
        VerticalMovement(direction);
    }
    private void WithinBiteRange()
    {
        //Debug.Log(Vector3.Distance(playerCoreSystem.transform.position, centerCheckDistance.position));
        //Debug.Log((Vector3.Distance(playerCoreSystem.transform.position, centerCheckDistance.position) > distanceAggro) + " Aggro Within Range");
        if (Vector3.Distance(playerCoreSystem.transform.position, enemy.transform.position) > distanceAggro) return;
        Debug.Log("Engage Biting");
        nextState.SetPlayerCoreSystem(playerCoreSystem);
        enemyStateMachine.OnTransitionState(nextState);
    }
    private void HorizontalMove(Vector3 direction)
    {
        if (isRotating)
        {
            RotatingOnYAxis();
            return;
        }
        if (direction.x == 0) return;
        float x_direction = direction.x > 0 ? 1 : -1;
        if (CheckHasToRotate(x_direction))
        {
            isRotating = true;
            return;
        }

        enemy.rigidBody.velocity += Vector3.right * linearSpeed * Time.fixedDeltaTime * x_direction;
        enemy.rigidBody.velocity = Vector3.ClampMagnitude(enemy.rigidBody.velocity, maxSpeed);
    }
    private bool CheckHasToRotate(float x)
    {
        if (isOnRightDirection && x < 0) return true;
        if (!isOnRightDirection && x > 0) return true;
        else return false;
    }
    private void RotatingOnYAxis()
    {
        if (!isRotating) return;
        enemy.rigidBody.velocity = Vector3.zero;
        float y_axis_target = isOnRightDirection ? 180 : 0;
        Quaternion targetRotate = Quaternion.Euler(0, y_axis_target, 0);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, targetRotate, rotatingSpeed * Time.fixedDeltaTime);
        if (Quaternion.Angle(enemy.transform.rotation, targetRotate) < 5f)
        {
            enemy.transform.rotation = targetRotate;
            isRotating = false;
            isOnRightDirection = !isOnRightDirection;
            Debug.Log(isRotating);
        }
    }
    private void VerticalMovement(Vector3 direction)
    {
        if (isRotating) return;
        float z_input = direction.y;
        if (z_input != 0)
        {
            Vector3 outputVelocity = Vector3.up * (linearSpeed/4 * Time.fixedDeltaTime * z_input);
            enemy.rigidBody.velocity += outputVelocity;
        }
    }
    public float GetAggroDistance() => distanceAggro;
}
