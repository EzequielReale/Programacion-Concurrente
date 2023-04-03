//a)

Impresora(int N) {

    process Imprimir(FP *documento);

    process Persona() [i = 0 to N-1] {
        FP documento = fopen ("/home/mis archivos/archivo", "r");
        
        <Imprimir(*documento);>
    }
}

//b)

Impresora(int N) {

    queue *colaImpresora;
    int siguiente = -1;

    process Imprimir(FP *documento);
    process EstaVacia();
    process Push(int id);
    process Pop();

    process Persona() [i = 0 to N-1] {
        FP documento = fopen ("/home/mis archivos/archivo", "r");

        <if (siguiente == -1) siguiente = i;
        else colaImpresora.Push(i);>
        
        <await (siguiente == i);
        Imprimir(documento);>

        <if colaImpresora.estaVacia() siguiente = -1;
        else siguiente = colaImpresora.Pop()>
    }
}

//c)

Impresora(int N) {

    queue *colaImpresora[N];
    int siguiente = -1;

    process Imprimir(FP *documento);
    process EstaVacia();
    process InsertarOrdenado(int id);
    process Pop();

    process Persona() [i = 0 to N-1] {
        FP documento = fopen ("/home/mis archivos/archivo", "r");

        if (siguiente == -1) siguiente = i;
        else colaImpresora.InsertarOrdenado(i);

        <await (siguiente == i);
        Imprimir(documento);>
        
        <if colaImpresora.estaVacia() siguiente = -1;
        else siguiente = colaImpresora.Pop()>
    }
}

//d)

Impresora(int N) {

    int siguiente = 0;

    process Imprimir(FP *documento);

    process Persona() [i = 0 to N-1] {
        FP documento = fopen ("/home/mis archivos/archivo", "r");
        
        <await (siguiente == i);>
        Imprimir(documento);
        siguiente++;
        if (siguiente >= N) siguiente = 0;
    }
}

//e)

Impresora(int N) {

    queue *colaImpresora;
    bool turno[N];

    process Imprimir(FP *documento);
    process EstaVacia();
    process Push(int id);
    process Pop();

    process Coordinador() {
        int idActual;
        
        <await (!colaImpresora.EstaVacia()); idActual = colaImpresora.Pop();>
        turno[idActual] = true;
        
        <await (!turno[idActual]);>
    }
    
    process Persona() [i = 0 to N-1] {
        FP documento = fopen ("/home/mis archivos/archivo", "r");
        
        <colaImpresora.Push(i)>
        
        <await (turno[i]);>
        Imprimir(documento);
        turno[i] = false;
    }
}
