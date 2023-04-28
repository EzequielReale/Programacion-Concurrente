/*
Resolver con semáforos el siguiente problema. Hay N personas que deben
usar UN cajero automático de acuerdo al orden de llegada.
NOTA: sólo se pueden usar los procesos Persona; el uso del cajero por
parte de una persona se simula llamando a la función UsarCajero().
*/

queue fila;
bool libre = true;
sem mutexLibre = 1, mutexFila = 1, espera[N] = ([N] , 0);

process Persona [id: 0 to N-1] {
    P(mutexLibre);
    if (libre) {
        libre = false;
        V(mutexLibre);
    }
    else {
        V(mutexLibre);
        P(mutexFila);
        push(fila, id);
        V(mutexFila);
        P(espera[id]);
    }

    usarCajero();

    P(mutexLibre);
    P(mutexFila);
    if (empty(fila)) {
        libre = true;
        V(mutexLibre);
        V(mutexFila);
    }
    else {
        V(mutexLibre);
        int idNext;
        pop(fila, idNext);
        V(mutexFila)
        V(espera[idNext]);
    }
}
