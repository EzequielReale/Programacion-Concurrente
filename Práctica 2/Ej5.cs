escuela () {

    int eligieronTema = 0, puntajeTarea[10];
    sem mutex = 1, termine = 0, comenzar[50] = ([50] 0), esperarCorreccion[10] = ([10] 0), ;
    queue entregas;

    process Profesor {
        int actual, entregaron = 0, puntaje = 10, tareasEntregadas[10] = ([10] 0)
        
        while (entregaron < 50) {
            P(termine);
            P(mutex);
            actual = entregas.pop();
            V(mutex);

            entregaron++;
            tareasEntregadas[actual]++;
            if (tareasEntregadas[actual] == 5) {
                puntajeTarea[actual] = puntaje;
                puntaje--;
                for (i:0 to 4) V(esperarCorrecion[actual]);
            }
        }
    }

    process alumno [id: 0 to 49] {
        int miTema = elegir();
        P(mutex);
        eligieronTema++;
        if (eligieronTema == 50) for (i:0 to 49) V(comenzar[i]);
        V(mutex);

        P(comenzar[i]);
        realizarTarea();

        P(mutex);
        entregas.push(miTema);
        V(mutex);

        V(termine);
        P(esperarCorreccion[miTema]);
    }
}