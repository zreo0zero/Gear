using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	GameObject thedoor;

	public void Opne (){
		thedoor= GameObject.FindWithTag("SF_Door");
		thedoor.GetComponent<Animation>().Play("open");
	}

	public void Close (){
		thedoor= GameObject.FindWithTag("SF_Door");
		thedoor.GetComponent<Animation>().Play("close");
	}
}