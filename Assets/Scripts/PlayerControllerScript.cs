using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField]
    CharacterController cc;

    [SerializeField]
    Transform mainCamera;

    [SerializeField]
    float speed;

    int health = 3;

    float mouseSensitivity = 400f;

    float xRotation = 0f;
    private bool grounded = true;
    private Vector3 playerVelocity;
    float jumpHeight = 1.0f;
    float gravityValue = -9.81f;

    bool healing = false;
    bool invul = false;

    [SerializeField]
    Transform holderTransform;
    GameObject currentlyHeldObject;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UtilityManager.instance.onPlayerTakeDamage += takeDamage;
        GameObject.Find("EventManager").GetComponent<CameraEffects>().HeadPoint = mainCamera;
        transform.position = new Vector3(transform.position.x,transform.position.y + 1.5f,transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        moveCamera();

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject interactable = checkForinteractable();
        }
        else if (Input.GetButtonUp("Fire1") && currentlyHeldObject)
        {
            currentlyHeldObject.GetComponent<Rigidbody>().isKinematic = false;
            currentlyHeldObject = null;
            holderTransform.DetachChildren();
        }



    }

    void movePlayer()
    {
        grounded = cc.isGrounded;
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = this.transform.TransformDirection(move);
        if (Input.GetButton("Sprint"))
        {
            cc.Move(move * Time.deltaTime * speed*1.5f);
        }
        else
        {
            cc.Move(move * Time.deltaTime * speed);
        }


        if (Input.GetButtonDown("Jump") && grounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue * 2);
        }

        playerVelocity.y += (gravityValue * 2) * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }
    void moveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    GameObject checkForinteractable()
    {
        Ray RayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitInfo;

        if (Physics.Raycast(RayOrigin, out HitInfo, 5.0f))
        {
            Debug.Log(HitInfo.transform.name);
            if (HitInfo.transform.tag == "Interactable")
            {
                currentlyHeldObject = HitInfo.transform.gameObject;
                currentlyHeldObject.transform.SetParent(holderTransform);
                currentlyHeldObject.GetComponent<Rigidbody>().isKinematic = true;
                LeanTween.move(currentlyHeldObject, holderTransform, 0.5f).setEase(LeanTweenType.easeOutQuad);
            }
        }
        return null;
    }

    void takeDamage()
    {
        if (!invul)
        {
            StartCoroutine("iframeTimer");
            health -= 1;
            StartCoroutine("healingTimer");
        }

        if (health == 0)
        {
            die();
        }
    }

    void die()
    {
        UtilityManager.instance.PlayerDie();
    }

    IEnumerator healingTimer()
    {
        healing = true;
        while (health > 3)
        {
            yield return new WaitForSeconds(10);
            health++;
        }
        healing = false;
    }
    IEnumerator iframeTimer()
    {
        invul = true;
        yield return new WaitForSeconds(2);
        invul = false;
    }
}

