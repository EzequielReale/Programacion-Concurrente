chan cola[5](int), turno[P](int), comprobante[P](text);

process Persona[id: 0 to P-1] {
    int miCaja = min(cola);
    text comp;

    send cola[miCaja](id);
    recieve turno[id](id);
    //Lo atienden
    recieve comprobante[id](comp);
}

process Caja[id: 0 to 4] {
    int idP;
    text comp;

    while (true) {
        recieve cola[id](idP);
        send turno[idP](idP);
        //Atiende
        send comprobante[idP](comp);
    }
}

//Técnicamente podría llamar a otra persona antes de que el cliente
//agarre el comprobante, pero igual como son canales privados funciona