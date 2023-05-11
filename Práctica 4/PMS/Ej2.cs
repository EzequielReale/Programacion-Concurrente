process Empleado1 {
    Muestra muestraDeADN;
    
    while (true) {
        muestraDeADN = prepararMuestra();
        Coordinador!muestra(muestraDeADN);
    }
}

process Empleado2 {
    Muestra muestraDeADN;
    Set setDeAnalisis;
    Resultado res;

    while (true) {
        Coordinador!pedido();
        Coordinador?muestra(muestraDeADN);
        setDeAnalisis = prepararSetDeAnalisis(muestraDeADN);
        Empleado3!set(setDeAnalisis);
        Empleado3?resultado(res);
        }
}

process Empleado3 {
    Set setDeAnalisis;
    Resultado res;

    while (true) {
        Empleado2?set(setDeAnalisis);
        res = analizar(setDeAnalisis);
        Empleado2!resultado(res);
    }
}

process Coordinador {
    queue bufferMuestras;
    Muestra muestraDeADN;

    do Empleado1?muestra(muestraDeADN) -> push(bufferMuestras, muestraDeADN);
    □  not empty(bufferMuestras); Empleado2?pedido() -> Empleado2!muestra(pop(bufferMuestras));
}