using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMove;
    public float setSpeed;
    private float speed;

    Rigidbody2D myBody;

    bool grounded = false;

    public float castDist = 0.2f;
    public float gravityScale = 5f;
    public float gravityFall = 40f;
    public float setJumpLimit = 5.7f;
    private float jumpLimit;
    private float xScale;
    public float objectWalkSpeed;

    bool jumping = false;

    public GameObject textBox;

    int jumpUpLayer;
    int furnitureLayer;

    Animator myAnim;

    public GameObject questionBubble;
    /*
    public GameObject particlePrefab;
    public GameObject collectParticlePrefab;

    public GameObject gameManager;
    */
    AudioSource myAudio;
    public AudioClip walkAudio;
    public AudioClip jumpAudio;

    public LineView LineView;
    public DialogueRunner dialogueRunner;
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
        xScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (textBox.GetComponent<DialogueRunner>().IsDialogueRunning)
        {
            speed = 0;
            jumpLimit = 0;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                LineView.UserRequestedViewAdvancement();
            }
        } else if (myAnim.GetBool("onObject"))
        {
            speed = objectWalkSpeed;
            jumpLimit = setJumpLimit;
        } else
        {
            speed = setSpeed;
            jumpLimit = setJumpLimit;
        }

        questionBubble.transform.position = new Vector3(transform.position.x, transform.position.y + 4.5f, 0);
        horizontalMove = Input.GetAxis("Horizontal");
        //Debug.Log(horizontalMove);

        if (Input.GetKeyDown(KeyCode.Space) && grounded && !dialogueRunner.IsDialogueRunning)

        {
            myAnim.SetBool("jumping", true);
            myAnim.SetBool("landing", false);
            jumping = true;
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
                transform.localScale = new Vector3(xScale, xScale, 0);
            } else
            {
                transform.localScale = new Vector3(-xScale, xScale, 0);
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
            

        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
            gameObject.layer = furnitureLayer;
            myAnim.SetBool("jumping", false);

        }
    
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);
        //Debug.Log(hit.transform);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);
        Debug.Log(myBody.velocity.y);

        if (hit.collider != null && myBody.velocity.y <= 0.1f && (hit.transform.CompareTag("ground") || hit.transform.CompareTag("furniture")))
        {
            grounded = true;
            myAnim.SetBool("landing", true);

        }
        else
        {
            grounded = false;
        }

        if (grounded)
        {
            if (hit.transform.CompareTag("furniture"))
            {
                myAnim.SetBool("onObject", true);
            } else
            {
                myAnim.SetBool("onObject", false);
            }
        }

        //Debug.Log(grounded);
        //Debug.Log(hit.transform.name);
        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0);
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("hit trigger");
        if (collision.gameObject.CompareTag("interactive"))
        {
            questionBubble.SetActive(true);
            Debug.Log(dialogueRunner.IsDialogueRunning);
            if (Input.GetKeyDown(KeyCode.C) && !dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.StartDialogue(collision.gameObject.name);
                //collision.gameObject.GetComponent<Animator>().SetBool("talk", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //textBox.SetActive(false);
        if (collision.gameObject.CompareTag("interactive"))
        {
           //collision.gameObject.GetComponent<Animator>().SetBool("talk", false);

        }
        questionBubble.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("interactive"))
        {
            
            //questionBubble.SetActive(true);
        }

        
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
