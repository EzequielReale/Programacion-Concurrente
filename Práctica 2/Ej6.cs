fabrica (int N) {

    int llegue = 0, termine = 0, idGanador = -1, maxTareas[N];
    sem mutex = 1, semTermine = 1, barrera[N] = ([N] 0);
    queue tareas = Tareas.tareasDiarias();

    process Empleado [id: 0 to N-1] {
        Tarea tarea;
        int misTareasCompletadas = 0;

        P(mutex);
        llegue++;
        if (llegue == N) for (i: 0 to N-1) V(barrera[i]);
        V(mutex);

        P(barrera[id]);
        
        P(mutex);
        while (!tareas.estaVacia()) {
            tarea = tareas.pop();
            V(mutex);
            realizarTarea();
            misTareasCompletadas++;
            P(mutex);
        }
        V(mutex);

        //Fin de la jornada

        maxTareas[id] = misTareasCompletadas;

        P(semTermine);
        termine++;
        if (termine == N) {
            idGanador = pos(max(maxTareas));
            for (i: 0 to N-1) V(barrera[i]);
        }
        V(semTermine);

        P(barrera[id]);
        
        if (id == idGanador) reclamarPremio();
    }
}