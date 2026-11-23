using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Banco de preguntas (mínimo 15)")]
    public List<PreguntaData> bancoPreguntas;

    public const int NUM_SITIOS = 3;
    public const int PREGUNTAS_POR_SITIO = 5;

    public string nombreJugador;
    public bool jugando;
    public float tiempo;

    // Preguntas asignadas a cada sitio
    public List<PreguntaData>[] preguntasSitio = new List<PreguntaData>[NUM_SITIOS];
    int[] puntosSitio = new int[NUM_SITIOS];
    int[] indicePregunta = new int[NUM_SITIOS];
    bool[] sitioCompletado = new bool[NUM_SITIOS];

    void Awake() { Instance = this; }

    void Update()
    {
        if (jugando) tiempo += Time.deltaTime;
    }

    public void EmpezarPartida(string nombre)
    {
        // --- LÓGICA FINAL (mínimo 15 preguntas, sin repetir) ---
        // if (bancoPreguntas.Count < NUM_SITIOS * PREGUNTAS_POR_SITIO)
        // {
        //     Debug.LogError("Faltan preguntas en el banco");
        //     return;
        // }
        // var mezcla = bancoPreguntas.OrderBy(_ => Random.value).ToList();

        // --- MODO PRUEBA (1 pregunta) ---
        if (bancoPreguntas.Count < 1)
        {
            Debug.LogError("Añade al menos 1 pregunta al banco");
            return;
        }

        nombreJugador = nombre;
        tiempo = 0;
        for (int s = 0; s < NUM_SITIOS; s++)
        {
            // --- LÓGICA FINAL ---
            // preguntasSitio[s] = mezcla.GetRange(s * PREGUNTAS_POR_SITIO, PREGUNTAS_POR_SITIO);

            // --- MODO PRUEBA: la misma pregunta 5 veces ---
            preguntasSitio[s] = Enumerable.Repeat(bancoPreguntas[0], PREGUNTAS_POR_SITIO).ToList();

            puntosSitio[s] = 0;
            indicePregunta[s] = 0;
            sitioCompletado[s] = false;
        }
        jugando = true;
    }

    public PreguntaData PreguntaActual(int sitio)
    {
        if (sitioCompletado[sitio]) return null;
        return preguntasSitio[sitio][indicePregunta[sitio]];
    }

    // Devuelve true si acierta
    public bool Responder(int sitio, int opcion)
    {
        bool acierto = opcion == PreguntaActual(sitio).indiceCorrecta;
        if (acierto) puntosSitio[sitio]++;
        indicePregunta[sitio]++;
        if (indicePregunta[sitio] >= PREGUNTAS_POR_SITIO)
        {
            sitioCompletado[sitio] = true;
            if (sitioCompletado.All(c => c)) TerminarPartida();
        }
        return acierto;
    }

    // Se aleja antes de acabar: pierde progreso, mismas 5 preguntas
    public void AbandonarSitio(int sitio)
    {
        if (sitioCompletado[sitio]) return;
        puntosSitio[sitio] = 0;
        indicePregunta[sitio] = 0;
    }

    public bool SitioCompletado(int sitio) => sitioCompletado[sitio];
    public int PuntosTotales() => puntosSitio.Sum();

    void TerminarPartida()
    {
        jugando = false;
        var entrada = Ranking.Añadir(nombreJugador, PuntosTotales(), tiempo);
        int pos = Ranking.Posicion(entrada);
        Debug.Log($"Fin: {entrada.nombre} {entrada.puntos} puntos en {entrada.tiempo}s. Posición {pos}");
        // Paso siguiente: mostrar esto en la pantalla final
    }
}