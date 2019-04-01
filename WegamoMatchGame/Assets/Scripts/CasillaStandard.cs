using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasillaStandard : Casilla
{
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if(tablero != null)
        {
            tablero.SeleccionarCasilla(this);
        }
    }

    void OnMouseEnter()
    {
        if(tablero != null)
        {
            tablero.ArrastradaCasilla(this);
        }
    }

    void OnMouseUp()
    {
        if(tablero != null)
        {
            tablero.AlSoltarCasilla();
        }
    }
}
