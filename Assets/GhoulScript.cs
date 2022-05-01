using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhoulScript : MonoBehaviour
{

    [SerializeField]
    Transform target;
    [SerializeField]
    NavMeshAgent agent;
    float swoopCoolDown = 10f;

    bool canSwoop = true;
    bool swooping = false;

    [SerializeField]
    GameObject model;

    Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(new Vector3(target.position.x, 0,target.position.z));
        destination = new Vector3(target.position.x, 0, target.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) < 10f && canSwoop)
        {
            agent.SetDestination(new Vector3(target.position.x, 0, target.position.z));
            destination = new Vector3(target.position.x, 0, target.position.z);
            StartCoroutine("swoop");
        }
        if (swooping == false)
        {
            if (Vector3.Distance(destination,transform.position)<0.5f)
            {
                destination = new Vector3(Random.Range(-4, 4)+ target.position.x, 0, Random.Range(-4, 4)+target.position.z);

                agent.SetDestination(destination);
                Debug.Log("fucking thang");
            }
            //if (Vector3.Distance(target.position, transform.position) > 10f)
            //{

            //    agent.SetDestination(target.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4)));
            //    destination = target.position;
            //}
        }
    }

    IEnumerator swoop()
    {
        swooping = true;
        StartCoroutine("coolDown");
        agent.speed = 7f;
        LeanTween.moveY(model,target.position.y,1f);
        yield return new WaitForSeconds(2);
        LeanTween.moveLocalY(model, 4, 2);
        agent.speed = 3.5f;
        swooping = false;

    }


    IEnumerator coolDown()
    {
        canSwoop = false;
        yield return new WaitForSeconds(swoopCoolDown);
        canSwoop = true;

    }

}
