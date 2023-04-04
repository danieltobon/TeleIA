using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class fileIO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = "C:/Users/danie/OneDrive/Escritorio/IA.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Noticia: " +"hOLA");
        writer.Close();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
