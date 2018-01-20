using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace basic2D
{
    public class CameraTracking : MonoBehaviour
    {

        public GameObject playerObj = null;
        public float trackingSpeed = 1f;

        private Vector3 lastTargetPos = Vector3.zero;
        private Vector3 currTargetPos = Vector3.zero;


        // Use this for initialization
        void Start() {
            if (playerObj == null) {
                P.WarningPrint(transform, this.GetType().ToString());
                return;
            }

            Vector3 playerPos = playerObj.transform.position;
            Vector3 cameraPos = transform.position;
            lastTargetPos = playerPos;
            currTargetPos = playerPos;
        }

        void Update() {         
            trackPlayer();
            transform.position = Vector3.Lerp(lastTargetPos, currTargetPos, trackingSpeed);
        }

        void trackPlayer() {

            lastTargetPos = transform.position;
            currTargetPos = playerObj.transform.position;
            currTargetPos.z = transform.position.z;
        }
    }

}
