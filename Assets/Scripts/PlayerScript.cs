using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private int scoreValue = 0;
    private bool facingRight = true;
    private int lives = 3;
    public int groundbound = 0;

    public string s ="Score: ";
    public string l ="Lives: ";
    public float speed;
    public Text score;
    public Text End;
    public Text lifeLog;
    
    public AudioClip musicClipOne;
    public AudioClip endMusic;
    public AudioSource musicSource;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicClipOne;
        musicSource.loop = true;
        musicSource.Play();
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        score.text = string.Concat(s, score.text);
        End.text = "";
        lifeLog.text = lives.ToString();
        lifeLog.text = string.Concat(l, lifeLog.text);
    }

    // Update is called once per frame
    void Update()
    {
        if (groundbound == 0 && rd2d.velocity.x < .5 && rd2d.velocity.x > -.5)
        {
            anim.SetInteger("State", 0);
        }
        if (groundbound == 0 && rd2d.velocity.x > .5 || rd2d.velocity.x < -.5 && groundbound == 0)
        {
            anim.SetInteger("State", 1);
        }
        if (rd2d.velocity.y > 1 || rd2d.velocity.y < -1)
        {
            groundbound = 1;
            anim.SetInteger("State", 2);
        }
    }
    void FixedUpdate()
    {   
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            score.text = string.Concat(s, score.text);
            Destroy(collision.collider.gameObject);
            if (scoreValue == 5)
            {
                transform.position = new Vector2(3.0f,26.0f);
                lives = 3;
                lifeLog.text = lives.ToString();
                lifeLog.text = string.Concat(l, lifeLog.text);
            }
            if (scoreValue == 10)
            {
                musicSource.Stop();
                musicSource.clip = endMusic;
                musicSource.Play();
                End.text = "You win! Game created by Jake Vaughan.";
                gameObject.SetActive(false);
            }
        }
        if (collision.collider.tag == "Enemy")
        {
            lives -= 1;
            lifeLog.text = lives.ToString();
            lifeLog.text = string.Concat(l, lifeLog.text);
            Destroy(collision.collider.gameObject);
            if (lives == 0)
            {
                End.text = "You lose. What a shocker. Game created by Jake Vaughan.";
                gameObject.SetActive(false);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {   
            groundbound = 0;
            if (Input.GetKey(KeyCode.W))
            {
                groundbound = 1;
                rd2d.AddForce(new Vector2(0,3), ForceMode2D.Impulse);
            }
        }
    }
    void Flip()
    {
        facingRight =!facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x= Scaler.x*-1;
        transform.localScale = Scaler;
    }
}
