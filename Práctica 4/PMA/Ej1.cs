chan cola(int), turno[N](int)

process Persona[id: 1 to N] {
    int nroTurno; 
    while(true)[
        send cola(id); 
        receive turno[id](nroTurno); 
    ]
}

process Empleado[id: 0 to 1] {
    int nroTurno, idP; 
    while(true){
        receive cola(idP); 
        nroTurno = siguiente()
        send turno[idP](nroTurno); 
    }
}