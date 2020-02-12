using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    private Rigidbody rig;
    private AudioSource audioSource;

    void Awake ()
    {
        // get the components
        rig = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1.0f;
    }

    void Update ()
    {
        if(GameManager.instance.paused)
            return;

        Move();

        // did we press down the "jump" button?
        if(Input.GetButtonDown("Jump"))
        {
            TryJump();
        }
    }

    void Move ()
    {
        // getting our inputs
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        // calculate the direction we need to move in
        Vector3 dir = new Vector3(xInput, 0, zInput) * moveSpeed;
        dir.y = rig.velocity.y;

        // set the rigidbody velocity
        rig.velocity = dir;

        // calculate the new forward direction
        Vector3 facingDir = new Vector3(xInput, 0, zInput);

        // are we holding down our movement buttons?
        if(facingDir.magnitude > 0)
        {
            transform.forward = facingDir;
        }
    }

    // called when we press the "jump" button
    void TryJump ()
    {
        // create 4 ray pointing downwards from the player on each corner of the cube
        Ray ray1 = new Ray(transform.position + new Vector3(0.5f, 0, 0.5f), Vector3.down);
        Ray ray2 = new Ray(transform.position + new Vector3(-0.5f, 0, 0.5f), Vector3.down);
        Ray ray3 = new Ray(transform.position + new Vector3(0.5f, 0, -0.5f), Vector3.down);
        Ray ray4 = new Ray(transform.position + new Vector3(-0.5f, 0, -0.5f), Vector3.down);

        // shoot those rays
        bool cast1 = Physics.Raycast(ray1, 0.7f);
        bool cast2 = Physics.Raycast(ray2, 0.7f);
        bool cast3 = Physics.Raycast(ray3, 0.7f);
        bool cast4 = Physics.Raycast(ray4, 0.7f);

        // shoot the raycast
        if(cast1 || cast2 || cast3 || cast4)
        {
            // add force upwards
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        // did we hit an enemy?
        if(other.CompareTag("Enemy"))
        {
            GameManager.instance.GameOver();
        }
        // did we hit a coin?
        else if(other.CompareTag("Coin"))
        {
            GameManager.instance.AddScore(1);
            Destroy(other.gameObject);
            audioSource.Play();
        }
        // did we hit a goal?
        else if(other.CompareTag("Goal"))
        {
            GameManager.instance.LevelEnd();
        }
    }
}