using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("----- Components -----")]

    [Header("-- Stats --")]
    int hitPoints;
    int moveSpeed;

    [Header("-- Variables --")]
    int viewAngle;
    Vector3 identVec;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FindPlayer()
    {
        
    }
    void BlastPlayer()
    {
        
    }

    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(identVec.x, 0, identVec.z));
    }

}
