﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public ParticleSystem player_pr;
    public Sprite wallhang;
    public Sprite midair;
    public Sprite standhang;
    public Sprite stagger;
    public Sprite death;

    public int spikeBumps = 0; // how many times the player "bumped" on a spike
    public float speed = 5f;
    public float jumpSpeed = 30f;
    public float jumpCooldown = 0.5f;
    public bool isFacingRight = true;
    public bool inPlay = true;
    public bool isMidAir = false;
    
    private bool isJumping = false;
    private float jumpCooldownC = 0f;
    private float groundDeathOffset = 1.5f;
    private int numSpikesUntilTumble = 2;
    private float timeUntilWallslideParticles = 0.7f;
    private float currentTimeUntilWallSlideParticles = 0f;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform tr;
    //private TrailRenderer trailr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        //trailr = GetComponent<TrailRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (inPlay)
        {
            if (Input.GetButtonDown("Jump") && !isMidAir && jumpCooldownC <= 0f)
            {
                isJumping = true;
                jumpCooldownC = jumpCooldown;
            }
            if (jumpCooldownC > 0f)
            {
                jumpCooldownC -= Time.deltaTime;
            }
            player_pr.transform.position = new Vector3(transform.position.x, transform.position.y, player_pr.transform.position.z);
        }
        else if (isMidAir) {
            // Rotate the character (tumbling) if spikebumps >= 3;
            if (spikeBumps >= numSpikesUntilTumble) {
                tr.Rotate(0, 0, 1.0f, Space.Self);
            }
        }
    }

    // Fixed update is called after a set time
    void FixedUpdate()
    {
        if (inPlay)
        {
            if (isJumping)
            {
                isJumping = false;
                //UnityEngine.Debug.Log("JUMP!");
                if (isFacingRight)
                {
                    rb.AddForce(new Vector2(speed, jumpSpeed), ForceMode2D.Impulse);
                }   else
                {
                    rb.AddForce(new Vector2(-speed, jumpSpeed), ForceMode2D.Impulse);
                }
            }
        }
    }


    // Called whenever a collider begins colliding with an object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            inPlay = false;
            isMidAir = false;
            sr.sprite = death;
            rb.isKinematic = true;
            rb.simulated = false;
            tr.position -= new Vector3(0, groundDeathOffset);
            tr.rotation = Quaternion.identity;
        } else if (inPlay)
        {
            if (collision.collider.tag == "Wall")
            {
                isMidAir = false;
                sr.sprite = wallhang;
                if (collision.collider.name == "LeftWall")
                {
                    isFacingRight = true;
                    sr.flipX = false;
                }
                else
                {
                    isFacingRight = false;
                    sr.flipX = true;
                }
                if (rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0f);
                }
            }
        }
    }


    // Called whenever a collider is currently colliding with an object
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (inPlay)
        {
            if (currentTimeUntilWallSlideParticles < timeUntilWallslideParticles)
            {
                UnityEngine.Debug.Log("Yoink!");
                currentTimeUntilWallSlideParticles += Time.deltaTime;
            } else
            {
                player_pr.Play();
            }
        }
    }

    // Called whenever a collider stops colliding with an object
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (inPlay)
        {
            currentTimeUntilWallSlideParticles = 0f;
            player_pr.Pause();
            if (collision.collider.tag == "Wall")
            {
                sr.sprite = midair;
                isMidAir = true;
            }
        }   
    }
}
