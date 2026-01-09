using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Final")]
    public GameObject panelFinal;
    public TextMeshProUGUI textoMensaje;

    [Header("Contadores de Vida")]
    public TextMeshProUGUI vidaJugadorTexto;
    public TextMeshProUGUI vidaBossTexto;
    
    [Header("Avisos")]
    public TextMeshProUGUI avisoB;
    private bool avisoQuitado = false;

    private bool juegoTerminado = false;

    void Awake() { instance = this; }

    void Start()
    {
        if (panelFinal != null) panelFinal.SetActive(false);
        if (vidaBossTexto != null) vidaBossTexto.gameObject.SetActive(false);
        
        if (avisoB != null) {
            avisoB.gameObject.SetActive(true);
            avisoB.text = "SI ENCUENTRAS LA SALA DEL BOSS PRESIONA LA B PARA INVOCARLO";
        }
        Time.timeScale = 1f;
    }

    void Update()
    {
        // QUITAR AVISO AL PULSAR CUALQUIER TECLA
        if (!avisoQuitado && Input.anyKeyDown) {
            avisoQuitado = true;
            if (avisoB != null) avisoB.gameObject.SetActive(false);
        }

        if (juegoTerminado) {
            // REINICIAR CON ENTER
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                Time.timeScale = 1f;
                // LIMPIEZA DE LISTA ANTES DE REINICIAR
                SlimeFollow.fila.Clear(); 
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            // SALIR CON ESC
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("Saliendo del juego...");
                Application.Quit();
            }
        }
    }

    public void ActualizarVidaJugador(int vida) {
        if (vidaJugadorTexto != null) vidaJugadorTexto.text = "HP: " + vida;
    }

    public void ActualizarVidaBoss(float vida) {
        if (vidaBossTexto != null) vidaBossTexto.text = "BOSS: " + Mathf.Max(0, vida).ToString("F0");
    }

    public void ActivarVidaBossUI() {
        if (vidaBossTexto != null) vidaBossTexto.gameObject.SetActive(true);
    }

    public void FinalizarJuego(bool victoria) {
        if (juegoTerminado) return; // Seguridad para no llamar dos veces
        
        juegoTerminado = true;
        Time.timeScale = 0f; // Pausa el juego
        
        if (panelFinal != null) panelFinal.SetActive(true);
        
        textoMensaje.text = victoria ? "VICTORY\nPresiona enter para reiniciar\nesc para salir" : "GAME OVER\nPresiona enter para reiniciar\nesc para salir";
        textoMensaje.color = victoria ? Color.green : Color.red;
    }
}