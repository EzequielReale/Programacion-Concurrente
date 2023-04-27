/*
Resolver con SEMÁFOROS el siguiente problema. Simular un examen escrito que deben
rendir 60 alumnos repartidos en 3 aulas (20 alumnos en cada aula) con un docente
en cada una de ellas. Cada alumno ya tiene asignado el aula en la que debe rendir.
El docente de cada aula espera hasta que sus 20 alumnos hayan llegado para darles
el enunciado del examen (el mismo para todos los alumnos), y luego les corrige el
examen de acuerdo al orden en que van entregando. Cada alumno cuando llega debe 
esperar a que su docente (el correspondiente a su aula) le dé el enunciado del
examen, lo resuelve, lo entrega para que el mismo docente lo corrija y le deje
la nota. Cuando el alumno ya tiene su nota se retira. Nota: maximizar la
concurrencia; sólo usar los procesos que representes a los alumnos y a los docentes;
todos los procesos deben terminar.
*/

sem mutex[3] = ([3] , 1), llegaronTodos[3] = ([3] , 0), iniciar[3] = ([3] , 0), termine[3] = ([3] , 0), esperarNota[60] = ([60] , 0);
int llegaron[3] = ([3] , 0), notas[60];
queue entregas[3];

process Alumno [id: 0 to 59] {
    int aula = asignarAula();
    text examen;

    P(mutex[aula]);
    llegaron[aula]++;
    if (llegaron[aula] == 20) V(llegaronTodos[aula]);
    V(mutex[aula]);

    P(iniciar[aula]);
    examen = recibirExamen();
    examen = realizarExamen();

    P(mutex[aula]);
    push(entregas[aula], id, examen)
    V(mutex[aula]);
    V(termine[aula]);
    P(esperarNota[id]);
    int miNota = notas[id];
}

process Profesor [id: 0 to 2] {
    text examen;
    int alumno, nota;

    P(llegaronTodos[id]);
    for i = 0 to 19 {
        V(iniciar)[aula];
        entregarExamen(examen);
    } 

    for i = 0 to 19 {
        P(termine[id]);
        
        P(mutex[id]);
        pop(entregas[id], alumno, examen);
        V(mutex[id]);
        nota = corregirExamen(examen);
        notas[alumno] = nota;
        V(esperarNota[alumno]);
    }
}
