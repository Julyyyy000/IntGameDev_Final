using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMove;
    public float speed;

    Rigidbody2D myBody;

    bool grounded = false;

    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float jumpLimit = 2f;

    bool jumping = false;

    public GameObject textBox;

    int jumpUpLayer;
    int furnitureLayer;

    /*
    public bool canShoot = false;

    public GameObject leftBullet;
    public GameObject rightBullet;
    public GameObject particlePrefab;
    public GameObject collectParticlePrefab;


    Animator myAnim;

    public GameObject gameManager;

    AudioSource myAudio;
    public AudioClip walkAudio;
    public AudioClip jumpAudio;
    public AudioClip collectAudio;
    public AudioClip shootAudio;

    public GameObject text;
    */
    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        jumpUpLayer = LayerMask.NameToLayer("JumpUp");
        furnitureLayer = LayerMask.NameToLayer("FurnitureLayer");
        //myAnim = GetComponent<Animator>();
        //myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        //Debug.Log(horizontalMove);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumping = true;
            //myAnim.SetBool("jumping", true);
            //myAudio.clip = jumpAudio;
            //myAudio.Play();
        }
        /*

        if (horizontalMove > 0.2f || horizontalMove < -0.2f)
        {

            myAnim.SetBool("walking", true);
            if (!myAudio.isPlaying && !myAnim.GetBool("jumping"))
            {
                myAudio.clip = walkAudio;
                myAudio.Play();
            }

        }
        else
        {
            myAnim.SetBool("walking", false);
            if (myAudio.clip == walkAudio)
            {
                myAudio.Stop();
            }
        }

        if (canShoot)
        {
            //Debug.Log(bullet.activeSelf);
            if (Input.GetKeyDown(KeyCode.J))
            {
                myAudio.clip = shootAudio;
                myAudio.Play();
                canShoot = false;
                text.SetActive(false);
                Instantiate(leftBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                myAudio.clip = shootAudio;
                myAudio.Play();
                canShoot = false;
                text.SetActive(false);
                Instantiate(rightBullet, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }
        }
        */
    }

    void FixedUpdate()
    {
        float moveSpeed = horizontalMove * speed;

        if (jumping)
        {
            myBody.AddForce(Vector2.up * jumpLimit, ForceMode2D.Impulse);
            gameObject.layer = jumpUpLayer;
            jumping = false;
        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale;

        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
            gameObject.layer = furnitureLayer;

        }
        /*
        if (grounded)
        {
            myAnim.SetBool("jumping", false);
        }
        */

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        //Debug.Log(hit.transform);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        if (hit.collider != null && hit.transform.CompareTag("ground"))
        {
            grounded = true;

        }
        else
        {
            grounded = false;
        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("hit trigger");
        if (collision.gameObject.CompareTag("interactive"))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                textBox.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        textBox.SetActive(false);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            if (!canShoot)
            {
                canShoot = true;
                Instantiate(collectParticlePrefab, new Vector3(transform.position.x, transform.position.y + 3, 0), Quaternion.identity);
                myAudio.clip = collectAudio;
                myAudio.Play();
                text.SetActive(true);
            }
        }

        if (collision.gameObject.name == "NextLevel")
        {
            gameManager.GetComponent<GameManager>().nextScene = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            //Debug.Log(collision);
            Instantiate(particlePrefab, new Vector3(transform.position.x, transform.position.y - 2, 0), Quaternion.identity);
            //GameObject.Destroy(newDust, 1f);
        }
    }
    */

}
