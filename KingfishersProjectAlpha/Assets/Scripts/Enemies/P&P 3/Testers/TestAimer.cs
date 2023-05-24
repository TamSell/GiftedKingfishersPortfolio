using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAimer : MonoBehaviour
{
    private float bulletSpeed = 100;
    private Vector3 PlayerVel;
    private Vector3 lookVector;
    Rigidbody playerBody;
    
    // Start is called before the first frame update
    void Start()
    {
        playerBody = gameManager.Instance.playerController.PlayerBody;
    }

    // Update is called once per frame
    void Update()
    {
        lookVector = gameManager.Instance.playerController.PlayerBody.transform.position - transform.position;
        Debug.DrawRay(transform.position, PredictedPosition());
        FaceThePlayer();   
    }

    void FaceThePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(PredictedPosition());
        transform.rotation =  rot;
    }

    protected Vector3 PredictedPosition()
    {
        PlayerVel = playerBody.velocity;

        float a = Vector3.Dot(PlayerVel, PlayerVel) - (bulletSpeed * bulletSpeed);
        float b = 2.0f * Vector3.Dot(PlayerVel, lookVector);
        float c = Vector3.Dot(lookVector, lookVector);

        float p = -b / (2 * a);
        float q = Mathf.Sqrt((b * b) - 4 * a * c) / (2 * a);

        float time1 = p - q;
        float time2 = p + q;
        float timeActual = time1 > time2 && time2 > 0 ? time2 : time1;

        return lookVector + PlayerVel * timeActual;
    }
}
