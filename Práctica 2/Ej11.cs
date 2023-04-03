TerminalDeMicros () {
    
    queue colaPasajeros[3];
    sem mutex = 1, mutexAtendidos = 1, mutexCola[3] = [3 1], esperando[3] = [3 0], yaAtendido[3] = [3 0];
    int atendidos = 0;
    
    Pasajero [id: 0 to 149] {
        P(mutex);
        int iColaMenor = min(colaPasajeros[0].length, colaPasajeros[1].length, colaPasajeros[2].length);
        //Otra opción más eficiente sería llevar aparte un arreglo [3] de tamaños de las colas, para consultar
        //por ese y no bloquear el acceso a quien se quiere encolar cada vez que se consulta la cola menor.
        //En ese caso habría que incrementarlo aparte cuando alguien llega y decrementarlo cuando se retira.
        V(mutex);
        
        P(mutexCola[iColaMenor]);
        colaPasajeros[iColaMenor].push(id);
        V(mutexCola[iColaMenor]);
        
        V(esperando[iColaMenor]);
        //lo hisopan
        P(yaAtendido[iColaMenor]);
    }
    
    Enfermera [id: 0 to 2] {
        while (atendidos < 150) {
            P(esperando[id]);
            
            P(mutexCola[id]);
            Pasajero pasajero = colaPasajeros[id].pop();
            V(mutexCola[id]);
            
            Hisopar(pasajero);
            
            V(yaAtendido[id]);

            P(mutexAtendidos);
            atendidos++;
            V(mutexAtendidos);
        }
    }
}