using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SharkAttack
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] public GameObject player;
        public float a;
       
        private void Start()
        {
            // As fallback get it only ONCE

        }
        void LateUpdate()
        {
            var position = transform.position;
            // overwrite only the X component
            position.z = player.transform.position.z; position.x = player.transform.position.x + a;
            // assign the new position back
            transform.position = position;
        }



    
        public void BlastShake()
        {
            transform.DOShakePosition(1f, new Vector3(100f, 50f, 0), 10, 8f, false, true);
        }
    }
}