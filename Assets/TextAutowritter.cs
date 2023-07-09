using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAutowritter : MonoBehaviour
{
    TextMeshProUGUI TextMesh;
    // Start is called before the first frame update
    void Awake(){
        TextMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }


    public void SetString(string String){
        TextMesh.text = String;
    }
    public void SetInt(int Int){
        TextMesh.text = Int.ToString();
    }
    public void SetColor(Color Color){
        TextMesh.color = Color;
    }
}
