using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    public int ancho, largo;  //Define las dimensiones del tablero. x,y
    private int tamanoBorde = 2; //Permite alejar el tablero de la camara. Funciona como un padding
    public GameObject P_CasillaStandard; //Casillas a instanciar
    public GameObject[] fichasPrefabs;
    Casilla[,] casillas; //Tiene toda la lista de las casillas

    private Casilla casillaSeleccionada;
    private Casilla casillaSeleccionada2;
    public float tiempoCambio = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        casillas = new Casilla[ancho, largo];
        CreacionCasillas();
        Debug.Log("Salio de creacionCasillas()");
        ArreglarCamera();
        InicializarVecinas();
        LlenarRandom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tablero(int ancho, int largo, GameObject P_CasillaStandard)
    {
       // Debug.Log("Entre en el constructor con parametros");
       // Debug.Log("ancho:" + ancho + " largo: " + largo);
        this.ancho = ancho;
        this.largo = largo;
        this.casillas = new Casilla[ancho, largo];
        this.P_CasillaStandard = P_CasillaStandard;
       // Debug.Log("Entrando a creacionCasillas()");
        CreacionCasillas();
    }

    public void CreacionCasillas()
    {
        if (ancho != null && largo != null)
        {
            for (int i = 0; i < ancho; i++)
            {
                for (int j = 0; j < largo; j++)
                {
                    GameObject casilla = Instantiate(P_CasillaStandard, new Vector3(i, j, 0), Quaternion.identity) as GameObject;
                    casilla.name = "Casilla (" + i + "," + j + ")";
                    casilla.GetComponent<CasillaStandard>().SetCoordenadas(i, j);
                    casilla.GetComponent<Casilla>().tablero = this;
                    casillas[i, j] = casilla.GetComponent<CasillaStandard>();
                    casilla.transform.parent = transform;
                }
            }
        }
    }

    void InicializarVecinas()
    {
        Debug.Log("Entro en InicializarVecinas");
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                int count = 0;
                if(i<ancho - 1)
                if (casillas[i + 1, j] != null)
                {
                    casillas[i, j].AddVecina(casillas[i + 1, j]);
                        count++;
                }
                if (i > 0)
                {
                    if (casillas[i - 1, j] != null)
                    {
                        casillas[i, j].AddVecina(casillas[i - 1, j]);
                        count++;
                    }
                }
                if(j<largo - 1)
                if (casillas[i, j + 1] != null)
                {
                    casillas[i, j].AddVecina(casillas[i, j + 1]);
                        count++;
                    }
                if (j > 0)
                {
                    if (casillas[i, j - 1] != null)
                    {
                        casillas[i, j].AddVecina(casillas[i, j - 1]);
                        count++;
                    }
                    
                }
            }
        }
    }

    void ArreglarCamera()
    {
       // Debug.Log("Entro en ArreglarCamera");
        Camera.main.transform.position = new Vector3((float)(ancho - 1) / 2f, (float)(largo - 1) / 2f, -10f);
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float verticalSize = (float)largo *1.5f / 2f + (float)tamanoBorde; //Tuve que multiplicarlo por 1.5f para arreglar el borde vertical
        float horizontalSize = ((float)ancho / 2f + (float)tamanoBorde) / aspectRatio;
        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize; // if else 
    }

    GameObject ObtenerFichaRandom()
    {
   // Debug.Log("Entro en ObtenerFichaRandom");
    int randomIndex = Random.Range(0, fichasPrefabs.Length);
    if (fichasPrefabs[randomIndex] == null)
        Debug.LogWarning("Tablero [" + randomIndex + "] no tiene una ficha");
    return fichasPrefabs[randomIndex];
    }

    public void ColocarFicha(Ficha ficha, Casilla casilla)
    {
    if (ficha == null)
    {
        Debug.LogWarning("Tablero: Ficha Invalida");
        return;
    }
    ficha.transform.position = new Vector3(casilla.x, casilla.y, 0);
    //ficha.transform.rotation = Quaternion.identity; // Si se quiere que esten en rotacion (0,0,0);
    Quaternion target = Quaternion.Euler(-110, 0, 0); //Se ve bonito con -110
    ficha.transform.rotation = target; 
    casilla.SetFicha(ficha);
    Debug.Log(casilla.name + "Tiene la ficha: " + casilla.GetFicha().name);
        //ficha.SetCoord(x, y);
        //m_allGamePieces[x,y] = gamepiece;
    }

    public void LlenarRandom()
    {
    for (int i = 0; i < ancho; i++)
    {
        for (int j = 0; j < largo; j++)
        {
            Casilla casilla = casillas[i,j];
            GameObject fichaRandom = Instantiate(ObtenerFichaRandom(), Vector3.zero, Quaternion.identity) as GameObject;
        //    Debug.Log("Instanciada FichaRandom");
            if (fichaRandom != null)
            {
                ColocarFicha(fichaRandom.GetComponent<Ficha>(), casilla);
                fichaRandom.GetComponent<Ficha>().tablero = this;
            }
        }
    }
    }

    public void SeleccionarCasilla(Casilla casilla)
    {
        if(casillaSeleccionada == null)
        {
            casillaSeleccionada = casilla;
            Debug.Log("Casilla seleccionada: " + casilla.name);

            //Animacion de la ficha
            Ficha fichaSeleccionada = casillaSeleccionada.GetFicha();
            AnimationScript animationS = fichaSeleccionada.gameObject.GetComponent<AnimationScript>();
            animationS.rotationSpeed=60f;
        }
    }

    public void ArrastradaCasilla(Casilla casilla)
    {
        if(casillaSeleccionada != null)
        {
            casillaSeleccionada2 = casilla;
           // Ficha fichaSeleccionada2 = casillaSeleccionada2.GetFicha();
           // AnimationScript animationS = fichaSeleccionada2.gameObject.GetComponent<AnimationScript>();
           // animationS.rotationSpeed=60f;
        }
    }

    public void AlSoltarCasilla()
    {
        if(casillaSeleccionada != null && casillaSeleccionada2 != null)
        {
            Debug.Log("Entro en el if de AlSoltarCasilla() "+this.name);
        CambiarCasilla(casillaSeleccionada,casillaSeleccionada2);
        }
        casillaSeleccionada = null;
        casillaSeleccionada2 = null;
    }

    //Esto no esta funcionando
    /*
    public void CambiarCasilla(Casilla casillaSeleccionada, Casilla casillaSeleccionada2)
    {
        Ficha fichaSeleccionada = casillaSeleccionada.GetFicha();
        Ficha fichaSeleccionada2 = casillaSeleccionada2.GetFicha();

        fichaSeleccionada.Moverse(casillaSeleccionada2, tiempoCambio);
        fichaSeleccionada2.Moverse(casillaSeleccionada, tiempoCambio);
    }
    */
    
    public void CambiarCasilla(Casilla casillaSeleccionada, Casilla casillaSeleccionada2)
    {
        Debug.Log("CambiarCasilla: Entrando a CambiarCasillaRutina");
        StartCoroutine(CambiarCasillaRutina(casillaSeleccionada,casillaSeleccionada2));
    }


    IEnumerator CambiarCasillaRutina(Casilla casillaSeleccionada, Casilla casillaSeleccionada2)
    {
        Debug.Log("CambiarCasillaRutina");
        Ficha fichaSeleccionada = casillaSeleccionada.GetFicha();
        Ficha fichaSeleccionada2 = casillaSeleccionada2.GetFicha();
        AnimationScript animationS = fichaSeleccionada.gameObject.GetComponent<AnimationScript>();
        animationS.rotationSpeed=60f;

        if(fichaSeleccionada!= null && fichaSeleccionada2!= null){
        fichaSeleccionada.Moverse(casillaSeleccionada2, tiempoCambio);
        fichaSeleccionada2.Moverse(casillaSeleccionada, tiempoCambio);
        yield return new WaitForSeconds(tiempoCambio);
        Debug.Log("CambiarCasillaRutina luego del yield return new WaitForSeconds");
        casillaSeleccionada.SetFicha(fichaSeleccionada2);
        casillaSeleccionada2.SetFicha(fichaSeleccionada);
        animationS.rotationSpeed=10f;
        }
    }

    
}