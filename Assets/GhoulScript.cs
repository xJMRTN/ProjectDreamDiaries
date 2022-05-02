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
        agent.SetDestination(destination);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        if (Vector3.Distance(target.position, transform.position) < 10f && canSwoop)
        {
            StartCoroutine("swoop");
        }
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
