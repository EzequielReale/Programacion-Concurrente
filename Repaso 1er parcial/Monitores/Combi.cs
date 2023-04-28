/*
Resolver con monitores el siguiente problema. Una empresa tiene 40
empleados que forman 4 equipos de trabajo (cada empleado conoce a
priori a que equipo pertenece). La empresa tiene 4 combis para
trasladar cada equipo a la planta de trabajo que le corresponda.
Cada empleado al llegar sube a la combi que le corresponde a su
equipo, cuando las 4 combis están 0completas cada una de ellas
parte a la planta correspondiente. El traslado dura un tiempo
aleatorio entre 5 y 20 minutos. Al llegar a su sector cada empleado
realiza su trabajo y luego se retira de la planta (cada empleado
se retira independientemente del resto).
Nota: por simplicidad los empleados no se deben esperar para retirarse.
*/


monitor Combi [id: 0 to 3] {

    int aBordo = 0, listos = 0;
    cond todasCompletas, llegamos;

    procedure subirYViajar() {
        aBordo++;
        if (aBordo < 10) wait(todasCompletas);
        else {
            Salida.esperarInicio();
            delay(random(5, 20) minutos); //viaje de la combi
            signal_all(todasCompletas);
        }
    }
}

monitor Salida {

    int combisCompletas = 0;
    cond todasCompletas;

    procedure esperarInicio() {
        combisCompletas++;
        if (combisCompletas < 4) wait(todasCompletas);
        else signal_all(todasCompletas);
    }
}

process Empleado [id: 0 to 39] {
    int equipo = getEquipo();
    Combi[equipo].subirYViajar();
    delay(8 horas) //trabaja
    //se retira
}