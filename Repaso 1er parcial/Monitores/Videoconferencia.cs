/*
Resolver el siguiente problema utilizando monitores.
Un equipo de videoconferencia puede ser usado por una persona a la vez 
Hay P personas que utilizan este equipo (una unica vez cada uno) para
su trabajo, de acuerdo a su prioridad. La prioridad de cada persona
está dada por un numero entero positivo. Ademas existe un administrador
que cada 3hs incrementa en 1 la prioridad de todas las personas que
están esperando usar el equipo.
NOTA: maximizar la concurrencia.
*/

monitor Videoconferencia {
    
    cond espera[P];
    sortedQueue colaOrdenada; //cola ordenada por prioridad
    bool libre = true;
    
    procedure entrar(id: in int, prioridad: in int) {
        if (libre) libre = false;
        else {
            push(colaOrdenada, id, prioridad);
            wait(espera[id]);
        }
    }
    
    procedure salir() {
        int id, prioridad;
        
        if (empty(colaOrdenada)) libre = true;
        else {
            pop(colaOrdenada, id, prioridad);
            signal(espera[id]);
        }
    }
    
    
    procedure aumentarPrioridad() { //????????????
        int id, prioridad;
        queue colaPrivada;
        
        while (!empty(colaOrdenada)) {
            pop(colaOrdenada, id, prioridad);
            prioridad ++;
            push(colaPrivada, id, prioridad);
        }
        
        while (!empty(colaPrivada)) {
            pop(colaPrivada, id, prioridad);
            push(colaOrdenada, id, prioridad);
        }
    }
}


process Persona [id: 0 to P-1] {
    Videoconferencia.entrar(id, prioridad);
    delay(random()); //usarEquipoDeVideoconferencia();
    Videoconferencia.salir();
}

process Administrador {
    while (true) {
        delay(3 horas);
        VideoConferencia.aumentarPrioridad();
    }
}
