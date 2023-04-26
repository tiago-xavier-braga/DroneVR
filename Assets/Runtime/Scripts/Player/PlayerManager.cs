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

        private Rigidbody Rigidbodyrb;

        public Vector2 leftAxisThumbstick, rightAxisThumbstick;

        public void Awake()
        {
            Rigidbodyrb = GetComponent<Rigidbody>();
        }

        public void Update()
        {

            leftAxisThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            rightAxisThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            forceJumpFrame = new Vector3(0, leftAxisThumbstick.y, 0) * force * Time.deltaTime/Rigidbodyrb.mass;

            rotateSideFrame = new Vector3(rightAxisThumbstick.y, 0, 0) * sensitivityAxis * Time.deltaTime;
            rotateVerticalFrame = new Vector3(0, 0, rightAxisThumbstick.x) * sensitivityAxis * Time.deltaTime * -1;
            rotateDirectionalFrame = new Vector3(0, leftAxisThumbstick.x, 0) * sensitivityAxis * Time.deltaTime;


            if (this.transform.rotation.y > 10f)
            {
                forceForwardFrame = transform.forward * rightAxisThumbstick.x * (force / acceleration) * Time.deltaTime;
            }
            else
            {
                forceForwardFrame = -transform.forward * rightAxisThumbstick.x * (force / acceleration) * Time.deltaTime;

            }

            if (rotateSideFrame.y != 0)
            {
                forceForwardFrame = transform.right * rightAxisThumbstick.y * (force / acceleration) * Time.deltaTime;
            }

            this.transform.eulerAngles += rotateSideFrame + rotateVerticalFrame + rotateDirectionalFrame;
            Rigidbodyrb.AddForce(forceJumpFrame, ForceMode.Impulse);
            Rigidbodyrb.AddForce(forceForwardFrame, ForceMode.Impulse);
        }
    }
}
