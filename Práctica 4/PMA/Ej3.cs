chan pedidos(int, text), disponibleParaComandas(int), siguiente[3](int, text), pedidoCocina(int, text), entregaComida[C](Comida);

process Cliente[id: 0 to C-1] {
    text miPedido = elegirDelMenu();
    Comida comida;

    send pedidos(id, miPedido);
    receive entregaComida(comida);
}

process Coordinador {
    int idC, idV;
    text pedidoC;

    while (true) {
        receive disponibleParaComandas(idV);
        if (!empty(pedidos)) receive pedidos(idC, pedidoC);
        else {
            pedidoC = "";
            idC = -1;
        }
        send siguiente[idV](idC, pedidoC);
    }
}

process Vendedor[id: 0 to 2]{
    int idC;
    text pedidoC;

    while (true) {
        send disponibleParaComandas(id);
        receive pedidos(idC, pedidoC);

        if (idc == -1) {
            //repone las bebidas
            delay(random() % 3 minutos);
        }
        else send pedidoCocina(idC, pedidoC);
    }
}

process Cocinero[id: 0 to 1] {
    int idC;
    text pedidoC;
    Comida comida;

    while (true) {
        receive pedidoCocina(idC, pedidoC);
        comida = cocinar(pedidoC);
        send entregaComida[idC](comida);
    }
}