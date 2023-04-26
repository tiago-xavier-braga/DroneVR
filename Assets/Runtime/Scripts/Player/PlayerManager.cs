using Facebook.WitAi.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DroneVR.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class PlayerManager : MonoBehaviour
    {
        [Range(0f, 3000f)]
        [SerializeField] float force = 5000f;
        [Range(0f, 20f)]
        [SerializeField] float acceleration = 5;
        [Range(0f, 500f)]
        [SerializeField] float sensitivityAxis = 500f;
        [Range(0f, 0.5f)]
        [SerializeField] float axisDeadZone = 0.2f;
        [Range(0f, 15f)]
        [SerializeField] float heightOperation = 4f;

        private Vector3 forceJumpFrame, forceForwardFrame;
        private Vector3 rotateSideFrame, rotateVerticalFrame, rotateDirectionalFrame;
        private float RPM = 0;

        private Rigidbody Rigidbodyrb;

        public Vector2 leftAxisThumbstick, rightAxisThumbstick;

        public void Awake()
        {
            Rigidbodyrb = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            if(RPM < force)
            {
                RPM++;
            }

            leftAxisThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            rightAxisThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            forceJumpFrame = new Vector3(0, leftAxisThumbstick.y, 0) * RPM * Time.deltaTime/Rigidbodyrb.mass;

            rotateSideFrame = new Vector3(rightAxisThumbstick.y, 0, 0) * sensitivityAxis * Time.deltaTime;
            rotateVerticalFrame = new Vector3(0, 0, rightAxisThumbstick.x) * sensitivityAxis * Time.deltaTime * -1;
            rotateDirectionalFrame = new Vector3(0, leftAxisThumbstick.x, 0) * sensitivityAxis * Time.deltaTime;


            if (rotateSideFrame.x != 0)
            {
                forceForwardFrame = transform.forward * (force / acceleration) * Time.deltaTime;
            }
            if (rotateSideFrame.y != 0)
            {
                forceForwardFrame = transform.right * (force / acceleration) * Time.deltaTime;
            }

            this.transform.eulerAngles += rotateSideFrame + rotateVerticalFrame + rotateDirectionalFrame;
            Rigidbodyrb.AddForce(forceJumpFrame, ForceMode.Impulse);
            Rigidbodyrb.AddForceAtPosition(forceForwardFrame, this.transform.position, ForceMode.Impulse);
        }
    }
}
