using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float speed = 15;
    public bool isTravelling;

    public AudioSource ballAudio;
    public AudioClip ballRolling;

    public ParticleSystem dust;
   

    private Vector3 travelDirection;
    private Vector3 nextCollissionPosition;

    //from where should a swipe be recognized as a minimum swipe
    public int minSwipeRecognition = 500;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;
    public float gravityModifier;
    private Color solvedColor;


    private void Start()
    {
        //generate a random color as solved color
        solvedColor = Random.ColorHSV(0.5f, 1); //0.5 f means a light color. Hue maximum is 1
        GetComponent<MeshRenderer>().material.color = solvedColor;

        ballAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
      
    }
   

    private void FixedUpdate()
    {
        if (isTravelling)
        {
            rb.velocity = speed * travelDirection;
           ballAudio.PlayOneShot(ballRolling, 1.0f);
            dust.Play();
           
                  
        }
       
        

        // change the color of ground
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f); //go don and make it five percent of size of sphere
        int i = 0;
        while (i < hitColliders.Length)
        {
            //if we collide with groundpiece store that in ground variable
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
        
            if(ground && !ground.isColored)
            {
                ground.ChangeColor(solvedColor);
               
               
            }
            //avoid infinite loop

            i++;
        }

       //checking ball has not reached destination
        if(nextCollissionPosition != Vector3.zero)
        {
            if(Vector3.Distance(transform.position, nextCollissionPosition) < 1)
            {
                isTravelling = false;
                travelDirection = Vector3.zero;
                nextCollissionPosition = Vector3.zero;
                ballAudio.Stop();
                
             

            }
        }
        //don't execute the other code in fixed updte if ball is travelling
        if (isTravelling)
        {
            return;
        }
        if(Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
         
        }
        //if mouse swipe is not at the top left which is default position
        if (swipePosLastFrame != Vector2.zero)
        {
            currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

            if(currentSwipe.sqrMagnitude < minSwipeRecognition)
            {
                return;
            }
            //get direction not distance
            currentSwipe.Normalize();

            //up/down
            if(currentSwipe.x > -0.5f && currentSwipe.x < 0.5)
                
            {
                //go up/down

                SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
            }
            if(currentSwipe.y> -0.5f && currentSwipe.y < 0.5)
            {
                //go left/right
                SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
            }
        }

        swipePosLastFrame = swipePosCurrentFrame;
        {

        }
        if(Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit; // check which object the ball will collide with
        if(Physics.Raycast(transform.position,direction,out hit, 100f))
        {
            nextCollissionPosition = hit.point;
            ballAudio.Stop();
          

        }
        isTravelling = true;
    }
}
