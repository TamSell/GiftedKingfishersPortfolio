using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMomentum1 : MonoBehaviour
{
    [SerializeField] private FinalPlayerController energizer;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedLimit;
    private Vector3 direction;
    private float newMoveSpeed;
    private float prevMoveSpeed;
    public float currentSpeed;
    public bool inMomentum;
    public bool slowing;

    public void SetUp()
    {
        Limiters();
        determineSpeed();
    }

    public void determineSpeed()
    {
        if (energizer.isRunning && energizer.isGrounded)
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
        energizer.PlayerBody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    public void MomentumState()
    {
        inMomentum = !inMomentum;
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

    public void Limiters()
    {
        Vector3 currVelocity = new Vector3(energizer.PlayerBody.velocity.x, 0f, energizer.PlayerBody.velocity.z);

        if (currVelocity.magnitude > speedLimit)
        {
            Vector3 engageLimit = currVelocity.normalized * speedLimit;
            energizer.PlayerBody.velocity = new Vector3(engageLimit.x, energizer.PlayerBody.velocity.y, engageLimit.z);
        }
    }
}