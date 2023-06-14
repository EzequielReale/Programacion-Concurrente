/*
Resolver con PASAJES DE MENSAJES SINCRÓNICOS (PMS) el siguiente problema. Existe un sitio
de algebra lineal especializado en resolver multiplicaciones de matrices que atiende
pedidos de N clientes, el sitio tiene un Servidor encargado de resolver los pedidos de los
clientes de acuerdo con el orden en que se hacen los mismos. Cada cliente hace un solo pedido.
NOTA: todas las tareas deben terminar.
*/


process Cliente[id: 0..N-1] {
    int matriz1[x][y], matriz2[x][y], result[x][y];
    
    matriz1 = llenarMatriz();
    matriz2 = llenarMatriz();
    Coordinador!pedidoMultiplicacion(matriz1, matriz2, id);
    Servidor?enviarResultado(result);
    imprimirMatriz(result);
}


process Coordinador {
    queue pedidos;
    int idC, matriz1[][], matriz2[][], completados = 0;

    do  (completados < N) ; Cliente[*]?pedidoMultiplicacion(matriz1, matriz2, idC) ->
            push(pedidos, matriz1, matriz2, idC);
    □   (!empty(pedidos)) ; Servidor?libre() ->
            Servidor!enviarDatos(pop(pedidos));
            completados++;
    od;
}


process Servidor {
    int idC, matriz1[][], matriz2[][], result[][];

    for i = 0..N-1 {
        Coordinador!libre();
        Coordinador?enviarDatos(matriz1, matriz2, idC);
        result = calcularMultiplicacion(matriz1, matriz2);
        Cliente[idC]!enviarResultado(result);
    }
}
