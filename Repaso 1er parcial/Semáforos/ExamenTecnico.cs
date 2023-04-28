/*
Resolver con SEMÁFOROS el siguiente problema. Simular un examen
técnico para concursos no-docentes en la Facultad, en el mismo
participan 100 personas distribuidas en 4 concursos (25 personas
en cada concurso) con un coordinador en cada una de ellos. Cada
persona ya conoce en que concurso participa. El coordinador de
cada concurso espera hasta que lleguen las 25 personas
correspondientes al mismo, les entrega el examen a resolver
(el mismo para todos los de ese concurso), y luego corrige los
examenes de esas 25 personas de acuerdo al orden en que van
entregando. Cada persona al llegar debe esperar a que su
coordinador (el que corresponde a su concurso) le dé el examen,
lo resuelve, lo entrega para que su coordinador lo evalúe y espera
hasta que le deje la nota para luego retirarse.
Nota: maximizar la concurrencia; sólo usar los procesos que representen
a las personas y a los coordinadores; todos los procesos deben terminar.
*/

sem mutex[4] = ([4] , 1), barrera[4] = ([4] = 0), llegaronTodos[4] = ([4] , 0), termine[4] = ([4] , 0), esperaNota[100] = ([100] , 0);
queue entregas[4], examenes[4]; 
int notas[100], cantLlegaron[4] = ([4] , 0);

process Alumno [id: 0 to 99] {
    int idC = getIdConcurso();

    P(mutex[idC]);
    cantLlegaron[idC]++;
    if (cantLlegaron[idC] == 25) V(llegaronTodos[idC]);
    V(mutex[idC]);
    P(barrera[idC]);

    P(mutex[idC]);
    pop(examenes[idC], examen);
    V(mutex[idC]);
    realizarExamen(examen);
    delay(60 minutos);

    P(mutex[idC]);
    push(entregas[idC], examen, id);
    V(mutex[idC]);
    V(termine[idC]);
    
    P(esperaNota[id]);
    int miNota = notas[id];
    //se retira
}

process Coordinador [id: 0 to 3] {
    text examen;
    int idP, nota;
    
    P(llegaronTodos[id]);
    for i = 0 to 24 {
        P(mutex[id])
        push(examenes[id], examen);
        V(mutex[id]);
        V(barrera[id]);
    }

    for i = 0 to 24 {
        P(termine[id]);
        P(mutex[id]);
        pop(entregas[id], examen, idP);
        V(mutex[id]);

        nota = corregirExamen(examen);
        delay(5 minutos);

        notas[idP] = nota;
        V(esperaNota[idP]);
    }
}
