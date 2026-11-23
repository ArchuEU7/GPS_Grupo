using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PantallaInicio : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public Button botonJugar;
    public TextMeshProUGUI textoAviso;

    void Start()
    {
        botonJugar.interactable = false;
        textoAviso.text = "";
        inputNombre.onValueChanged.AddListener(ComprobarNombre);
        botonJugar.onClick.AddListener(Jugar);
    }

    void ComprobarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            botonJugar.interactable = false;
            textoAviso.text = "";
        }
        else if (!Ranking.NombreDisponible(nombre))
        {
            botonJugar.interactable = false;
            textoAviso.text = "Ese nombre ya está en el ranking";
        }
        else
        {
            botonJugar.interactable = true;
            textoAviso.text = "";
        }
    }

    void Jugar()
    {
        GameManager.Instance.EmpezarPartida(inputNombre.text.Trim());
        gameObject.SetActive(false); // oculta el panel de inicio
    }
}