using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
  Animator animator;// in case we add player animations
  public CharacterController controller;
  public float speed = 12f;
  
  public float gravity = -9.81f;
  public Transform groundCheck;
  public Transform headCheck;
  public LayerMask headMask;
  public float groundDistance = 0.45f;
  public float headDistance = .45f;
  public LayerMask groundMask;
  public float jumpHeight = 3f;
  public float normalHeight, crouchHeight;
  public float sprintTime = 4.5f;
  float sprintTimeOG;
  bool isCrouched = false;
  public float stunT = 1.5f;
  public int ending = 0;
  //public float rcRange=2.5F;
  float ogSpeed;
  Vector3 velocity;
  bool isGrounded;
  bool isStunned = false;
  bool isUnder;
  bool audioPlayed =  false;
  bool winL;
  bool isSprinting;

  //audioStuff here
  public AudioSource gameMusic;//if using game music
  AudioSource playerAudio;
  public AudioClip stun;
  public AudioClip win;
  public AudioClip lose;
  public AudioClip shutDown;


    void Start()
    {
      ogSpeed = speed;
      playerAudio = GetComponent<AudioSource>();
      isSprinting = false;
      sprintTimeOG = sprintTime;

    }


    void Update()//all control stuff is in here
    {
      /*
      //ray cast to detect things in front and ask player if they want to interact
      Vector3 direction = Vector3.forward;
      Ray theRay = new Ray(transform.position, transform.TransformDirection(direction * rcRange));
      Debug.DrawRay(transform.position, transform.TransformDirection(direction * rcRange));

      if(Physics.Raycast(theRay, out RaycastHit hit, rcRange))
      {
        if(hit.collider.tag == "BackRobot")
        {
          Debug.Log("EnemyDetected");
          //turn on UI input here for the button
          if(Input.GetKeyDown(KeyCode.F))
          {
            Debug.Log("ROBOTOFF");
            //TurnOff();
          }
        }

      }*/


      //controls
      isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
      isUnder= Physics.CheckSphere(headCheck.position, headDistance, headMask);

      if(isGrounded && velocity.y <0)
      {
        velocity.y = -2f;
      } 
      float x = Input.GetAxis ("Horizontal");
      float z = Input.GetAxis ("Vertical");
      
      Vector3 move = transform.right * x + transform.forward * z;
      controller.Move(move * speed * Time.deltaTime);
      

      if(isStunned == false)
      {
        if(Input.GetKeyDown(KeyCode.LeftShift) && sprintTime >= 0)
        {
          speed += 6;
          isSprinting = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) || sprintTime <=0)
        {
          if(speed == 12)
            speed -= 6;
          isSprinting = false;
          Debug.Log("NO MORE SPRINT");
        }
        /*if(Input.GetButtonDown("Jump")&& isGrounded && isUnder == false)//remove if jump is not needed
        {
          velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }*/ //BRING BACK JUMP IF NEEDED
      }

      if(isSprinting == true)
      {
          sprintTime-=Time.deltaTime;
          Debug.Log("Subtracting: "+ sprintTime);
      }
      else
      {
        if(sprintTime < sprintTimeOG)
        {
          sprintTime += Time.deltaTime;
          Debug.Log("Adding: "+ sprintTime);
        }
      }

      if(Input.GetKeyDown(KeyCode.C) && isCrouched == false)
      {
        controller.height = crouchHeight;
        isCrouched = true;
      }
      else if(Input.GetKeyDown(KeyCode.C) && isCrouched == true && isUnder == false)
      {
        controller.height = normalHeight;
        isCrouched = false;
      }

      velocity.y += gravity * Time.deltaTime;
      controller.Move(velocity * Time.deltaTime);//double the multiplication to simulate physics
        
    }
  void OnCollisionEnter(Collision contact)//I'm not sure if this works with player collider
  {
    if(contact.collider.tag == "Projectlie")
    {
      PlaySound(stun);
      StartCoroutine(Shocked(stunT));
    }
    if(contact.collider.tag == "enemy")
    {
      //game over
      winL = false;
      //could add grab animation if one is created. And have player uncrouch
      GameOver(winL);
    }
    if(contact.collider.tag == "diamond")
    {
      winL = true;
      GameOver(winL);
    }
    //change to whatever you name the stun pro.
    //sound effect for stun can go in here
    

  }
  private IEnumerator Shocked(float interval)
  {
    speed= 1.5f;
    yield return new WaitForSeconds(stunT);
    speed = ogSpeed;
  }
  public void PlaySound(AudioClip clip)
  {
      if (clip == lose || clip == win)
      {
          if(audioPlayed == false)
          {
              playerAudio.volume = 0.06f;
              audioPlayed = true;
              playerAudio.PlayOneShot(clip);
          }     
      }
      else
          playerAudio.PlayOneShot(clip);
    }
    

  void GameOver(bool winL)
  {
    //stop all movement like the pause screen and unlock the mouse
    if(winL == false)
    {
      PlaySound(lose);
      ending--;
      //turn off game audio component here
      //UI updates to lose screen (prolly gonna do that on another screen)
    }
    if(winL == true)
    {
      PlaySound(win);
      ending++;
      //same as lose but for a win
    }
        
  }

  void TurnOff()
  {
      PlaySound(shutDown);
      //Turns off robot
  }
}
