/*
Se tienen N procesos que comparten el uso de una CPU.
Un proceso, cuando necesita utilizar la CPU, le pide el
uso, le proporciona el trabajo que va a ejecutar y el
tiempo que consume ejecutar el trabajo. Luego, el proceso
se queda dormido hasta que la CPU haya finalizado de
ejecutar su trabajo, momento en que es despertado y
prosigue su ejecución normal. La CPU funciona de la
siguiente manera. Cada vez que el proceso i (i: I..N)
pide CPU esta lo pone en el lugar iesimo de una lista de
procesos esperando para ejcutar. La CPU recorre
circulamente la lista de procesos esperando a ser
ejecutados (del 1 al N) , si el proceso esta listo lo
saca de la lista y lo ejecuta durante un lapso de 10 mls
(o un lapso menor, si el tiempo de ejecución del proceso
es menor a 10 mls). Una vez que ejecutó un proceso, si
todavía le queda al proceso tiempo de ejecución lo vuelve
a poner en su lugar en la lista y pasa al siguiente: Si el
proceso terminó de ejecutar su trabajo, la CPU lo despierta
para que continúe su ejecución y continúa con el siguiente
proceso de la lista. Cuando la lista esté vacía la CPU no
debe hacer nada, simplemente debe esperar a que algún
proceso ingrese a la lista de procesos en espera de
ejecución para empezar a trabajar. Tenga en cuenta que
existe la función delay(x) que retarda un proceso durante
× mls. La ejecución de un proceso pude ser modelizada
utilizando esta función. Modelice utilizando MONITORES
*/

monitor Scheduling {

    cond hayProceso, espera[N], mlsRecibido;
    queue lista;
    int newMls;

    procedure pedirCPU(id: in int, mls: in-out int) {
        push(lista, id, mls);
        signal(hayProceso);
        wait(espera[id]);
            
        mls = newMls;
        signal(mlsRecibido);
    }

    procedure ejecutarProcesos() {
        int idP, mlsP;
        
        if (empty(lista)) wait(hayProceso);

        pop(lista, idP, mlsP);
        if (mlsP >= 10) delay(10 mls);
        else delay(mlsP mls);
        newMls = mlsP - 10;

        signal(espera[idP]);
        wait(mlsRecibido);
    }
}


process CPU {
    while (true) Scheduling.ejecutarProcesos();
}

process proceso [id: 0 to N-1] {
    int mls = random();
    while (mls > 0) Scheduling.pedirCPU(id, mls);
}