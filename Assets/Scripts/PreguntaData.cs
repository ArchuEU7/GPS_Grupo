using UnityEngine;

[CreateAssetMenu(fileName = "Pregunta", menuName = "JuegoGPS/Pregunta")]
public class PreguntaData : ScriptableObject
{
    [TextArea] public string enunciado;
    public string[] opciones = new string[5];
    [Range(0, 4)] public int indiceCorrecta;
}