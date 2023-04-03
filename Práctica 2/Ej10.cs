Vacunatorio () {
    
    queue personas;
    sem mutex = 1, barrera[50] = [50 0], sonCinco = 0; 
    int esperando = 0;
    
    Persona [id: 0 to 49] {
        P(mutex);
        esperando++;
        personas.push(id);
        if (esperando == 5) {
            esperando = 0;
            V(sonCinco);
        }
        V(mutex);
        
        P(barrera[id]);
    }
    
    EmpleadoDeSalud {
        int vacunados = 0;
        while (vacunados < 50) {
            P(sonCinco);
            for (i = 0 to 4) {
                P(mutex);
                Persona persona = personas.pop();
                V(mutex);
                VacunarPersona();
                V(barrera[vacunados]);
                vacunados++;
            }    
        }
    }
}