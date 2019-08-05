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

    public override void OnMouseDown()
    {
        if(tablero != null)
        {
            tablero.SeleccionarCasilla(this);
            tablero.particleSystem.Play();
            tablero.particleSystem.transform.position = this.transform.position;
        }
    }

    public override void OnMouseEnter()
    {
        if(tablero != null)
        {
            tablero.ArrastradaCasilla(this);
        }
    }

    public override void OnMouseUp()
    {
        if(tablero != null)
        {
            //Animacion
            Ficha ficha2 = this.GetFicha();
            AnimationScript animationS = ficha2.gameObject.GetComponent<AnimationScript>();
            animationS.rotationSpeed = 10f;
            tablero.particleSystem.Stop();
            tablero.AlSoltarCasilla();
        }
    }
}
