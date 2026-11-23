using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class EntradaRanking
{
    public string nombre;
    public int puntos;
    public float tiempo; // segundos
}

[Serializable]
class ListaRanking { public List<EntradaRanking> entradas = new List<EntradaRanking>(); }

public static class Ranking
{
    const string CLAVE = "ranking";

    public static List<EntradaRanking> Cargar()
    {
        string json = PlayerPrefs.GetString(CLAVE, "");
        if (string.IsNullOrEmpty(json)) return new List<EntradaRanking>();
        return JsonUtility.FromJson<ListaRanking>(json).entradas;
    }

    static void Guardar(List<EntradaRanking> lista)
    {
        PlayerPrefs.SetString(CLAVE, JsonUtility.ToJson(new ListaRanking { entradas = lista }));
        PlayerPrefs.Save();
    }

    // Nombre libre (sin distinguir mayúsculas ni espacios extra)
    public static bool NombreDisponible(string nombre)
    {
        string n = nombre.Trim().ToLower();
        return n.Length > 0 && !Cargar().Any(e => e.nombre.Trim().ToLower() == n);
    }

    // Más puntos primero, empate -> menos tiempo
    public static List<EntradaRanking> Ordenado()
    {
        return Cargar().OrderByDescending(e => e.puntos).ThenBy(e => e.tiempo).ToList();
    }

    // Posición compartida si mismos puntos y mismo tiempo (1, 1, 3...)
    public static int Posicion(EntradaRanking entrada)
    {
        return Cargar().Count(e => e.puntos > entrada.puntos ||
                                  (e.puntos == entrada.puntos && e.tiempo < entrada.tiempo)) + 1;
    }

    public static EntradaRanking Añadir(string nombre, int puntos, float tiempo)
    {
        var lista = Cargar();
        // Redondeo a décimas para que "mismo tiempo" sea realista
        var nueva = new EntradaRanking
        {
            nombre = nombre.Trim(),
            puntos = puntos,
            tiempo = Mathf.Round(tiempo * 10f) / 10f
        };
        lista.Add(nueva);
        Guardar(lista);
        return nueva;
    }

    public static void Borrar() { PlayerPrefs.DeleteKey(CLAVE); } // para pruebas
}