Monitor Corralon {

    cond cola, hayCliente, lista, comprobante, terminado;
    int esperando = 0;

    procedure PedirAtencion() {
        esperando++;
        signal(hayCliente);
        wait(cola);
    }

    procedure Atenderse(list: out text, comp: in text) {
        text l = getLista();
        signal(lista);
        list = l;
        wait(comprobante);
        text c = comp;
        signal(terminado);
    }

    procedure Proximo() {
        if (esperando == 0) wait(hayCliente);
        esperando--;
        signal(cola);
    }

    procedure AtenderCliente(list: in text, comp: out text) {
        text c = getComprobante();
        wait(lista);
        text l = list;
        signal(comprobante);
        comp = c;
        wait(terminado);
    }
}


process Cliente [id: 0 to N-1] {
    text list, comp;

    Corralon.PedirAtencion();
    Corralon.Atenderse(list, comp);
}

process Empleado {
    text list, comp;

    while (true) {
        Corralon.Proximo();
        Corralon.AtenderCliente(list, comp);
    }
}