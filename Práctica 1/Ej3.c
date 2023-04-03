/*
a) El problema que puede ocurrir es que el productor aumenta la cantidad y después produce, por lo que
el consumidor puede leer que cant es > 0 e intentar consumir algo que aún no se produjo.

Así mismo, el productor puede producir un elemento sobre una casilla que aún estaba ocupada, es decir,
producir de más para nada

La solución sería aumentar/disminuir la cantidad una vez terminada la producción/consumición
*/

ProductorConsumidor()
{
    int cant = pri_ocupada = pri_vacia = 0;
    int buffer[N];
    
    Process Productor() {
        while (true) {
            //produce elemento
            <await (cant < N);>
            buffer[pri_vacia] = elemento;
            pri_vacia = (pri_vacia + 1) mod N;
            <cant++>
        }
    }
    
    Process Consumidor() {
        while (true) {
            <await (cant > 0);>
            elemento = buffer[pri_ocupada];
            pri_ocupada = (pri_ocupada + 1) mod N;
            <cant-->
            //consume elemento
        }
    }
}

/*
b) Para N productores/consumidores habría que incluir casi todo el código (el que contenga no solo cant sino
también pri_vacia/pri_ocupada) porque de otro modo varios procesos podrían intentar acceder a la misma
posición del arreglo al mismo tiempo, produciendo/consumiendo cosas de más o que ya no existen.
*/

ProductorConsumidor(int N)
{
    int cant = pri_ocupada = pri_vacia = 0;
    int buffer[N];
    
    Process Productor() [i = 0 to N-1] {
        while (true) {
            //produce elemento
            <await (cant < N); cant++; //Ahora pongo el cant++ acá porque total está todo bajo atomicidad
            buffer[pri_vacia] = elemento;
            pri_vacia = (pri_vacia + 1) mod N;>
        }
    }
    
    Process Consumidor() [i = 0 to N-1] {
        while (true) {
            <await (cant > 0); cant--; //Misma razón
            elemento = buffer[pri_ocupada];
            pri_ocupada = (pri_ocupada + 1) mod N;>
            //consume elemento
        }
    }
}