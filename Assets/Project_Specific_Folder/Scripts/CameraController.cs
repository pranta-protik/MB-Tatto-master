using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SharkAttack
{
    public class CameraController : MonoBehaviour
    {
        public GameObject player;
        public float offsetZ = -9.18f;
        public float offsetX = -0.47f;
        public float offsetY = -1.08f, OffsetYIncrease = .1f;
        Quaternion InitailRot;
        public int SmoothSpeed;
        // Use this for initialization
        void Start()
        {
            offsetZ = -9.18f;

            InitailRot = transform.rotation;
            //offset = transform.position - player.transform.position;
        }
        // Update is called once per frame
        void Update()
        {
            //  transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.transform.position.z +offset);
          

                transform.position = Vector3.Lerp(transform.position, new Vector3(4.96f + offsetX, 5.96f + offsetY, player.transform.position.z + offsetZ), Time.deltaTime * SmoothSpeed);
            


        }
        public void BlastShake()
        {
            transform.DOShakePosition(1f, new Vector3(100f, 50f, 0), 10, 8f, false, true);
        }
    }
}