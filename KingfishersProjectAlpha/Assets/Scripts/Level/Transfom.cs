using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.WSA;

public class Transfom : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Door;
    [SerializeField] Transform[] WayPoints;
    [SerializeField] int speed;
    [SerializeField] int UpTime;
    [SerializeField] bool TypeOfDoor;
    [SerializeField] bool Obsticle;

    bool open;
   

    float radius = 500;

    // Update is called once per frame
    void Update()
    {
        if(TypeOfDoor)
        {
            if (open)
            {
                StartCoroutine(Open());
            }
            else
            {
                StartCoroutine(Close());
            }
        }
        if(Obsticle)
        {
            if (open)
            {
                StartCoroutine(Open());
            }
            else
            {
                StartCoroutine(Close());
            }

        }
       
       
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
          
            open = true;
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            open= false;
        }
    }

    private IEnumerator Close()
    {
       
        if (Vector3.Distance(WayPoints[1].transform.position, Door.transform.position) < radius)
        {
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, WayPoints[1].position, Time.deltaTime * speed);
        }
        yield return new WaitForSeconds(UpTime);
      if(Obsticle)
        {
            open = true;
        }

    }
    private IEnumerator Open()
    {
        
        if (Vector3.Distance(WayPoints[0].transform.position , Door.transform.position) < radius)
        {
            Door.transform.position = Vector3.MoveTowards(Door.transform.position, WayPoints[0].position, Time.deltaTime * speed);   
        }
        yield return new WaitForSeconds(UpTime);
        if (Obsticle)
        {
            open = false;
        }
    }

   

}
