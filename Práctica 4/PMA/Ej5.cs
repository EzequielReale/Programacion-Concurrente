chan paraImprimir(Document), pedido(), imprimirUsuario(Document), imprimirDirector(Document);

process Impresora [id: 0 to 2] {
    Document doc;
    while (true) {
        receive paraImprimir(doc);
        Imprimir(doc);   
    }
}

process Usuario [id: 0 to N-1] {
    Document doc;
    while (true) {
        send imprimirUsuario(doc);
        send pedido();
    }
}

process Director {
    Document doc;
    while (true) {
        send imprimirDirector(doc);
        send pedido();
    }
}

process Coordinador {
    Document doc;
    while (true) {
        receive pedido();
        if (!empty(imprimirDirector)) {
            receive imprimirDirector(doc);
            send paraImprimir(doc);
        } else if (!empty(imprimirUsuario)) {
            receive imprimirUsuario(doc);
            send paraImprimir(doc);
        }
    }
}