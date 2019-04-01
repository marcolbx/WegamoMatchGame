using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryTablero : MonoBehaviour
{
    public int ancho, largo;
    //public Tablero tablero;
    //GameObject tablero;
    public GameObject Tablero;
    public GameObject P_CasillaStandard;
    public GameObject[] fichasPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        GetTablero(ancho, largo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Inicializara las casillas y sus vecinas, para crear el tablero
    public void GetTablero(int x, int y)
    {
        GameObject tablero = Instantiate(Tablero) as GameObject; //Metodo 2
        tablero.transform.position = new Vector3 (0, 0, 0);
        tablero.name = "Tablero";
        tablero.GetComponent<Tablero>().ancho = x; //Metodo 2
        tablero.GetComponent<Tablero>().largo = y; //Metodo 2


     //   tablero = new Tablero(x,y,P_CasillaStandard); //Metodo 1
     //   tablero.fichasPrefabs = this.fichasPrefabs; //Metodo 1
     //   tablero.LlenarRandom(); //Metodo 1
        
    }
}
