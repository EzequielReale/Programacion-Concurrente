monitor Equipo [id: 0 to 4] {
    
    cond equipoCompleto;
    int jugadoresEquipo = 0;
    
    procedure esperarEquipo() {
        jugadoresEquipo++;
        if (jugadoresEquipo < 4) wait(equipoCompleto);
        else {
            signal_all(equipoCompleto);
        }
    }
}

monitor Resultado [id: 0 to 4] {
    
    int termine = 0, valorMonedas = 0;
    cond fin;
    
    procedure contarMonedas(subtotal: in int, resultado: out int, ganador: out int) {
        valorMonedas += subtotal;
        
        if (termine < 4) 
            wait(fin);
            resultado = valorMonedas;
        else
            resultado = valorMonedas;
            Maximo.Max(resultado, ganador);
            signal_all(fin);
    }
}

monitor Maximo { //No lo pide el ejercicio, es una complejidad extra que le agregué
    
    int termineNro = 0, puntajeMasAlto = -1;
    cond terminado;
    
    procedure Max(puntaje: in int, ganador: out int) {
        if (puntaje > puntajeMasAlto) puntajeMasAlto = puntaje;
        termineNro++;
        
        if (termineNro < 4) wait(terminado);
        else signal_all(terminado);
        
        ganador = puntajeMasAlto;
    }
}

process Jugador [id: 0 to 19] {
    int miPuntajeEquipo, ganador, idEquipo = getEquipo();
    int monedas[15];
    
    Equipo[idEquipo].EsperarEquipo();
    
    for monedas = 0 to 14 {
        arregloMonedas[monedas] = Moneda();
    }
    
    for i = 0 to 14 {
        valorMonedas += arregloMonedas[i];
        //Super ineficiente usar 2 for cuando se podría usar
        //1 solo, pero el ejercicio dice que 1ro se juntan
        //las monedas y después se cuentan. So...
    }
        
    Resultado[idEquipo].ContarMonedas(valorMonedas, miPuntajeEquipo, ganador);
    
    if (ganador == miPuntajeEquipo) Festejar();
}
