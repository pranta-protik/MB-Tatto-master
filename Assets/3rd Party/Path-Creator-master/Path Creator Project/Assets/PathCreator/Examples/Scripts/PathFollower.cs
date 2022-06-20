using UnityEngine;

namespace PathCreation.Examples
{
    public class PathFollower : MonoBehaviour
    {
        public EndOfPathInstruction endOfPathInstruction;
        public float distanceTravelled = 3.7f;
        public float maxSpeed = 2f;
        public float increaseMultiplier = 4f;
        public float speedDecrementFactor = 3f;
        public float speedDecrementDuration = 0.6f;

        [HideInInspector] public PathCreator pathCreator;
        [HideInInspector] public float speed;

        void Start()
        {
            enabled = false;
            if (pathCreator != null)
            {
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
            
            if (GameManager.Instance.StartGame && !GameManager.Instance.GameOver)
            {
                if (speed <= maxSpeed)
                {
                    speed += increaseMultiplier * Time.deltaTime;
                }
            }
            else
            {
                speed = 0;
            }

            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        private void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}