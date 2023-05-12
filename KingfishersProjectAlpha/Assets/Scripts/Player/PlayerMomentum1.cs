using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Search;
using UnityEngine;

public class PlayerMomentum1 : MonoBehaviour
{
    [SerializeField] private FinalPlayerController energizer;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedLimit;
    [SerializeField] private float airSpeed;
    [SerializeField] private float sprintSpeed;
    private float newMoveSpeed;
    private float prevMoveSpeed;
    private float currentSpeed;
    public bool inMomentum;
    public bool slowing;

    public void SecondaryMovement(Vector3 dir)
    {
        determineSpeed();
        SlowDown();
        MovePlayerDiff(dir);
    }

    public void determineSpeed()
    {
        if(!energizer.isGrounded)
        {
            newMoveSpeed = energizer.currAirSpeed;
        }
        else if(energizer.isRunning)
        {
            newMoveSpeed = energizer.currRunSpeed;
        }
        else
        {
            newMoveSpeed = energizer.currWalkSpeed;
        }

        if((energizer.CurrentSpeed > 2 * speedLimit || Mathf.Abs(newMoveSpeed - prevMoveSpeed) > 4f) && currentSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SlowDown());
        }
        else
        {
            currentSpeed = newMoveSpeed;
        }

        newMoveSpeed = prevMoveSpeed;
    }

    private void MovePlayerDiff(Vector3 direction)
    {
        if (energizer.isGrounded)
        {
            energizer.PlayerBody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);
        }
        else
        {
            energizer.PlayerBody.AddForce(direction.normalized * currentSpeed * 10f * energizer.currAirSpeed, ForceMode.Force);
        }
    }

    public void MomentumState()
    {
        inMomentum = !inMomentum;
        MomentumChange();
    }

    public void MomentumChange()
    {
        if (inMomentum)
        {
            energizer.currAirSpeed = energizer.airSpeedMomentum;
            energizer.currRunSpeed= energizer.runSpeedMomentum;
            energizer.currWalkSpeed= energizer.walkSpeedMomentum;
        }
        else
        {
            energizer.currAirSpeed = energizer.airSpeedBase;
            energizer.currRunSpeed = energizer.runSpeedBase;
            energizer.currWalkSpeed = energizer.walkSpeedBase;
        }
    }

    IEnumerator SlowDown()
    {
        float stop = 0;
        float diff = Mathf.Abs(newMoveSpeed - energizer.CurrentSpeed);
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
            Vector3 engageLimit = currVelocity.normalized * energizer.CurrentSpeed;
            energizer.PlayerBody.velocity = new Vector3(engageLimit.x, energizer.PlayerBody.velocity.y,engageLimit.z);
        }
    }


}
