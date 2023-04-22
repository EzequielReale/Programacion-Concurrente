monitor Comision {

    cond fila, todosLlegaron, numeroGuardado, terminado, esperaGrupo[25];
    queue entregados;
    int llegaron = 0, grupo, entregaron[25], notaTarea[25];

    procedure presentarse(num: out int) {
        llegaron++;
        if (llegaron == 50) signal(todosLlegaron);
        wait(fila);
        num = grupo;
        signal(numeroGuardado);
    }

    procedure OrganizarGrupos() {
        if (llegaron < 50) wait(todosLlegaron);

        for i = 0 to 49 {
            grupo = DarNumero();
            signal(fila);
            wait(numeroGuardado);
        }
    }

    procedure EntregarTarea(num: in int, nota: in-out int, tarea: in text) {
        push(entregados, tarea, num);
        entregaron[num]++;
        signal(terminado);
        wait(esperaGrupo[num]);
        int nota = notaTarea[num];
    }

    procedure CorregirTarea(nota: in-out int) {
        text tarea;
        int num;

        if (empty(entregados)) wait(terminado);

        pop(tarea, num);
        if (entregaron[num] == 2) {
            notaTarea[num] = nota;
            nota--;
            signal_all(esperaGrupo[num]);
        }
    }
}


process Alumno [id: 0 to 49] {
    text tarea;
    int num, correccion;

    Comision.Presentarse(num);
    //RealizarTarea(tarea);
    delay(1 hora);
    Comision.EntregarTarea(num, correccion, tarea);
}

process JTP {
    int nota = 25;

    Comision.OrganizarGrupos();
    for i = 0 to 49 Comision.CorregirTarea(nota);
}
