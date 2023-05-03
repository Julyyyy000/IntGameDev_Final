using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Yarn.Unity;

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

    Animator myAnim;
    /*
    public GameObject particlePrefab;
    public GameObject collectParticlePrefab;

    public GameObject gameManager;
    */
    AudioSource myAudio;
    public AudioClip walkAudio;
    public AudioClip jumpAudio;
    //public AudioClip collectAudio;
    //public AudioClip shootAudio;

    //public GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        jumpUpLayer = LayerMask.NameToLayer("JumpUp");
        furnitureLayer = LayerMask.NameToLayer("FurnitureLayer");
        myAnim = GetComponent<Animator>();
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        //Debug.Log(horizontalMove);

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            jumping = true;
            myAnim.SetBool("jumping", true);
            myAudio.clip = jumpAudio;
            myAudio.volume = 0.5f;
            myAudio.Play();
        }


        if (horizontalMove > 0.2f || horizontalMove < -0.2f)
        {

            myAnim.SetBool("walking", true);
            
            if (!myAudio.isPlaying && !myAnim.GetBool("jumping"))
            {
                myAudio.clip = walkAudio;
                myAudio.volume = 1;
                myAudio.Play();
            }
            

            if (horizontalMove > 0.2f)
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 0);
            } else
            {
                transform.localScale = new Vector3(-0.8f, 0.8f, 0);
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
            myAnim.SetBool("jumping", false);

        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
            gameObject.layer = furnitureLayer;

        }
    
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        //Debug.Log(hit.transform);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);

        if (hit.collider != null && (hit.transform.CompareTag("ground") || hit.transform.CompareTag("furniture")))
        {
            grounded = true;

        }
        else
        {
            grounded = false;
        }

        if (grounded)
        {
            myAnim.SetBool("landing", true);
            if (hit.transform.CompareTag("furniture"))
            {
                myAnim.SetBool("onObject", true);
            } else
            {
                myAnim.SetBool("onObject", false);
            }
        }

        Debug.Log(grounded);
        //Debug.Log(hit.transform.name);
        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("hit trigger");
        if (collision.gameObject.CompareTag("interactive"))
        {
            if (Input.GetKey(KeyCode.C))
            {
                textBox.GetComponent<DialogueRunner>().StartDialogue("Gregg");
                collision.gameObject.GetComponent<Animator>().SetBool("talk", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //textBox.SetActive(false);
        if (collision.gameObject.CompareTag("interactive"))
        {
           collision.gameObject.GetComponent<Animator>().SetBool("talk", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //just for now TODO:change to game manager
        if (collision.transform.CompareTag("door"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {

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
