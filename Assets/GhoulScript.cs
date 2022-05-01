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
        target = GameObject.Find("Player(Clone)").transform;

        //agent.SetDestination(new Vector3(target.position.x, 0, target.position.z));
        destination =FindPos(target.position);

        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) < 10f && canSwoop)
        {
            //agent.SetDestination(new Vector3(target.position.x, 0, target.position.z));
            destination = FindPos(target.position);
            agent.SetDestination(destination);
            StartCoroutine("swoop");
        }
        if (swooping == false)
        {
            if (Vector3.Distance(destination,transform.position)<0.5f)
            {

                destination = FindPos(new Vector3(Random.Range(-4, 4) + target.position.x, target.position.y, Random.Range(-4, 4) + target.position.z));
               // destination = new Vector3(Random.Range(-4, 4)+ target.position.x, 0, Random.Range(-4, 4)+target.position.z);

                agent.SetDestination(destination);
            }
            //if (Vector3.Distance(target.position, transform.position)d > 10f)
            //{

            //    agent.SetDestination(target.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4)));
            //    destination = target.position;
            //}
        }
    }
    Vector3 FindPos(Vector3 pos)
    {
        Vector3 ItemPosition = new Vector3();
        bool ready = false;
        int x = 0;
        while (!ready)
        {
            Vector3 topPosition = (Vector3)Random.insideUnitCircle*3 + pos;
            topPosition = new Vector3(topPosition.x, 10, topPosition.z);
            Vector3 placePosition = UtilityManager.Instance.ShootRayCastDown(topPosition);
            if (placePosition != Vector3.zero)
            {
                ready = true;
                ItemPosition = placePosition;
            }
            x++;
            if (x > 50) return Vector3.zero;
        }
        return ItemPosition;
    }

    IEnumerator swoop()
    {
        swooping = true;
        StartCoroutine("coolDown");
        agent.angularSpeed = 300;
        agent.speed = 7f;
        LeanTween.moveY(model,target.position.y,1f);
        yield return new WaitForSeconds(2);
        LeanTween.moveLocalY(model, 4, 2);
        agent.speed = 3.5f;

        agent.angularSpeed = 120;
        swooping = false;

    }


    IEnumerator coolDown()
    {
        canSwoop = false;
        yield return new WaitForSeconds(swoopCoolDown);
        canSwoop = true;

    }

}
