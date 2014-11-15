using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	public Vector3[] positions;
	public GUIText[] texts;
	private float vertValue;
	private int posID = 0;
	public KeyCode confirm;
	public float fadeTimer;

	void Start(){
		Time.timeScale = 1.0f;
	}

	void Update(){
		vertValue = Input.GetAxis("Vertical");

		if(posID == 1 && vertValue > 0){
			posID = 0;
			transform.position = positions[posID];
			FadeText(posID);
		}else if(posID == 0 && vertValue < 0){
			posID = 1;
			transform.position = positions[posID];
			FadeText(posID);
		}
		

		if(Input.GetKey(KeyCode.Return) || Input.GetKey(confirm)){
			ConfirmSelect();
		}
	}

	private void ConfirmSelect(){
		switch(posID){
		case 0:
			Application.LoadLevel("level1");
			break;
		case 1:
			Application.Quit();
			break;
		default:
			break;
		}
	}

	//these are functions to fade the text of the main menu in and out 
	public void FadeText (int pID){
		for(int i = 0; i < texts.Length; i++){
			if(i == pID){
				StartCoroutine(FadeIn(texts[i]));
			}else{
				StartCoroutine(FadeOut(texts[i]));
			}
		}
	}

	private IEnumerator FadeIn (GUIText gUIText){
		float speed = 1.0f / fadeTimer;
		for(float t = 0.0f; t < 1.0; t += Time.deltaTime*speed){
			float a = Mathf.Lerp(0.0f, 1.0f, t);
			Color faded = gUIText.color;
			faded.a = a;
			gUIText.color = faded;
			yield return 0;
		}

	}

	private IEnumerator FadeOut (GUIText gUIText){
		float speed = 1.0f / fadeTimer;
		for(float t = 0.0f; t < 1.0; t += Time.deltaTime*speed){
			float a = Mathf.Lerp(1.0f, 0.0f, t);
			Color faded = gUIText.color;
			faded.a = a;
			gUIText.color = faded;
			yield return 0;
		}
	}
}
