using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using HighlightingSystem;
public class InitializeVR : MonoBehaviour {
	public Transform leftrayPos;
	public Transform rightrayPos;
 	public SteamVR_ControllerManager root;
    public SteamVR_GameView head;
    public SteamVR_TrackedObject leftHand;
    public SteamVR_TrackedObject rightHand;

    private VRControllerState_t leftControllerState;
    private VRControllerState_t rightControllerState;
	public string hightLightTagName = "HighlightObj";
    SteamVR_Controller.Device leftdevice;
    SteamVR_Controller.Device rightdevice;
    public float speed = 0.06f;
    public float height = 1f;
	public GameObject processHighlightObj;
    // Use this for initialization
    void Start () {
		UnityEngine.VR.VRSettings.enabled = true; 
    }

    private void OnGUI()
    {
    }

    private void FixedUpdate()
    {
        this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update () {
        CVRSystem system = OpenVR.System;
        //leftHand
        if (null != system && system.GetControllerState((uint)leftHand.index, ref leftControllerState))
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)(uint)leftHand.index);
            leftdevice = device;
            if (device.GetPress(EVRButtonId.k_EButton_Grip))
            {
                //root.transform.RotateAround(new Vector3(0, 1, 0), -Time.deltaTime* 0.5f);
//                print("leftHand: k_EButton_Grip has been touched!");
				//draw the line
				GetComponent<LineRenderer>().SetPosition(0,leftrayPos.transform.position);


                RaycastHit hitInfo;
				Highlighter highlighter;
				if (Physics.Raycast (leftrayPos.transform.position, leftrayPos.transform.forward, out hitInfo, Mathf.Infinity)) {
					processHighlightObj.SendMessage ("ProcessHiglight", hitInfo);
					GetComponent<LineRenderer> ().SetPosition (1,hitInfo.point);
				} else {
					GetComponent<LineRenderer> ().SetPosition (1, leftrayPos.transform.forward * 1000+leftrayPos.transform.position);
				}
            }
            if (device.GetPressUp(EVRButtonId.k_EButton_Grip))
            {
//                print("rightHand: k_EButton_Grip has been touched!");
                //                root.transform.RotateAround(new Vector3(0, 1, 0), Time.deltaTime*0.5f);
                GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
                GetComponent<LineRenderer> ().SetPosition (1,Vector3.zero);
            }
            if (device.GetTouchDown(EVRButtonId.k_EButton_ApplicationMenu))
            {
//                print("leftHand: k_EButton_ApplicationMenu has been touched!");
                UnityEngine.VR.VRSettings.enabled = false;
                Application.LoadLevel("ShangFei");
            }
			if (device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
			{
				// OnPadButtonPressDown(px, py);
				OnPadButtonPressDownNew(device.GetAxis());
			}
        }

