chan pedido(String), pedirCaja(int), nroCaja[P](int), cola[5](int), turno[P](), comprobante[P](text), termine(int);

process Persona[id: 0 to P-1] {
    int miCaja 
    text comp;
    
    send pedirCaja(id);
    send pedido("llego");
    receive nroCaja[id](miCaja);

    send cola[miCaja](id);
    recieve turno[id]();
    //Lo atienden
    recieve comprobante[id](comp);
    
    send termine(miCaja);
    send pedido("me voy");
}

process Caja[id: 0 to 4] {
    int idP;
    text comp;

    while (true) {
        recieve cola[id](idP);
        send turno[idP]();
        //Atiende
        send comprobante[idP](comp);
    }
}

process Coordinador {
    int idP, idC, personasEsperando[5] = ([5] 0);
    String queHacer;
    
    while (true) {
        receive pedido(QueHacer);
        
        if (queHacer == "llego") {
            receive pedirCaja(idP);
            idC = min(personasEsperando);
            send nroCaja[idP](idC);
            personasEsperando[idC]++;
        } else if (queHacer == "me voy") {
            receive termine(idC);
            personasEsperando[idC]--;
        }
    }
}
