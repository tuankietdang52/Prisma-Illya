using Assets.Script;
using Assets.Script.PlayerContainer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Sprite state;

    private Transform player => Player.Instance.transform;

    [SerializeField]
    private GameObject detectObject;

    [SerializeField]
    protected int framecount = 0;

    [SerializeField]
    protected float speed = 0.05f;

    [SerializeField]
    protected float chasespeed = 0.15f;

    [SerializeField]
    protected float detectdistance = 10f;

    private bool isDetect = false;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Detect();
    }

    private void FixedUpdate()
    {
        if (isDetect)
        {
            MoveToPlayer();
            return;
        }

        framecount++;
        SetDirection();
        RegularMove();
    }

    private void SetDirection()
    {
        if (framecount < 400) return;
        
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        framecount = 0;
    }

    private void Detect()
    {
        if (isDetect) return;

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hitplayer = Physics2D.Raycast(detectObject.transform.position, direction, detectdistance);


        Debug.DrawRay(detectObject.transform.position, direction * detectdistance, Color.red);

        if (hitplayer.collider != null)
        {
            if (hitplayer.collider.tag == "Player") 
            {
                isDetect = true;
                return;
            }
        }

        isDetect = false;
    }

    private void RegularMove()
    {
        float x;
        if (transform.localScale.x > 0)
        {
            x = transform.position.x + speed;
        }
        else
        {
            x = transform.position.x - speed;
        }

        Vector2 pos = new Vector2(x, transform.position.y);
        transform.position = pos;
    }

    private void MoveToPlayer()
    {
        transform.localScale = new Vector2(transform.localScale.x + 0.01f, transform.localScale.y + 0.01f);
        float x;
        if (transform.position.x > player.position.x)
        {
            x = transform.position.x - chasespeed;
        }
        else
        {
            x = transform.position.x + chasespeed;
        }

        Vector2 pos = new Vector2(x, transform.position.y);
        transform.position = pos;
    }
}
