using UnityEngine;

public class TestGame : MonoBehaviour
{
    void Start()
    {
        var gm = GameManager.Instance;
        string nombre = "Prueba" + Random.Range(0, 3);
        if (!Ranking.NombreDisponible(nombre))
        {
            Debug.Log($"{nombre} ya existe en el ranking");
            return;
        }
        gm.EmpezarPartida(nombre);
        for (int s = 0; s < 3; s++)
            for (int i = 0; i < 5; i++)
                gm.Responder(s, Random.Range(0, 5));
    }
}