using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMovement : MonoBehaviour
{
    [SerializeField] private FinalPlayerController player;
    [SerializeField] private float speedMult;
    [SerializeField] private float speedLimit;
    public float newMoveSpeed;
    public float prevMoveSpeed;
    public float currentSpeed;
    public bool inMomentum;
    public bool slowing;

    public void MoveCharacter()
    {
        SlowDown();
        setSpeed();
    }

    public void setSpeed()
    {
        //if(player.isGrounded && player.isRunning)
        //{
        //    newMoveSpeed = player.currRunSpeed;
        //}
        //else if(!player.isGrounded)
        //{
        //    newMoveSpeed = player.currAirSpeed;
        //}
        //else if(player.isGrounded)
        //{
        //    newMoveSpeed = player.currWalkSpeed;
        //}
        //if((player.CurrentSpeed > 2 * speedLimit || Mathf.Abs(newMoveSpeed - prevMoveSpeed) > 4f) && currentSpeed != 0)
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(ChangingSpeeds());
        //}
        //else
        //{
        //    currentSpeed = newMoveSpeed;
        //}

        //prevMoveSpeed = newMoveSpeed;
    }

    private IEnumerator ChangingSpeeds()
    {
        float stop = 0;
        float diff = Mathf.Abs(newMoveSpeed-currentSpeed);
        float startSpeed = currentSpeed;

        while(stop > diff)
        {
            currentSpeed = Mathf.Lerp(startSpeed, newMoveSpeed, stop/diff);
            stop += Time.deltaTime * speedMult;
            yield return null;
        }

        currentSpeed = newMoveSpeed;
    }

    public void MoveThem()
    {
        player.PlayerBody.AddForce(player.PlayerMovementInput.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    private void SlowDown()
    {
        Vector3 slowVel = new Vector3(player.PlayerBody.velocity.x, 0f, player.PlayerBody.velocity.y);
        if(slowVel.magnitude > currentSpeed)
        {
            Vector3 limits = slowVel.normalized * currentSpeed;
            player.PlayerBody.velocity = new Vector3(limits.x, player.PlayerBody.velocity.y, limits.z);
        }
    }

    public void ChangeSpeedStates()
    {
        if (inMomentum)
        {
            //player.currAirSpeed = player.airSpeedMomentum;
            //player.currRunSpeed = player.runSpeedMomentum;
            //player.currWalkSpeed = player.walkSpeedMomentum;
        }
        else
        {
            //player.currAirSpeed = player.airSpeedBase;
            //player.currRunSpeed = player.runSpeedBase;
            //player.currWalkSpeed = player.walkSpeedBase;
        }
    }

    public void ChangeState()
    {
        inMomentum = !inMomentum;
        ChangeSpeedStates();
    }
}
