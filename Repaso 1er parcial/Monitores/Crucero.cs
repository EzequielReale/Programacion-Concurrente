/*
Resolver con MONITORES el siguiente problema. En un Crucero por
el Mediterráneo hay 200 personas que deben subir al barco por
medio de 10 lanchas con 20 lugares cada una. Cada persona sube a
la lancha que le corresponde. Cuando en una lancha han subido sus
20 personas, durante 5 minutos navega hasta el barco. Recién cuando
han llegado las 10 lanchas al barco se les permite a las 200
personas subir al barco. 
NOTA: suponga que cada persona llama a la función
int NúmeroDeLancha() que le devuelve un valor entre 0 y 9
indicando la lancha a la que debe subir. Maximizar la concurrencia.
*/

monitor Lancha [id: 0 to 9] {

    cond arribo;
    int llegaron = 0;

    procedure subirYViajar() {
        llegaron++;
        if (llegaron < 20) wait(arribo);
        else {
            delay(5 minutos); //viaje hasta el crucero
            Abordaje.esperarLlegada();
            signal_all(arribo);
        }
    }
}

monitor Abordaje {

    cond llegaronTodas;
    int llegaron = 0;

    procedure esperarLlegada() {
        llegaron++;
        if (llegaron < 10) wait(llegaronTodas);
        else signal_all(llegaronTodas);
    }
}

process Persona [id: 0 to 199] {
    int idLancha = NumeroDeLancha();

    Lancha[idLancha].subirYViajar();
    SubirAlCrucero();
}