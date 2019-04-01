using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Matcher : MonoBehaviour
{
    List<Casilla> casillas;
    public int size; // Esto sabra el length necesario que debe tener el Match.
    // Start is called before the first frame update
    void Start()
    {
        casillas = new List<Casilla>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AgregarCasilla(Casilla casilla)
    {
        casillas.Add(casilla);
    }

    public virtual bool HasMatch(Casilla casilla)
    {
        bool hasMatch = true;
        return hasMatch;
    }

}
