using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Task : MonoBehaviour {
	public bool EndDialog;
	public GameObject Dialog1;
	public GameObject Dialog2;
	public Quest_Event QE;
	public bool Fin_Dialog;
	private bool OnHide;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(QE.end_Quest1 == true)
		{
			Fin_Dialog = true;
		}
		if (EndDialog == true) {
			Time.timeScale = 1;
			QE.Quest1 = true;
			Dialog1.SetActive (false);
		}
		if (Fin_Dialog == true) {
			Time.timeScale = 1;
			QE.Quest1 = false;
			Dialog1.SetActive (false);
		}
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") {if(Fin_Dialog == false) {Time.timeScale = 0;
			
			if (QE.end_Quest1 == false) 
			{
             Dialog1.SetActive (true);
			} else {
				Dialog2.SetActive(true);
			}
			}
			else
			{
				if(OnHide == false)
				{
Dialog2 .SetActive (true);
				OnHide = true;
				}
				
			}
				
			
			
			
		}
	}
}