process Persona[id: 0 to P-1] {
    Coordinador!encolar(id);
    Empleado?usarSimulador();
    delay(random() minutos); //Usa el simulador
    Empleado!liberar();
}

process Empleado {
    int idP;

    while (true) {
        Coordinador!pedido();
        Coordinador?enviar(idP);
        Persona[idP]!usarSimulador();
        Persona[idP]?liberar();
    }
}

process Coordinador {
    queue cola;
    int idP;

    do Persona[*]?encolar(idP) -> push(cola, idP);
    □  !empty(cola); Empleado?pedido() -> Empleado!enviar(pop(cola));
}