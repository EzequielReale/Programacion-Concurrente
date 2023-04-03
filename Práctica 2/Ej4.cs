//a
Impresora(int N) {

    sem mutex = 1;

    process Persona [i=0 to N-1] {        
        P(mutex);
        Imprimir("documento.docx");
        V(mutex);
    }
}


//b
Impresora(int N) {

    sem mutex = 1, espera[N] = ([N] 0);
    int siguiente = -1;
    queue cola;

    process Persona [i=0 to N-1] {
        P(mutex);
        if (siguiente == -1) {
            siguiente = i;
            V(mutex);
        } 
        else {
            cola.push(i);
            V(mutex);     
            P(espera[i]);
        }
        
        Imprimir("documento.docx");

        P(mutex);
        if (cola.estaVacia()) siguiente = -1;
        else {
            siguiente = cola.pop();        
            V(espera[siguiente]);
        }
        V(mutex);
    }
}


//c
Impresora(int N) {

    sem espera[N] = ([N] 0);
    int siguiente = 0;

    process Persona [i=0 to N-1] {
        if (siguiente != i) P(espera[i]);

        Imprimir("documento.docx");
        siguiente++;
        V(espera[siguiente]);
    }
}


//d
Impresora(int N) {

    sem mutex = 1, usando = 0, llega = 0, espera[N] = ([N] 0);
    queue cola;

    process Coordinador {
        int siguiente;

        P(llega);
        P(mutex);
        siguiente = cola.pop();
        V(mutex);
        
        V(espera[siguiente]);
        P(usando);
    }

    process Persona [i=0 to N-1] {      
        P(mutex);
        cola.push(i);
        V(mutex);
        V(llega);

        P(espera[i]);
        Imprimir("documento.docx");
        V(usando);
    }
}


//e
Impresora (int N) {
    queue cola, impresoras[5];
    Impresora asignadas[N];
    int libre = 0, ocupadas = 0;
    sem mutex = 1, mutexImp = 1, espera[N] = ([N] 0), impLibre = 5;

    process Coordinador {
        int siguiente;
        P(impLibre);
        P(mutexImnp);
        impresora = impresoras[ocupadas];
        ocupadas = (ocupadas + 1) mod 5;
        v(mutexImp);

        p(hayCliente);
        p(mutex);
        siguiente = cola.pop();
        v(mutex);

        asignadas[siguiente] = impresora;
        v(espera[siguiente]);
    }

    process Persona [id: 1 to N] {
        P(mutex);
        cola.push(id);
        v(mutex);

        v(hayCliente);
        P(espera[id]);
        impresora = asignadas[id];
        Imprimir("documento.docx");

        P(mutexImp);
        impresoras[libre] = impresora;
        libre = (libre + 1) mod 5;
        v(mutexImp);
        v(impresoraLibre);
    }
}

//Medio rancio, pero podría incrementarse el semáforo "usando" del punto d y funcionaría
//Pero no cumpliría la función de elegir CÚAL impresora específicamente usar
Impresora(int N) {

    sem mutex = 1, usando = 4, llega = 0, espera[N] = ([N] 0); //Cambio el valor de usando
    queue cola;

    process Coordinador {
        int siguiente;

        P(llega);
        P(mutex);
        siguiente = cola.pop();
        V(mutex);
        
        V(espera[siguiente]);
        P(usando);
    }

    process Persona [i=0 to N-1] {      
        P(mutex);
        cola.push(i);
        V(mutex);
        V(llega);

        P(espera[i]);
        Imprimir("documento.docx");
        V(usando);
    }
}