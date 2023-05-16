//a)
process Alumno [id: 0 to N-1] {
    Examen examen = resolver();
    int nota;
    
    Coordinador!entregarExamen(examen, id);
    Profesor?esperarNota(nota);
}

process Profesor {
    int idA, nota;
    Examen examen;
    
    for i = 0 to N-1 {
        Coordinador!pedido();
        Coordinador?recibirExamen(examen, idA);
        nota = corregir(examen);
        Alumno[idA]!esperarNota(nota);
    }
}

process Coordinador {
    int idA;
    Examen examen;
    queue entragas;
    
    do Alumno[*]?entregarExamen(examen, idA) -> push(entregas, examen, idA);
    □  !empty(entregas); Profesor?pedido() -> Profesor!recibirExamen(pop(entregas));
}


//b)
process Alumno [id: 0 to N-1] {
    Examen examen = resolver();
    int nota;
    
    Coordinador!entregarExamen(examen, id);
    Profesor?esperarNota(nota);
}

process Profesor [id: 0 to P-1] {
    int idA, nota;
    Examen examen;
    
    for i = 0 to N-1 {
        Coordinador!pedido(id);
        Coordinador?recibirExamen(examen, idA);
        nota = corregir(examen);
        Alumno[idA]!esperarNota(nota);
    }
}

process Coordinador {
    int idA, idP;
    Examen examen;
    queue entragas;
    
    do Alumno[*]?entregarExamen(examen, idA) -> push(entregas, examen, idA);
    □  !empty(entregas); Profesor?pedido(idP) -> Profesor[idP]!recibirExamen(pop(entregas));
}


//c)
process Alumno [id: 0 to N-1] {
    Examen examen = resolver();
    int nota;
    
    Coordinador!llegue();
    Coordinador?empezar();
    
    Coordinador!entregarExamen(examen, id);
    Profesor?esperarNota(nota);
}

process Profesor [id: 0 to P-1] {
    int idA, nota;
    Examen examen;
    
    for i = 0 to N-1 {
        Coordinador!pedido(id);
        Coordinador?recibirExamen(examen, idA);
        nota = corregir(examen);
        Alumno[idA]!esperarNota(nota);
    }
}

process Coordinador {
    int idA, idP, llegaron = 0;
    Examen examen;
    queue entragas;
    
    do (llegaron < (N)); Alumno[*]?llegue() -> llegaron++;
    
    for i = 0 to N-1 Alumno[*]!empezar();
    
    do Alumno[*]?entregarExamen(examen, idA) -> push(entregas, examen, idA);
    □  !empty(entregas); Profesor?pedido(idP) -> Profesor[idP]!recibirExamen(pop(entregas));
}