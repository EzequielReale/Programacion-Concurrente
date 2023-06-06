/*
Resolver con PMS el siguiente problema: Se debe administrar el acceso para usar en
determinado Servidor donde no se permite más de 10 usuarios trabajando al mismo tiempo,
por cuestiones de rendimiento. Existen N usuarios que solicitan acceder al Servidor,
esperan hasta que se les de acceso para trabajar en él y luego salen del mismo.
Nota: suponga que existe una función TrabajarEnServidor() que llaman los usuarios para
representar que están trabajando en el Servidor
*/

process Persona[id: 0..N-1] {
    Servidor!pedirAcceso(id);
    Servidor?recibirAcceso();
    TrabajarEnServidor();
    Servidor!liberar();
}

process Servidor {
    queue espera;
    int idP, cantUsando = 0;

    do  Persona[*]?pedirAcceso(idP) ->
            if (cantUsando < 10) {
                Persona[idP]!recibirAcceso();
                cantUsando++;
            }
            else push(espera, idP);
    □   Persona[*]?liberar() ->
            if  !(empty(espera)) Persona[pop(espera)]!recibirAcceso();
            else cantUsando--;
}
