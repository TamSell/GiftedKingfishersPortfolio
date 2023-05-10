using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class PlayerMomentum1 : MonoBehaviour
{
    [SerializeField] private FinalPlayerController energizer;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedLimit;
    [SerializeField] private float airSpeed;
    [SerializeField] private float sprintSpeed;
    private Vector3 direction;
    private float newMoveSpeed;
    private float prevMoveSpeed;
    private float currentSpeed;
    private float energy;
    public bool inMomentum;
    public bool slowing;

    public void SecondaryMovement()
    {
        direction = energizer.PlayerBody.transform.forward * Input.GetAxis("Vertical") + energizer.PlayerBody.transform.right * Input.GetAxis("Horizontal");
        energy = energizer.currentEnergy;
        MovePlayerDiff();
        determineSpeed();
        EnergyBuildUp();
        SlowDown();
    }

    public void determineSpeed()
    {
        if(Input.GetButtonDown("Run") && energizer.isGrounded)
        {
            newMoveSpeed = energizer.runSpeed * 2;
        }
        else if(!energizer.isGrounded)
        {
            newMoveSpeed = energizer.airSpeed * 2;
        }
        else
        {
            newMoveSpeed = energizer.walkSpeed * 2;
        }

        if(Mathf.Abs(newMoveSpeed-prevMoveSpeed) > 4f && currentSpeed != 0)
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

    private void MovePlayerDiff()
    {
        if (energizer.isGrounded)
        {
            energizer.PlayerBody.AddForce(direction.normalized * currentSpeed * 10f, ForceMode.Force);
        }
        else
        {
            energizer.PlayerBody.AddForce(direction.normalized * currentSpeed * 10f * airSpeed, ForceMode.Force);
        }
    }

    private float EnergyBuildUp()
    {

        if (energy < energizer.energyMax)
        {
            if (energizer.speed > 12)
            {
                energizer.currentEnergy += 3 * Time.deltaTime;
            }
            else if (energizer.speed > 20)
            {
                energizer.currentEnergy += 5 * Time.deltaTime;

            }
            else if (energizer.speed > 40)
            {
                energizer.currentEnergy += 10 * Time.deltaTime;
            }
            else if (energizer.currentEnergy > 0)
            {

                energizer.currentEnergy -= 1 * Time.deltaTime;
            }
        }
        return energy;
    }

    public void MomentumState()
    {
        inMomentum =!inMomentum;
    }

    IEnumerator SlowDown()
    {
        float stop = 0;
        float diff = Mathf.Abs(newMoveSpeed - currentSpeed);
        float startSpeed = currentSpeed;
        slowing = true;

        while (stop < diff)
        {
            currentSpeed = Mathf.Lerp(startSpeed, newMoveSpeed, stop/diff);
            stop += Time.deltaTime * speedMultiplier;
        }

        yield return new Null();
        slowing = false;
        currentSpeed = newMoveSpeed;
    }

    public void Limiters()
    {
        Vector3 currVelocity = new Vector3(energizer.PlayerBody.velocity.x, 0f, energizer.PlayerBody.velocity.z);

        if(currVelocity.magnitude > speedLimit)
        {
            Vector3 engageLimit = currVelocity.normalized * currentSpeed;
            energizer.PlayerBody.velocity = engageLimit;
        }
    }
}
