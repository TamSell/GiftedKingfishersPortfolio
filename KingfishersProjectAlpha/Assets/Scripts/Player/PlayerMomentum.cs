using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum speeds
{
    Walk,
    Run,
    Slide,
    Infinite
}
public class PlayerMomentum : MonoBehaviour
{
    [Header("---Speeds---")]
    [SerializeField] float acceleration = Time.deltaTime;
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float slideSpeed;

    [Header("---Player---")]
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerController player;

    private bool momentumState;
    private float currentSpeed;

    private void Update()
    {
        if(Input.GetKeyDown("Momentum"))
        {
        }
    }

    private void Speed()
    {
        rb.AddForce(player.transform.forward * acceleration, ForceMode.Force);
    }



}
