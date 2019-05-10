using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tablero : MonoBehaviour
{
    public int ancho, largo;  //Define las dimensiones del tablero. x,y
    private int tamanoBorde = 2; //Permite alejar el tablero de la camara. Funciona como un padding
    public GameObject P_CasillaStandard; //Casillas a instanciar
    public GameObject[] fichasPrefabs; //Fichas a instanciar
    public int minimoParaMatch = 3;
    Casilla[,] casillas; //Tiene toda la lista de las casillas

    private Casilla casillaSeleccionada;
    private Casilla casillaSeleccionada2;
    public float tiempoCambio = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        casillas = new Casilla[ancho, largo];
        CreacionCasillas();
        ArreglarCamera();
        InicializarVecinas();
        LlenarRandom();
       // HighlightMatches();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tablero(int ancho, int largo, GameObject P_CasillaStandard)
    {
       // Debug.Log("ancho:" + ancho + " largo: " + largo);
        this.ancho = ancho;
        this.largo = largo;
        this.casillas = new Casilla[ancho, largo];
        this.P_CasillaStandard = P_CasillaStandard;
       // Debug.Log("Entrando a creacionCasillas()");
        CreacionCasillas();
    }

    /**
     * CreacionCasillas() permite crear la cantidad de casillas mediante el ancho y largo introducido.
     */
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

    /**
     * InicializarVecina() permite a las casillas saber cuales casillas tienen a los lados.
     */
    void InicializarVecinas()
    {
     //   Debug.Log("Entro en InicializarVecinas");
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

    /**
     * ObtenerFichaRandom() obtiene una ficha aleatoriamente del Array de fichasPrefabs.
     */
    GameObject ObtenerFichaRandom()
    {
   // Debug.Log("Entro en ObtenerFichaRandom");
    int randomIndex = Random.Range(0, fichasPrefabs.Length);
    if (fichasPrefabs[randomIndex] == null)
        Debug.LogWarning("Tablero [" + randomIndex + "] no tiene una ficha");
    return fichasPrefabs[randomIndex];
    }

    /**
     * ColocarFicha() le agrega la Ficha como atributo a la clase Casilla.
     */
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
   // Debug.Log(casilla.name + "Tiene la ficha: " + casilla.GetFicha().name);
    }

    public void InicializarRotacion(Ficha ficha)
    {
        //ficha.transform.rotation = Quaternion.identity; // Si se quiere que esten en rotacion (0,0,0);
        Quaternion target = Quaternion.Euler(-110, 0, 0); //Se ve bonito con -110
        ficha.transform.rotation = target;
    }

    /**
     * LlenarRandom() llenara las casillas con fichas aleatorias.
     */
    public void LlenarRandom()
    {
    for (int i = 0; i < ancho; i++)
    {
        for (int j = 0; j < largo; j++)
        {
            Casilla casilla = casillas[i,j];
            GameObject fichaRandom = Instantiate(ObtenerFichaRandom(), Vector3.zero, Quaternion.identity) as GameObject;
            if (fichaRandom != null)
            {
                ColocarFicha(fichaRandom.GetComponent<Ficha>(), casilla);
              //  fichaRandom.GetComponent<Ficha>().tablero = this;
            }
        }
    }
    }

    /**
     * SeleccionarCasilla(Casilla ), se comunica con el metodo de mouseinput de casilla.
     */
    public void SeleccionarCasilla(Casilla casilla)
    {
        if(casillaSeleccionada == null)
        {
            casillaSeleccionada = casilla;
            Debug.Log("Casilla Seleccionada: " + casillaSeleccionada.name + "Ficha Seleccionada: " + casillaSeleccionada.GetFicha() + casillaSeleccionada.GetFicha().GetValor());
            AnimarCasilla(casillaSeleccionada);
        }
    }

    /**
     * ArrastradaCasilla(Casilla ), se comunica con el metodo de mouseinput de casilla.
     */
    public void ArrastradaCasilla(Casilla casilla)
    {
        if(casillaSeleccionada != null)
        {
            casillaSeleccionada2 = casilla;
        }
    }

    /**
     * AlSoltarCasilla(), se comunica con el metodo de mouseinput de casilla. En caso de tener 2 casillas seleccionadas, llama al cambio.
     */
    public void AlSoltarCasilla()
    {
        if(casillaSeleccionada != casillaSeleccionada2)
        if(casillaSeleccionada != null && casillaSeleccionada2 != null)
        {
                Debug.Log("Casilla Seleccionada2: " + casillaSeleccionada2.name + "Ficha Seleccionada: " + casillaSeleccionada2.GetFicha() + casillaSeleccionada2.GetFicha().GetValor());
                if (Vecinas2(casillaSeleccionada, casillaSeleccionada2)) 
                CambiarCasilla(casillaSeleccionada,casillaSeleccionada2);
        }
        casillaSeleccionada = null;
        casillaSeleccionada2 = null;
    }

    /**
     * CambiarCasilla() cambia las casillas seleccionadas por el usuario.
     */
    public void CambiarCasilla(Casilla casillaSeleccionada, Casilla casillaSeleccionada2)
    {
    //    Debug.Log("CambiarCasilla: Entrando a CambiarCasillaRutina");
        StartCoroutine(CambiarCasillaRutina(casillaSeleccionada,casillaSeleccionada2));
    }


    /**
     * CambiarCasillaRutina() Permite usar deltaTime para manejar el tiempo de cambio entre casillas.
     */
    IEnumerator CambiarCasillaRutina(Casilla casillaSeleccionada, Casilla casillaSeleccionada2)
    {

        Ficha fichaSeleccionada = casillaSeleccionada.GetFicha();
        Ficha fichaSeleccionada2 = casillaSeleccionada2.GetFicha();
        Debug.Log("Casilla Seleccionada: " + casillaSeleccionada.name + "Ficha Seleccionada: " + fichaSeleccionada + fichaSeleccionada.GetValor());
        Debug.Log("Casilla Seleccionada2: " + casillaSeleccionada2.name + "Ficha Seleccionada2: " + fichaSeleccionada2 + fichaSeleccionada2.GetValor());

        if (fichaSeleccionada!= null && fichaSeleccionada2!= null){
            fichaSeleccionada.Moverse(casillaSeleccionada2, tiempoCambio);
            fichaSeleccionada2.Moverse(casillaSeleccionada, tiempoCambio);
            yield return new WaitForSeconds(tiempoCambio);
            
            casillaSeleccionada.SetFicha(fichaSeleccionada2);
            casillaSeleccionada2.SetFicha(fichaSeleccionada);
            HighlightMatchesCasillas(casillaSeleccionada);
            HighlightMatchesCasillas(casillaSeleccionada2);
            Debug.Log("NUEVOS Casilla Seleccionada: " + casillaSeleccionada.name + "Ficha Seleccionada: " + casillaSeleccionada.GetFicha() + casillaSeleccionada.GetFicha().GetValor());
            Debug.Log("NUEVOS Casilla Seleccionada2: " + casillaSeleccionada2.name + "Ficha Seleccionada2: " + casillaSeleccionada2.GetFicha() + casillaSeleccionada2.GetFicha().GetValor());
            var totalMatches = EncontrarTodosLosMatchesSinDiagonales(casillas[casillaSeleccionada.x, casillaSeleccionada.y]);
            var totalMatches2 = EncontrarTodosLosMatchesSinDiagonales(casillas[casillaSeleccionada2.x, casillaSeleccionada2.y]);

           // HighlightMatches();
            yield return new WaitForSeconds(tiempoCambio + 0.5f);
            //HighlightDesactivarTodosMatches();

            if (totalMatches.Count == 0 && totalMatches2.Count == 0)
            {
                fichaSeleccionada = casillaSeleccionada.GetFicha();
                fichaSeleccionada2 = casillaSeleccionada2.GetFicha();
                fichaSeleccionada.Moverse(casillaSeleccionada2, tiempoCambio);
                fichaSeleccionada2.Moverse(casillaSeleccionada, tiempoCambio);
                yield return new WaitForSeconds(tiempoCambio);

                casillaSeleccionada.SetFicha(fichaSeleccionada2);
                casillaSeleccionada2.SetFicha(fichaSeleccionada);
            }
            else
            {

                if (totalMatches.Count > 0)
                {
                    foreach (Casilla casilla in totalMatches)
                    {
                        EliminarPiezaCasilla(casilla);
                    }
                    

                }
                if (totalMatches2.Count > 0)
                {
                    foreach (Casilla casilla in totalMatches2)
                    {
                        EliminarPiezaCasilla(casilla);
                    }
                    
                }

                yield return new WaitForSeconds(tiempoCambio);
                var combinedMatches = totalMatches.Union(totalMatches2).ToList();
                GravedadEnColumnas(combinedMatches);
                if (totalMatches.Count > 0)
                    HighlightDesactivar(totalMatches);
                if (totalMatches2.Count > 0)
                    HighlightDesactivar(totalMatches2);
                //  HighlightMatches();
                yield return new WaitForSeconds(tiempoCambio);
                
            }
        }
        RefillBoard();
    }

    public void AnimarCasilla(Casilla casilla)
    {
        Ficha ficha = casilla.GetFicha();
        AnimationScript animationS = ficha.gameObject.GetComponent<AnimationScript>();
        animationS.rotationSpeed = 60f;
    }
    public void AnimarCasillaMinimizarFicha(List<Casilla> casillas)
    {
        foreach (Casilla casilla in casillas)
        {
            Ficha ficha = casilla.GetFicha();
            ficha.transform.localScale -= new Vector3(0.05f, 0.05f, 0);
        }
    }

    public void DesanimarCasilla(Casilla casilla)
    {
        Ficha ficha = casilla.GetFicha();
        if (casilla.GetFicha() != null)
        {
            AnimationScript animationS = ficha.gameObject.GetComponent<AnimationScript>();
            animationS.rotationSpeed = 10f;
        }
    }

    bool DentroDeLimites(int x, int y)
    {
        return (x >= 0 && x < ancho && y >= 0 && y < largo);
    }

    //Funcionales
    List<Casilla> EncontrarMatches(Casilla casilla, Vector2 direccionBusqueda, int minimo)
    {
        List<Casilla> matches = new List<Casilla>();
        Ficha fichaInicial = null;

        if (DentroDeLimites(casilla.x, casilla.y))
        {
            fichaInicial = casilla.GetFicha();
        }
        
       // if (casilla.GetFicha())
        //    fichaInicial = casilla.GetFicha();

        if (fichaInicial != null)
        {
            matches.Add(casilla);
        }
        else
        {
            return null;
        }

        int proximoX;
        int proximoY;
        int valorMaximo = (ancho > largo) ? ancho : largo;

        for(int i = 1; i < valorMaximo; i++)
        {
            proximoX = casilla.x + (int)Mathf.Clamp(direccionBusqueda.x, -1, 1) * i;
            proximoY = casilla.y + (int)Mathf.Clamp(direccionBusqueda.y, -1, 1) * i;
            if (!DentroDeLimites(proximoX, proximoY))
            {
                break;
            }
            if (casillas[proximoX, proximoY].GetFicha() == null)
                break;
            if (casilla.SameFicha(casillas[proximoX, proximoY]))
            {
                matches.Add(casillas[proximoX, proximoY]);
            }
            else
                break;
        }
        if (matches.Count >= minimo)
        {
         //   Debug.Log("Entromatch.Size()>= minimo (METODO) EncontrarMatches. Devolviendo un match mayor de 2");
         //   Debug.Log("EntroMatch.Size()= " + matches.Count);
            return matches;
        }
        return null;
    }

    List<Casilla> EncontrarMatchesVerticales(Casilla casilla, int minLength = 3)
    {
        int startX = casilla.x;
        int startY = casilla.y;
     //   Debug.Log("Entrando a upwardMatches (METODO: FindVerticalMatches)");
        List<Casilla> upwardMatches = EncontrarMatches(casilla, new Vector2(0, 1), 2);
        
     //   Debug.Log("Entrando a downwardMatches (METODO: FindVerticalMatches)");

        List<Casilla> downwardMatches = EncontrarMatches(casilla, new Vector2(0, -1), 2);

        if (upwardMatches == null)
            upwardMatches = new List<Casilla>();
        if (downwardMatches == null)
            downwardMatches = new List<Casilla>();
/*
        if(upwardMatches != null)
        {
            Debug.Log("upwardMatches.Size()=" + upwardMatches.Count);
        }*/

     var resultado = upwardMatches.Union(downwardMatches).ToList();
      //  resultado = UnirMatches(upwardMatches, downwardMatches);
       // Debug.Log("XYZDevolviendo2 matchResultado, #Casillas= " + resultado.Size());
        //var combinedMatches = upwardMatches.Union(downwardMatches).ToList();
       // return (resultado.Size() >= minLength) ? resultado : null;
       return (resultado.Count >= minLength) ? resultado : null; //Devuelve la lista de Matches si son > de 3, sino devuelve null
    }

    List<Casilla> EncontrarMatchesHorizontales(Casilla casilla, int minLength = 3)
    {
        int startX = casilla.x;
        int startY = casilla.y;
     //   Debug.Log("Entrando a upwardMatches (METODO: FindVerticalMatches)");
        List<Casilla> rightMatches = EncontrarMatches(casilla, new Vector2(1, 0), 2);

     //   Debug.Log("Entrando a downwardMatches (METODO: FindVerticalMatches)");

        List<Casilla> leftMatches = EncontrarMatches(casilla, new Vector2(-1, 0), 2);

        if (rightMatches == null)
            rightMatches = new List<Casilla>();
        if (leftMatches == null)
            leftMatches = new List<Casilla>();
/*
        if (rightMatches != null)
        {
            Debug.Log("upwardMatches.Size()=" + rightMatches.Count);
        }*/

        var resultado = rightMatches.Union(leftMatches).ToList();
        //  resultado = UnirMatches(upwardMatches, downwardMatches);
        // Debug.Log("XYZDevolviendo2 matchResultado, #Casillas= " + resultado.Size());
        //var combinedMatches = upwardMatches.Union(downwardMatches).ToList();
        // return (resultado.Size() >= minLength) ? resultado : null;
        return (resultado.Count >= minLength) ? resultado : null; //Devuelve la lista de Matches si son > de 3, sino devuelve null
    }

    List<Casilla> EncontrarTodosLosMatchesSinDiagonales(Casilla casilla)
    {
        List<Casilla> horizMatches = EncontrarMatchesHorizontales(casilla, 3);
        List<Casilla> vertMatches = EncontrarMatchesVerticales(casilla, 3);

        if (horizMatches == null)
            horizMatches = new List<Casilla>();
        if (vertMatches == null)
            vertMatches = new List<Casilla>();

        var combinedMatches = horizMatches.Union(vertMatches).ToList();
        return combinedMatches;
    }

    void HighlightCasilla(Casilla casilla)
    {
        SpriteRenderer spriteRenderer = casillas[casilla.x, casilla.y].GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
    }

    void HighlightDesactivar(List<Casilla> casillas)
    {
        foreach (Casilla casilla in casillas)
        {
            SpriteRenderer spriteRenderer = casilla.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
        }
    }

    void HighlightMatches()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                SpriteRenderer spriteRenderer = casillas[i, j].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
                DesanimarCasilla(casillas[i, j]);
                var totalMatches = EncontrarTodosLosMatchesSinDiagonales(casillas[i, j]);

                if (totalMatches.Count > 0)
                {
                    foreach (Casilla casilla in totalMatches)
                    {
                        HighlightCasilla(casilla);
                        AnimarCasilla(casilla);
                    }
                }
            }
        }
    }

    void HighlightMatchesCasillas(Casilla casilla)
    {
        var matches = EncontrarTodosLosMatchesSinDiagonales(casilla);
        if(matches.Count > 0)
        {
            foreach (Casilla casilla2 in matches)
            {
                HighlightCasilla(casilla2);
                AnimarCasilla(casilla2);
            }
        }
    }

    void HighlightDesactivarTodosMatches()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                SpriteRenderer spriteRenderer = casillas[i, j].GetComponent<SpriteRenderer>();
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
                DesanimarCasilla(casillas[i, j]);
            }
        }
    }

    void EliminarPiezaCasilla(Casilla casilla)
    {
        Ficha ficha = casilla.GetFicha();
        if (ficha != null)
        {
            Destroy(ficha.gameObject);
            casilla.SetFicha(null);
        }
    }

    void EliminarPiezaCasillas(List<Casilla> casillas)
    {
        foreach (Casilla casilla in casillas)
        {
            EliminarPiezaCasilla(casilla);
        }
    }

    void EliminarMatchesTablero()
    {
        for (int i = 0; i < ancho; i++)
        {
            for (int j = 0; j < largo; j++)
            {
                EliminarPiezaCasilla(casillas[i, j]);
            }
        }
    }
    /*
     * Vecinas devuelve true si en efecto son vecinas. Usando las posiciones de las casillas. Forma 1 de hacerlo.
     */
    bool Vecinas(Casilla casilla1, Casilla casilla2)
    {
        if (Mathf.Abs(casilla1.x - casilla2.x) == 1 && casilla1.y == casilla2.y)
            return true;
        if (Mathf.Abs(casilla1.y - casilla2.y) == 1 && casilla1.x == casilla2.x)
            return true;

        return false;
    }

    /*
     * Vecinas2 devuelve true si en efecto son vecinas. Usando el atributo vecinas de las casillas. Forma 2 de hacerlo.
     */
    bool Vecinas2(Casilla casilla1, Casilla casilla2)
    {
        if (casilla1.vecinas.Contains(casilla2))
            return true;
        else
            return false;
    }


    List<Casilla> GravedadEnColumnas(int columna, float tiempo = 0.1f)
    {
        List<Casilla> casillasEnMovimiento = new List<Casilla>();
        for (int i = 0; i<largo - 1; i++)
        {
            if(casillas[columna,i].GetFicha() == null)
            {
                for(int j = i+1; j < largo; j++)
                {
                    if (casillas[columna, j].GetFicha() != null)
                    {
                        casillas[columna, j].GetFicha().Moverse(casillas[columna, i], tiempo);
                        casillas[columna, i].SetFicha(casillas[columna, j].GetFicha());
                        if (!casillasEnMovimiento.Contains(casillas[columna, i]))
                        {
                            casillasEnMovimiento.Add(casillas[columna, i]);
                        }
                        casillas[columna, j].SetFicha(null);
                        break;
                    }
                }
            }
        }
        return casillasEnMovimiento;
    }

    List<Casilla> GravedadEnColumnas(List<Casilla> casillas)
    {
        List<Casilla> casillasMovimiento = new List<Casilla>();
        List<int> columnasAAfectar = ObtenerLasColumnas(casillas);
        foreach(int columna in columnasAAfectar)
        {
            casillasMovimiento = casillasMovimiento.Union(GravedadEnColumnas(columna)).ToList();
        }
        return casillasMovimiento;
    }

    List<int> ObtenerLasColumnas(List<Casilla> casillas)
    {
        List<int> columnas = new List<int>();
        foreach(Casilla casilla in casillas)
        {
            if (!columnas.Contains(casilla.x))
                columnas.Add(casilla.x);
        }
        return columnas;
    }


    void RefillBoard()
    {
        StartCoroutine(RefillRutina());
    }

    IEnumerator RefillRutina()
    {
        List<GameObject> fichas = new List<GameObject>();
        List<Casilla> casillas = new List<Casilla>();
        fichas = GenerarFichasFaltantes();
        casillas = CasillasFaltantes();
        yield return new WaitForSeconds(tiempoCambio);
        int i = casillas.Count;
        foreach (GameObject ficha in fichas)
        {
            ficha.GetComponent<Ficha>().Moverse(casillas[i - 1], 0.45f);
            yield return new WaitForSeconds(0.45f);
            ColocarFicha(ficha.GetComponent<Ficha>(), casillas[i - 1]);
            i--;
        }
    }

    List<GameObject> GenerarFichasFaltantes()
    {
        List<GameObject> fichas = new List<GameObject>();
        
        foreach (Casilla casilla in casillas)
        {
            if (casilla.GetFicha() == null)
            {
                Vector3 position = new Vector3(casilla.x,largo + casilla.y - 1,0);
                GameObject fichaRandom = Instantiate(ObtenerFichaRandom(), position, Quaternion.identity) as GameObject;
                InicializarRotacion(fichaRandom.GetComponent<Ficha>());
                fichas.Add(fichaRandom);
            }
        }
        return fichas;
    }

    List<Casilla> CasillasFaltantes()
    {
        List<Casilla> casillas = new List<Casilla>();

        foreach (Casilla casilla in this.casillas)
        {
            if (casilla.GetFicha() == null)
            {
                casillas.Add(casilla);
            }
        }
        return casillas;
    }

    //No Funcionales

    /*
Match EncontrarMatches(Casilla casilla, Vector2 direccionBusqueda, int minimo)
{
    Match matches = new Match();
    Ficha fichaInicial = null;

    if (DentroDeLimites(casilla.x, casilla.y))
    {
        fichaInicial = casilla.GetFicha();
    }

    if (fichaInicial != null)
    {
        matches.AgregarCasilla(casilla);
    }
    else
    {
        return null;
    }

    int proximoX;
    int proximoY;
    int valorMaximo = (ancho > largo) ? ancho : largo;

    for (int i = 1; i < valorMaximo; i++)
    {
        proximoX = casilla.x + (int)Mathf.Clamp(direccionBusqueda.x, -1, 1) * i;
        proximoY = casilla.y + (int)Mathf.Clamp(direccionBusqueda.y, -1, 1) * i;
        if (!DentroDeLimites(proximoX, proximoY))
        {
            break;
        }
        if (casilla.SameFicha(casillas[proximoX, proximoY]))
        {
            matches.AgregarCasilla(casillas[proximoX, proximoY]);
        }
        else
            break;
    }
    if (matches.Size() >= minimo)
    {
        Debug.Log("Entromatch.Size()>= minimo (METODO) EncontrarMatches. Devolviendo un match mayor de 2");
        Debug.Log("XYZDevolviendo: " + matches.Size());
        return matches;
    }
    return null;
}

Match FindVerticalMatches(Casilla casilla, int minLength = 3)
{
    int startX = casilla.x;
    int startY = casilla.y;
    Debug.Log("Entrando a upwardMatches (METODO: FindVerticalMatches)");
    Match upwardMatches = new Match();
    upwardMatches = DeepCopy(EncontrarMatches(casilla, new Vector2(0, 1), 2));
    Debug.Log("Entrando a downwardMatches (METODO: FindVerticalMatches)");
    Match downwardMatches = new Match();
    upwardMatches = DeepCopy(EncontrarMatches(casilla, new Vector2(0, -1), 2));

    if (upwardMatches == null)
        upwardMatches = new Match();
    if (downwardMatches == null)
        downwardMatches = new Match();

    if (upwardMatches != null)
    {
        Debug.Log("XYZDevolviendo2:" + upwardMatches.Size());
    }
    if (downwardMatches != null)
    {
        Debug.Log("XYZDevolviendo2:" + downwardMatches.Size());
    }

    var resultado = upwardMatches.getCasillas().Union(downwardMatches.getCasillas()).ToList();
    Match matchCombinado = new Match();
    //  resultado = UnirMatches(upwardMatches, downwardMatches);
    // Debug.Log("XYZDevolviendo2 matchResultado, #Casillas= " + resultado.Size());
    //var combinedMatches = upwardMatches.Union(downwardMatches).ToList();
    // return (resultado.Size() >= minLength) ? resultado : null;
    matchCombinado.setCasillas(resultado);
    Debug.Log("XYZDevolviendo2.2: " + matchCombinado.Size());
    return (resultado.Count >= minLength) ? matchCombinado : null; //Devuelve la lista de Matches si son > de 3, sino devuelve null
}

static Match DeepCopy(Match existing)
{
    if (existing != null)
    {
        Match temp = new Match();
        temp = existing;
        temp.setCasillas(existing.getCasillas());
        return temp;
    }
    else return null;
}

void HighlightMatches()
{
    for (int i = 0; i < ancho; i++)
    {
        for (int j = 0; j < largo; j++)
        {
            SpriteRenderer spriteRenderer = casillas[i, j].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);

            //   List<GamePiece> horizMatches = FindHorizontalMatches(i, j, 3);
            Match vertMatches = FindVerticalMatches(casillas[i, j], 3);
            if (vertMatches != null)
                Debug.Log("XYZDevolviendo3 matchResultado, #Casillas= " + vertMatches.Size());
            //    if (horizMatches == null)
            //        horizMatches = new List<GamePiece>();
            if (vertMatches == null)
                vertMatches = new Match();
            //      var combinedMatches = horizMatches.Union(vertMatches).ToList();

            //Debug.Log("Entrando al if de vertMatches.Size()");
            if (vertMatches.Size() > 0)
            {
                Debug.Log("Entro en vertMatches.Size>0 (METODO HighlightMatches)");
                foreach (Casilla casilla in vertMatches.getCasillas())
                {
                    Debug.Log("Casilla con Match: " + casilla.name);
                    spriteRenderer = casillas[casilla.x, casilla.y].GetComponent<SpriteRenderer>();
                    spriteRenderer.color = new Color(0, 0, 0, 1);
                }
            }
        }
    }
}

static Match UnirMatches(List<Casilla> match, List<Casilla> match2)
{
    Debug.Log("XYZCasillasU match.Size()" + match.Count);
    Debug.Log("XYZCasillasU match2.Size()" + match2.Count);
    var casillasUnidas = match.Union(match2).ToList();
        Debug.Log("XYZCasillas Unidas: " + casillasUnidas.Count());
    Debug.Log("XYZCasillas Unidas: " + casillasUnidas.Count);
    Match matchResultado = new Match();
        matchResultado.setCasillas(casillasUnidas);
    Debug.Log("XYZDevolviendo1 matchResultado, #Casillas= " + matchResultado.Size());
        return matchResultado;
}

*/

}