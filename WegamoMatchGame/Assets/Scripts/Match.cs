using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Match : MonoBehaviour
{
    List<Casilla> casillas = new List<Casilla>();
    // Start is called before the first frame update
    void Start()
    {

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

    public int Size()
    {
        return this.casillas.Count;
    }

    public List<Casilla> getCasillas()
    {
        return this.casillas;
    }

    public void setCasillas(List<Casilla> casillas)
    {
        this.casillas = casillas;
    }

    public Match UnirMatches(Match match, Match match2)
    {
        List<Casilla> casillasUnidas = new List<Casilla>();
        casillasUnidas = match.getCasillas().Union(match2.getCasillas()).ToList();

        Match matchResultado = new Match();
        matchResultado.setCasillas(casillasUnidas);
        return matchResultado;
    }

}
