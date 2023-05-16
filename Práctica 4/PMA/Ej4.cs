chan pedido(), pedirCabina(int), recibirCabina[N](int), termine(int, int), recibirTicket[P](Ticket);

process Cliente[id: 0 to N-1] {
    int idC;
    Ticket factura;

    send pedirCabina(id);
    send pedido();
    receive recibirCabina[id](idC);

    delay(random() minutos); //Usa la cabina

    send termine(id, idC);
    send pedido();
    receive recibirTicket[id](factura);
}

process Empleado {
    int idP, idC;
    bool cabinas[5] = ([5] true);
    Ticket factura;

    while (true) {
        receive pedido();

        if (!empty(termine)) {
            receive termine(idP, idC);
            cabinas[idC] = true;
            factura = cobrar(idP);
            send recibirTicket[idP](factura);
        }
        else if (!empty(pedirCabina)) {
            receive pedirCabina(idP);  
            if (!tengoCabinasLibres(cabinas)) {
                receive termine(idP, idC);
                receive pedido(); //Para que no quede el pedido de arriba sin resolver
                cabinas[idC] = true;
                factura = cobrar(idP);
                send recibirTicket[idP](factura);
            }
            idC = getCabinaLibre();
            cabinas[idC] = false;
            send recibirCabina[idP](idC);
        }
    }
}
