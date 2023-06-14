/*
Resolver con PASAJE DE MENSAJES ASINCRÓNICOS (PMA) el siguiente problema. En un
corralón se pueden pedir presupuestos por mail, para esto tiene 3 empleados para
atender los pedidos de N personas de acuerdo con el orden en que llegan los mismos.
Cada persona envía la lista de materiales a comprar y recibe el presupuesto para
dicha lista. Los empleados atienden los pedidos de a uno a la vez, cuando no hay
pedidos para pendientes ordenen materiales durante 10 minutos.
NOTA: los empleados no deben terminar.
*/


chan pedidoPresupuesto(List, int), pedidoTrabajo(int), entregaPresupuesto[N](double), envioDatos[3](List, int);

process Cliente[id: 0..N-1] {
    List materiales = listarMateriales();
    double presupuesto;

    send pedidoPresupuesto(materiales, id);
    receive entregaPresupuesto[id](presupuesto);
}


process Coordinador {
    List materiales;
    int idC, idE;

    while (true) {
        receive pedidoTrabajo(idE);
        if (!empty(pedidoPresupuesto)) {
            receive pedidoPresupuesto(materiales, idC);
            send envioDatos[idE](materiales, idC);
        }
        else send envioDatos[idE](null, -1);
    }
}


process Empleado[id: 0..2] {
    List materiales;
    int idC;
    double presupuesto;

    while (true) {
        send pedidoTrabajo(id);
        receive envioDatos[id](materiales, idC);
        if (idC != -1) {
            presupuesto = calcularPresupuesto(materiales);
            send entregaPresupuesto[idC](presupuesto);
        }
        else delay(600); //ordena materiales
    }
}
