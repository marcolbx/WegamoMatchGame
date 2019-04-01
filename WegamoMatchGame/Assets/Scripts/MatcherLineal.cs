using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatcherLineal : Matcher
{
    private List<List<Casilla>> matches;
    private int numDirecciones;
    public int MIN_MATCH = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MatcherLineal(int direcciones)
    {
        this.numDirecciones = direcciones;
        this.matches = new List<List<Casilla>>(numDirecciones);
    }

    public override bool HasMatch(Casilla casilla)
    {
        bool hasMatch = false;

        //TODO Mejorar el manejo de las direcciones y sentidos. Generalizar
        for (int direccion = 1; direccion <= numDirecciones; direccion++)
        {
            List<Casilla> match = new List<Casilla>();
            match.Add(casilla);
            Visit(casilla, direccion, match);
            Visit(casilla, direccion + numDirecciones, match);
            matches.Add(match);
            hasMatch = hasMatch || (match.Count >= MIN_MATCH);
        }

        return hasMatch;
    }

    private void Visit(Casilla casilla, int direccion, List<Casilla> match)
    {
        Casilla next = casilla.GetCasilla(direccion);
        if (next != null)
        {
            if (casilla.SameFicha(next))
            {
                match.Add(next);
                Visit(next, direccion, match);
            }
        }
    }
}
