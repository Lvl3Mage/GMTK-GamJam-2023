using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextWriter : MonoBehaviour
{
	[SerializeField] string prefix = "";
	[SerializeField] string suffix = "";
	string _text;
	public string text 
	{ 
		get
		{
			return _text;
		} 
		set
		{
			_text = value;
			Set(_text);
		}
	}
	TextMeshProUGUI TextMesh;
    // Start is called before the first frame update
    void Awake(){
        TextMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }


    public void Set(string Text){
        SetString(Text);
    }
    public void Set(int Int){
        SetString(Int.ToString());
    }
    public void Set(Color Color){
    	TextMesh.color = Color;
    }
    void SetString(string value){
    	TextMesh.text = prefix + value + suffix;
    }
}
