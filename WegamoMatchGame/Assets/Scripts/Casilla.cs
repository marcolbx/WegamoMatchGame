using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Casilla : MonoBehaviour
{
    public int x, y;
    public Tablero tablero;
    private List<Casilla> vecinas = new List<Casilla>();
    private Dictionary<int, Casilla> vecinasxdireccion;
    private Ficha ficha;
    private bool comprobanteExistencia = false;

    // Start is called before the first frame update
    void Start()
    {
       // Casilla temp = new Casilla();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool HasMatch(Matcher matcher)
    {
        return matcher.HasMatch(this);
    }

    public virtual Casilla GetCasilla(int direccion)
    {
            if (vecinasxdireccion[direccion]!=null)
            {
                comprobanteExistencia = true;
                return vecinasxdireccion[direccion];
            }
            else
            {
            Debug.Log("No existe la key ingresada en el dictionary " + this.name);
            return null;
            }
    }

    public virtual void SetCoordenadas(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public virtual void AddVecina(Casilla casilla)
    {
        vecinas.Add(casilla);
    }

    public virtual Ficha GetFicha()
    {
        return this.ficha;
    }

    public virtual void SetFicha(Ficha ficha)
    {
        this.ficha = ficha;
    }

    public virtual bool SameFicha(Casilla casilla)
    {
        return this.GetFicha().equals(casilla.GetFicha());
    }

    public virtual void OnMouseDown()
    {
        if(tablero != null)
        {
            tablero.SeleccionarCasilla(this);
        }
    }

    public virtual void OnMouseEnter()
    {
        if(tablero != null)
        {
            tablero.ArrastradaCasilla(this);
        }
    }

    public virtual void OnMouseUp()
    {
        if(tablero != null)
        {
            //Parar animacion una vez soltado el click
           // ficha = this.GetFicha();
           // AnimationScript animationS = ficha.gameObject.GetComponent<AnimationScript>();
           // animationS.rotationSpeed = 10f;
            tablero.AlSoltarCasilla();
        }
    }
}
