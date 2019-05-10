using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ficha : MonoBehaviour
{
    public enum FichaValor {DIAMANTE,ESTRELLA,ROMBO,CORAZON,ESPIRAL}; //Define los valores posibles que puede tener una ficha
    public GameObject[] gameObjects;  //Se colocan los objetos que pertencen a cada uno. (Esto para el momento de renderizar el tipo de ficha)
    public FichaValor valor;         //El valor de la ficha
    public bool moviendo = false;          //Condicion que dice si la ficha se esta moviendo.
    public int x, y;
   // public Tablero tablero; //Momentaneamente
                            
    public FichaValor fichaValor;

    /**
    * interpolation permite el movimiento visual de las fichas.
    * 
    */
    public InterType interpolation = InterType.SmootherStep;

    public enum InterType
    {
        Linear,
        EaseOut,
        EaseIn,
        SmoothStep,
        SmootherStep
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual FichaValor GetValor()   //Devuelve el valor actual de la ficha 
    {
        return valor;
    }

    public virtual bool equals(Ficha ficha)  //Devuelve si el valor de la ficha es igual al de la ficha que se pasa por parametro.
    {
        return ficha.GetValor() == this.valor;
    }

    public virtual void Moverse(Casilla casilla, float tiempoMovimiento)
    {
        if(!moviendo)
        {
            StartCoroutine(RutinaMovimiento(casilla,tiempoMovimiento));
        }
    }

    /**
     * RutinaMovimiento permite la visualizacion del movimiento de cambio usando el tiempo.
     */
    public IEnumerator RutinaMovimiento(Casilla casilla, float tiempoMovimiento)
    {
        Vector3 destino = new Vector3(casilla.x, casilla.y, 0);
        Vector3 posicionInicial = transform.position;
        bool llegoADestino = false;
        float tiempoTranscurrido = 0f;
        moviendo = true;
        Debug.Log("Entro en RutinaMovimiento");
        while(!llegoADestino)
        {

            if(Vector3.Distance(transform.position, destino)<0.01f)
            {
                llegoADestino = true;
                transform.position = destino;

                //De esta manera la rotacion no se vuelve a inicializar
                    float rotX=this.transform.eulerAngles.x;
                    float rotY = this.transform.eulerAngles.y;
                    float rotZ = this.transform.eulerAngles.z;

                 //   tablero.ColocarFicha(this, casilla);
                    this.transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
                break;
            }
        tiempoTranscurrido += Time.deltaTime;
        float t = Mathf.Clamp(tiempoTranscurrido / 0.5f, 0f,1f);
           // Debug.Log("t: " + t);
        switch (interpolation)
            {
                case InterType.Linear:
                    break;
                case InterType.EaseOut:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case InterType.EaseIn:
                    t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                    break;
                case InterType.SmoothStep:
                    t = t * t * (3 - 2 * t);
                    break;
                case InterType.SmootherStep:
                    t = t * t * t * (t * (t * 6 - 15) + 10);
                    break;
            }

        transform.position = Vector3.Lerp(posicionInicial, destino, t);

        yield return null;
        }
        moviendo = false;
    }
}
