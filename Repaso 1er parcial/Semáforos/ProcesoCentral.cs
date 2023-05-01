/*
Resolver con semáforos el siguiente problema: Un sistema de software
está compuesto por un proceso CENTRAL y un conjunto de los procesos
periféricos donde cada uno de ellos realiza una determinada operación
especial (cuyo resultado es un valor entero) El proceso CENTRAL debe
esperar a que todos los procesos periféricos se hayan iniciado para
poder comenzar. Una vez que el proceso central comenzó a trabajar,
cada vez ue necesita realizar alguna de las 10 operaciones especiales
avisa al correspondiente periférico para que realice el trabajo y
espera a que le devuelva el resultado.
NOTA: Suponga que existe una función int TrabajoProcesoCentral() que
simula el trabajo del proceso CENTRAL y devuelve un valor entero entre
1 y 10, que indica cual de las 10 operaciones debe realizar en ese momento.
*/

sem mutex = 1, esperaCentral = 0, requiereOperacion[10] = ([10] 0);
int iniciados = 0, resultados[10];


process Central {
    int operacion, resultado;

    P(esperaCentral);

    while (true) {
        operacion = TrabajoProcesoCentral();
        V(requiereOperacion[operacion]);
        P(esperaCentral);
        resultado = resultados[operacion];
    }
}

process Periferico [id: 0 to 9] {
    P(mutex);
    iniciados++;
    if (iniciados == 10) V(esperaCentral);
    V(mutex);

    P(requiereOperacion[id]);
    resultados[id] = operacionEspecial();
    V(esperaCentral);
}