        //righthand
        if (null != system && system.GetControllerState((uint)rightHand.index, ref rightControllerState))
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)(uint)rightHand.index);
            rightdevice = device;
			if (device.GetPress(EVRButtonId.k_EButton_Grip))
            {
				// draw the line startend
				GetComponent<LineRenderer>().SetPosition(0,rightrayPos.transform.position);

                RaycastHit hitInfo;
				Highlighter highlighter;
				if (Physics.Raycast (rightrayPos.transform.position, rightrayPos.transform.forward, out hitInfo, Mathf.Infinity)) {
					processHighlightObj.SendMessage ("ProcessHiglight", hitInfo);
					GetComponent<LineRenderer> ().SetPosition (1,hitInfo.point);
				} else {
					GetComponent<LineRenderer> ().SetPosition (1,rightrayPos.transform.forward * 1000+rightrayPos.transform.position);
				}
//                print("rightHand: k_EButton_Grip has been touched!");
            }
            if (device.GetPressUp(EVRButtonId.k_EButton_Grip))
            {
//                print("rightHand: k_EButton_Grip has been touched!");
                //                root.transform.RotateAround(new Vector3(0, 1, 0), Time.deltaTime*0.5f);
                GetComponent<LineRenderer>().SetPosition(0,Vector3.zero);
                GetComponent<LineRenderer> ().SetPosition (1,Vector3.zero);
            }
            if (device.GetTouchDown(EVRButtonId.k_EButton_ApplicationMenu))
            {
//                print("rightHand: k_EButton_ApplicationMenu has been touched!");
                UnityEngine.VR.VRSettings.enabled = false;
                Application.LoadLevel("ShangFei");
            }
            if (device.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                // OnPadButtonPressDown(px, py);
                OnPadButtonPressDownNew(device.GetAxis());
            }
        }
    }
    void OnPadButtonPressDown(float px, float py)
    {
        float area = 0.8f;

        //RaycastHit hitinfo;
        if (px > 0f && py > -area && py < area)
        {
            root.gameObject.transform.position += head.gameObject.transform.right * speed;
        }
        if (px < 0f && py > -area && py < area)
        {
            //left
            root.gameObject.transform.position -= head.gameObject.transform.right * speed;
        }
        if (py > 0f && px > -area && px < area)
        {
            //up
            root.gameObject.transform.position += head.gameObject.transform.forward * speed;
        }
        if (py < 0f && px > -area && px < area)
        {
            //down
            root.gameObject.transform.position -= head.gameObject.transform.forward * speed;
        }
    }
    //方向圆盘最好配合这个使用 圆盘的.GetAxis()会检测返回一个二位向量，可用角度划分圆盘按键数量  
    //这个函数输入两个二维向量会返回一个夹角 180 到 -180  
    float VectorAngle(Vector2 from, Vector2 to)
    {
        float angle;
        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector2.Angle(from, to);
        return cross.z > 0 ? -angle : angle;
    }
    void OnPadButtonPressDownNew(Vector2 axis)
    {
        // 例子：圆盘分成上下左右  
        float jiaodu = VectorAngle(new Vector2(1, 0), axis);
        //Debug.Log("angle :"+jiaodu);
        //下  
        if (jiaodu > 45 && jiaodu < 135)
        {
            //Debug.Log("下");
            root.gameObject.transform.position -= head.gameObject.transform.forward * speed;
            root.gameObject.transform.position = new Vector3(root.gameObject.transform.position.x, height, root.gameObject.transform.position.z);
        }
        //上  
        if (jiaodu < -45 && jiaodu > -135)
        {
            //Debug.Log("上");
            root.gameObject.transform.position += head.gameObject.transform.forward * speed;
            root.gameObject.transform.position = new Vector3(root.gameObject.transform.position.x, height, root.gameObject.transform.position.z);
        }
        //左  
        if ((jiaodu < 180 && jiaodu > 135) || (jiaodu < -135 && jiaodu > -180))
        {
            //Debug.Log("左");
            root.gameObject.transform.position -= head.gameObject.transform.right * speed;
            root.gameObject.transform.position = new Vector3(root.gameObject.transform.position.x, height, root.gameObject.transform.position.z);
        }
        //右  
        if ((jiaodu > 0 && jiaodu < 45) || (jiaodu > -45 && jiaodu < 0))
        {
            //Debug.Log("右");
            root.gameObject.transform.position += head.gameObject.transform.right * speed;
            root.gameObject.transform.position = new Vector3(root.gameObject.transform.position.x, height, root.gameObject.transform.position.z);
        }
    }

    IEnumerator Vibrate()
    {
        for(float timer = 1; timer >=0; timer -= Time.deltaTime)
        {
            if (leftdevice != null)
            {
                leftdevice.TriggerHapticPulse(3600);
            }

            if (rightdevice != null)
            {
                rightdevice.TriggerHapticPulse(3600);
            }
            yield return 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Vibrate());
    }
	IEnumerator StopFlashing(Highlighter higlighter)
	{
		yield return new WaitForSeconds (3);
		higlighter.FlashingOff ();
	}
}
