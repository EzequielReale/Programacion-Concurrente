process Espectador [id: 0 to E-1] {
    Coordinador!encolar(id);
    Coordinador!pedido();
    Coordinador?usar();
    delay(1);
    Coordinador!liberar();
}

process Coordinador {
    int idP;
    bool libre = true;
    queue cola;
    
    do Espectador[*]?encolar(idP) -> push(cola, idP);
    □  libre && !empty(cola) ; Espectador[*]?pedido() ->
            pop(cola, idP);
            Espectador[idP]!usar();
            libre = false;
    □  Espectador[*]?liberar() -> libre = true;
}