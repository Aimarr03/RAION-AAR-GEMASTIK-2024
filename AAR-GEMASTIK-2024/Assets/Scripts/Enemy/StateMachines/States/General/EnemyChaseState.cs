using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    private float radius = 3;
    private bool isRotating = false;
    private float rotatingSpeed = 8f;
    private float linearSpeed = 2f;
    private float maxSpeed = 4f;
    private bool isOnRightDirection = true;
    private Vector3 direction;
    private EnemyBaseState nextState;
    public EnemyChaseState(EnemyStateMachine enemyStateMachine, EnemyBase enemy, LayerMask playerLayerMask) : base(enemyStateMachine, enemy, playerLayerMask)
    {

    }
    public void SetNextState(EnemyBaseState nextState) => this.nextState = nextState;

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.headFish.transform.position, radius);
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
        if (Vector3.Distance(playerCoreSystem.transform.position, enemy.transform.position) > 3) return;
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
}
