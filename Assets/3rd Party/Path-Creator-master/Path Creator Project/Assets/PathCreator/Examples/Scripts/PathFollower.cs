using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace PathCreation.Examples
{
    public enum Players
    {
        Bot ,
        Player 
    }
    public enum FollowerType
    {
      Curved,
      Straight
    }


    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public FollowerType m_FollowerType;

        public GameObject Next, Retry;
        public Text TimeText;
        public float TimeLeft = 30;
        public Players players;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed =0  , BotSpeed =  4;
        public float distanceTravelled = 1.1f;
   
        bool played ;
        public float MaxSpeed, MinSpeed;

        public float IncreazseMultiplier = .1f;


     
        void Start()
        {

          
  
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
      
               
               


            
   
        }
     
    
        void FixedUpdate()
        {
                      pathCreator = GameManager.Instance.pathCreator;

                          if (speed <= 0)
                        {
                            speed = 0;
                           
                        }
                        if ( GameManager.Instance.StartGame && !GameManager.Instance.GameOver)
                        {
                           
                            if (speed <= MaxSpeed)
                            {
                                speed += IncreazseMultiplier * Time.deltaTime;                               
                            }
                            else
                            {
                              /*  speed -= IncreazseMultiplier * Time.deltaTime;*/
                            }
                        }
                        else
                        {
                              speed = 0;
                             //if (speed >= MinSpeed)
                            //{
                            //    speed -= IncreazseMultiplier * Time.deltaTime;
                            //}
                        }

                        if (pathCreator != null)
                        {
                            distanceTravelled += speed * Time.deltaTime;
                            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                        }
                    
               
            
   
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
    public    void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}