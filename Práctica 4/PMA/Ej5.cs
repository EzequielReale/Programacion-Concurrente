chan paraImprimir(Document), pedido(), pedidoImpresora(), imprimirUsuario(Document), imprimirDirector(Document);

process Impresora [id: 0 to 2] {
    Document doc;
    while (true) {
        send pedidoImpresora();
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
        receive pedidoImpresora();
        
        if (!empty(imprimirDirector)) {
            receive imprimirDirector(doc);
        } else if (!empty(imprimirUsuario)) {
            receive imprimirUsuario(doc);
        }
        
        send paraImprimir(doc);
    }
}