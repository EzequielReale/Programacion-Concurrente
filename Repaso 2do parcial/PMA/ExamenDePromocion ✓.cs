/*
Resolver con PMA.
N alumnos rinden un examen de promocion, M rinden normal, los alumnos realizan 
el examen (suponga que ya lo tienen al empezar) y luego lo entregan. Hay 3 profesores
que corrigen los examenes priorizando los de promocion, luego le dan la nota al alumno y este se retira.
*/

chan enviarExamenDePromo(Examen, int), enviarExamenNormal(Examen, int), hayExamen(), enviarNota[N+M](int), profesorLibre(int), enviarParaCorregir[3](Examen, int);

process Alumno[id: 0 to N+M-1] {
	Examen examen = resolverExamen();
	int nota;

	if (Tipo() == "promocion") send enviarExamenDePromo(examen, id);
	else send enviarExamenNormal(examen, id);
	send hayExamen();

	receive enviarNota[id](nota);
	if (nota >= 4) print("Disculpame si te rompí el orto, pero así son los parciales champagne");
}

process Profesor[id: 0 to 2] {
	int idA, nota;
	Examen examen;

	while (true) {
		send profesorLibre(id);
		receive enviarParaCorregir[id](examen, idA);
		nota = corregirExamen(examen);
		send enviarNota[idA](nota);
	}
}

process Coordinador {
	Examen examen;
	int idA, idP;

	while (true) {
		receive profesorLibre(idP);
		receive hayExamen();
		if (!empty(enviarExamenDePromo)) receive enviarExamenDePromo(examen, idA);
		else receive enviarExamenNormal(examen, idA);
		send enviarParaCorregir[idP](examen, idA);
	}
}
