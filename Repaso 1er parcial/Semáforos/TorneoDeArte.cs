/*
Resolver con SEMÁFOROS la siguiente situación. En un torneo de
arte hay P Participantes que realizan una pintura cada uno. Al
terminar dejan la pintura para ser evaluada y luego esperan el
resultado, lo imprimen y se retiran. Además existe un Evaluador
que evalúa las pinturas de acuerdo al orden en que las fueron
terminando. El Evaluador toma una pintura, la evalúa y le da
la nota al participante correspondiente.
NOTA: No se puede usar una estructura auxiliar para almacenar
las notas de los participantes. Maximizar la concurrencia.
*/

sem mutex = 1, notaRecibida = 0, espera[N] = ([N] 0);
int nota;
queue cola;

process Participante [id: 0 to N-1] {
    Pintura pintura = RealizarPintura();
    
    P(mutex);
    push(cola, id, pintura);
    V(mutex);
    
    P(espera[id]);
    int miNota = nota;
    V(notaRecibida);
}

process Evaluador {
    Pintura pintura;
    int idP;

    while (true) {
        P(mutex);
        pop(cola, idP, pintura);
        V(mutex);
        
        nota = evaluarPintura();
        V(espera[idP]);
        P(notaRecibida);
    }
}