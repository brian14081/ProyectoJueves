using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8.5f;

    [Header("Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float tiempoEntreDisparos = 0.25f;
    public int danio = 1;
    private float proximoDisparo = 0f;

    [Header("Estadísticas")]
    public int vidas = 3;

    [Header("Inventario de Poderes (Cola de 3)")]
    public List<TipoPoder> colaPoderes = new List<TipoPoder>();
    public float duracionPoderes = 5f;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoVidas;
    public GameObject panelMejoras;
    public GameObject cartelGameOver;

    [Header("UI: Cola de Imágenes")]
    public Image[] slotsPoderes;
    public Sprite spriteNuke;
    public Sprite spriteTriple;
    public Sprite spriteEscudo;
    public Sprite spriteVelocidad;

    private Rigidbody2D rb;
    private SpriteRenderer naveRenderer;
    private Vector2 movement;
    private Vector2 mousePos;
    private bool isDead = false;
    private bool isPaused = false;

    // NUEVO: Contadores de cargas para que los poderes se puedan acumular de forma segura
    private int cargasEscudo = 0;
    private int cargasTriple = 0;
    private int cargasVelocidad = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        naveRenderer = GetComponent<SpriteRenderer>();
        ActualizarUI();
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Escape) && (panelMejoras == null || !panelMejoras.activeSelf))
        {
            if (isPaused) ReanudarJuegoPausa();
            else PausarJuego();
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0) && Time.time >= proximoDisparo && Time.timeScale > 0)
        {
            Shoot();
            proximoDisparo = Time.time + tiempoEntreDisparos;
        }

        if (Input.GetKeyDown(KeyCode.Space) && colaPoderes.Count > 0 && Time.timeScale > 0)
        {
            ActivarPrimerPoder();
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // NUEVO: Calculamos la velocidad sobre la marcha. 
        // Tu moveSpeed base JAMÁS se modifica permanentemente.
        float velocidadActual = moveSpeed;
        if (cargasVelocidad > 0) velocidadActual += 6f;

        rb.linearVelocity = movement.normalized * velocidadActual;
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void Shoot()
    {
        if (cargasTriple > 0) // Revisamos el contador en vez del booleano
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -15f));
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 15f));
        }
        else
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void RecibirDano()
    {
        if (isDead) return;

        if (cargasEscudo > 0) return; // Si tenemos al menos 1 escudo, bloqueamos el daño

        vidas--;
        ActualizarUI();

        if (vidas <= 0)
        {
            isDead = true;
            Time.timeScale = 0f;
            naveRenderer.enabled = false;
            if (cartelGameOver != null) cartelGameOver.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PowerUp"))
        {
            ItemDrop itemInfo = collision.GetComponent<ItemDrop>();
            if (itemInfo != null)
            {
                if (colaPoderes.Count >= 3) ActivarPrimerPoder();
                colaPoderes.Add(itemInfo.tipo);
            }
            Destroy(collision.gameObject);
            ActualizarUI();
        }
        else if (collision.CompareTag("Vida"))
        {
            if (vidas < 3)
            {
                vidas++;
                ActualizarUI();
            }
            Destroy(collision.gameObject);
        }
    }

    void ActivarPrimerPoder()
    {
        if (colaPoderes.Count == 0) return;

        TipoPoder poderAUsar = colaPoderes[0];
        colaPoderes.RemoveAt(0);

        switch (poderAUsar)
        {
            case TipoPoder.Nuke: EjecutarNuke(); break;
            case TipoPoder.TripleShot: StartCoroutine(EfectoTripleShot()); break;
            case TipoPoder.Escudo: StartCoroutine(EfectoEscudo()); break;
            case TipoPoder.Velocidad: StartCoroutine(EfectoVelocidad()); break;
        }

        ActualizarUI();
    }

    // ==========================================
    // CORRUTINAS (Ahora con sistema de cargas)
    // ==========================================

    IEnumerator EfectoEscudo()
    {
        cargasEscudo++;
        ActualizarColorNave();
        yield return new WaitForSeconds(duracionPoderes);
        cargasEscudo--;
        ActualizarColorNave();
    }

    IEnumerator EfectoVelocidad()
    {
        cargasVelocidad++;
        ActualizarColorNave();
        yield return new WaitForSeconds(duracionPoderes);
        cargasVelocidad--;
        ActualizarColorNave();
    }

    IEnumerator EfectoTripleShot()
    {
        cargasTriple++;
        ActualizarColorNave();
        yield return new WaitForSeconds(duracionPoderes);
        cargasTriple--;
        ActualizarColorNave();
    }

    // NUEVO: Función para resolver peleas de colores si usas varios poderes
    void ActualizarColorNave()
    {
        // Jerarquía de colores (el escudo pisa a la velocidad, la velocidad pisa al triple tiro)
        if (cargasEscudo > 0) naveRenderer.color = Color.cyan;
        else if (cargasVelocidad > 0) naveRenderer.color = Color.yellow;
        else if (cargasTriple > 0) naveRenderer.color = new Color(1f, 0.5f, 0f);
        else naveRenderer.color = Color.white;
    }

    void EjecutarNuke()
    {
        GameObject[] enemigosEnPantalla = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigosEnPantalla)
        {
            Destroy(enemigo);
        }
    }

    // ==========================================
    // UI Y SISTEMA GENERAL
    // ==========================================

    void ActualizarUI()
    {
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;

        if (slotsPoderes == null) return;

        for (int i = 0; i < slotsPoderes.Length; i++)
        {
            if (i < colaPoderes.Count)
            {
                slotsPoderes[i].enabled = true;
                switch (colaPoderes[i])
                {
                    case TipoPoder.Nuke: slotsPoderes[i].sprite = spriteNuke; break;
                    case TipoPoder.TripleShot: slotsPoderes[i].sprite = spriteTriple; break;
                    case TipoPoder.Escudo: slotsPoderes[i].sprite = spriteEscudo; break;
                    case TipoPoder.Velocidad: slotsPoderes[i].sprite = spriteVelocidad; break;
                }
            }
            else
            {
                slotsPoderes[i].enabled = false;
            }
        }
    }

    public void MejorarVelocidadNave() { moveSpeed += 2f; ReanudarJuegoMejoras(); }
    public void MejorarVelocidadDisparo() { tiempoEntreDisparos -= 0.1f; if (tiempoEntreDisparos < 0.1f) tiempoEntreDisparos = 0.1f; ReanudarJuegoMejoras(); }
    public void MejorarDanio() { danio += 1; ReanudarJuegoMejoras(); }
    void ReanudarJuegoMejoras() { if (panelMejoras != null) panelMejoras.SetActive(false); Time.timeScale = 1f; }
    public void ReiniciarJuego() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    void PausarJuego() { isPaused = true; Time.timeScale = 0f; }
    void ReanudarJuegoPausa() { isPaused = false; Time.timeScale = 1f; }
}