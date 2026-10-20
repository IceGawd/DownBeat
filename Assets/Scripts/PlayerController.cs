using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float moveDist = 1;

    private int bearing;
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Animator playerAnim;
    [SerializeField] Transform movePoint;
    void Start()
    {
        movePoint.parent = null;
        bearing = 2;
        playerAnim.SetInteger("Bearing", 2);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if( transform.GetChild(0).GetComponent<SpriteRenderer>().color != Color.white)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0f, 0.4f, 0.4f, 0f) * Time.deltaTime;
        }
        playerAnim.SetBool("Moving", false);
        playerAnim.SetBool("Parrying", false);
    }


    public void attemptMove()
    {
        Keyboard kb = Keyboard.current;
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if(kb.wKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame)
            {
                movePoint.position += new Vector3(0f, moveDist, 0f);
                playerAnim.SetInteger("Bearing", 0);
                playerAnim.SetBool("Moving", true);
                bearing = 0;
            }
            if(kb.aKey.wasPressedThisFrame || kb.leftArrowKey.wasPressedThisFrame)
            {
                movePoint.position -= new Vector3(moveDist, 0f, 0f);
                playerAnim.SetInteger("Bearing", 3);
                playerAnim.SetBool("Moving", true);
                bearing = 3;
            }
            if(kb.sKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame)
            {
                movePoint.position -= new Vector3(0f, moveDist, 0f);
                playerAnim.SetInteger("Bearing", 2);
                playerAnim.SetBool("Moving", true);
                bearing = 2;
            }
            if(kb.dKey.wasPressedThisFrame || kb.rightArrowKey.wasPressedThisFrame)
            {
                movePoint.position += new Vector3(moveDist, 0f, 0f);
                playerAnim.SetInteger("Bearing", 1);
                playerAnim.SetBool("Moving", true);
                bearing = 1;
            }
        }
    }

    public void attepmtParry()
    {
        playerAnim.SetBool("Parrying", true);
    }

    public int getBearing()
    {
        return bearing;
    }
}
