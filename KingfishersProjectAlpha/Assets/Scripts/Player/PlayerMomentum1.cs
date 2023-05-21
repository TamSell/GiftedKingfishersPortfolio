using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMomentum1 : MonoBehaviour
{
    [SerializeField] private FinalPlayerController energizer;
    [SerializeField] private float speedMultiplier;
    [SerializeField] public float speedLimit;
    [SerializeField] public float maxSlopeAngle;
    private RaycastHit hit;
    private Vector3 direction;
    private float newMoveSpeed;
    private float prevMoveSpeed;
    public float currentSpeed;
    public bool inMomentum;
    public float slideMult;
    private bool slowing;
    private bool slideDown;

    public void SetUp()
    {
        Limiters();
        determineSpeed();
    }

    public void determineSpeed()
    {
        if(energizer.isDashing)
        {
            newMoveSpeed = energizer.DashSpeed;
        }
        else if (energizer.isRunning && energizer.isGrounded)
        {
            newMoveSpeed = energizer.currRunSpeed;
        }
        else if (!energizer.isGrounded)
        {
            newMoveSpeed = energizer.currAirSpeed;
        }
        else
        {
            newMoveSpeed = energizer.currWalkSpeed;
        }

        if (Mathf.Abs(newMoveSpeed - prevMoveSpeed) > 4f && currentSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SlowDown());
        }
        else
        {
            currentSpeed = newMoveSpeed;
        }
        prevMoveSpeed = newMoveSpeed;
    }

    public void MovePlayerDiff()
    {
        direction = energizer.PlayerMovementInput;
        if(OnSlope() && !energizer.exitSlope && energizer.PlayerBody.velocity.y < -0.1f)
        {
            energizer.PlayerBody.AddForce(SlopeMoveAngle(direction) * currentSpeed * 10f * slideMult, ForceMode.Force);
        }
        else if(OnSlope() && !energizer.exitSlope)
        {
            energizer.PlayerBody.AddForce(SlopeMoveAngle(direction) * currentSpeed * 10f, ForceMode.Force);
        }
        else
        {
            energizer.PlayerBody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);
        }
    }

    public void MomentumState()
    {
        inMomentum = !inMomentum;
        gameManager.Instance.MomentumOverlay.enabled = inMomentum;
    }

    IEnumerator SlowDown()
    {
        float stop = 0;
        float diff = Mathf.Abs(newMoveSpeed - currentSpeed);
        float startSpeed = currentSpeed;
        slowing = true;

        while (stop < diff)
        {
            currentSpeed = Mathf.Lerp(startSpeed, newMoveSpeed, stop / diff);
            stop += Time.deltaTime * speedMultiplier;
        }

        yield return new Null();
        slowing = false;
        currentSpeed = newMoveSpeed;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            energizer.isGrounded = true;
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            if(angle < maxSlopeAngle && angle != 0)
                return true;
        }

        return false;
    }

    public Vector3 SlopeMoveAngle(Vector3 director)
    {
        return Vector3.ProjectOnPlane(director,hit.normal).normalized;
    }

    public void Limiters()
    {
        if(OnSlope() && !energizer.exitSlope)
        {
            if(energizer.PlayerBody.velocity.magnitude > speedLimit)
            {
                energizer.PlayerBody.velocity = energizer.PlayerBody.velocity.normalized * speedLimit;
            }
        }
        else
        {
            Vector3 currVelocity = new Vector3(energizer.PlayerBody.velocity.x, 0f, energizer.PlayerBody.velocity.z);
            if (currVelocity.magnitude > speedLimit && !energizer.isDashing)
            {
                Vector3 engageLimit = currVelocity.normalized * speedLimit;
                energizer.PlayerBody.velocity = new Vector3(engageLimit.x, energizer.PlayerBody.velocity.y, engageLimit.z);
            }
            else if (!energizer.DashReady)
            {
                Vector3 engageLimit = currVelocity.normalized * (speedLimit * 0.5f);
                energizer.PlayerBody.velocity = new Vector3(engageLimit.x, energizer.PlayerBody.velocity.y, engageLimit.z);
            }
        }
    }
}