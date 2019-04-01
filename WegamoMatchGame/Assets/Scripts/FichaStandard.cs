using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FichaStandard : Ficha
{
    // Start is called before the first frame update
    void Start()
    {
        
        //this.valor = (FichaValor)Random.Range(0, System.Enum.GetValues(typeof(FichaValor)).Cast<FichaValor>().Max());
       //  var values = Enum.GetValues(typeof(FichaValor));
       // FichaValor randomValue = (FichaValor)values[Random.Range(0,values.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValor(FichaValor valor){
        this.valor = valor;
    }
}
