using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float moveDist = 1;

    public Transform movePoint;
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if( transform.GetChild(0).GetComponent<SpriteRenderer>().color != Color.white)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0f, 0.4f, 0.4f, 0f) * Time.deltaTime;
        }
        
    }


    public void attemptMove()
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if(Math.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") * moveDist, 0f, 0f);
            }else if(Math.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") * moveDist, 0f);
            }

        }
    }

    public void attepmtParry()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1f);
    }
}
