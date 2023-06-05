/*
Resolver este ejercicio con PMS. 
En un banco se tiene un sistema que administra el uso de una sala de reuniones
por parte de N clientes. Los clientes se clasifican en habituales o temporales.
La sala puede ser usada por un unico cliente a la vez
Y cuando esta libre se debe determinar a quien permitirle su uso siempre priorizando a los clientes habituales
Dentro de cada clase de cliente se debe respetar el orden de llegada
Nota: suponga que existe una funcion tipo() que le indica al cliente de que tipo esta
*/


process Cliente[id: 0..N-1] {
    String tipo = Tipo();
    SalaDeReuniones!Encolar(id, tipo);
    SalaDeReuniones!Pedido();
    SalaDeReuniones?Usar();
    delay(random());
    SalaDeReuniones!Liberar();
}

process SalaDeReuniones {
    queue colaHabituales, colaTemporales;
    int idC,
    String tipo;
    bool libre = true;
    
    do  Cliente[*]?Encolar(idC, tipo) ->
            if (tipo == "habitual") push(colaHabituales, idC);
            else push(colaTemporales, idC);
    □   libre && !(empty(colaHabituales)) ; Cliente[*]Pedido() ->
            pop(colaHabituales, idC);
            Cliente[idC]!Usar();
            libre = false;
    □   libre && (!(empty(colaTemporales)) && (empty(colaHabituales))) ; Cliente[*]Pedido() ->
            pop(colaTemporales, idC);
            Cliente[idC]!Usar();
            libre = false;
    □   Cliente[*]Liberar ->
            libre = true;
    od
}
