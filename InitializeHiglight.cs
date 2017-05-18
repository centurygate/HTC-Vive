using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;
using UnityEngine.UI;

public class InitializeHiglight : MonoBehaviour
{
	public Color color1 = Color.blue;
	public Color color2 = Color.red;
	public float flashFrequency = 2.0f;
	public string hightLightTagName = "HighlightObj";
	//public UnityEngine.UI.Text msgText;
	private Highlighter preHighlighter = null;
	private int delaycount = 0;
	// Use this for initialization
	void Start ()
	{
		//add the highlighter component for the gameobj tagged by "HighlightObj"
		GameObject[] highlightobjs = GameObject.FindGameObjectsWithTag (hightLightTagName);
		foreach (GameObject tagobj in highlightobjs) {
            if(tagobj.GetComponent<Highlighter>() == null)
			tagobj.AddComponent<Highlighter> ();
		}
		//msgText.transform.parent.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
    //void Update ()
    //{
		
    //}

	void ProcessHiglight (RaycastHit hitInfo)
	{
		print ("hitInfo:" + hitInfo.transform.tag);
		Highlighter highlighter;
		//the display text should be fetched from database
//		if(!(hitInfo.transform.gameObject.tag.Equals(hightLightTagName) || hitInfo.transform.parent.gameObject.tag.Equals(hightLightTagName)))
//		{
//			return;
//		}

		Transform transform = hitInfo.transform;
		while ((transform != null) && (transform.gameObject != null)) {
//			&& (!transform.gameObject.tag.Equals (hightLightTagName))
//			print ("---------------------------");
			highlighter = transform.gameObject.GetComponent<Highlighter> ();
			if (highlighter != null) {
				delaycount++;
				//msgText.transform.parent.gameObject.SetActive (true);
				//msgText.text = transform.gameObject.name;
				if ((preHighlighter != null) && (preHighlighter != highlighter)) {
					preHighlighter.FlashingOff ();
				}
				preHighlighter = highlighter;
				print ("Will HighLight");
				highlighter.FlashingOn (color1, color2, flashFrequency);
				StartCoroutine (StopFlashing (highlighter));
				break;
			} else {
				transform = transform.parent;
			}
		}
	}

	IEnumerator StopFlashing (Highlighter higlighter)
	{
		yield return new WaitForSeconds (3);
		higlighter.FlashingOff ();

		while (delaycount >= 0) {
			print ("delaycount = " + delaycount);
			yield return new WaitForSeconds (0);
			delaycount--;
		}
		//msgText.text = "";
		//msgText.transform.parent.gameObject.SetActive (false);
	}
}
