SistemaOperativo (int N) {

    sem accesoCola = 1, recurso = 5;
    queue colaRecurso;

    process proceso [id: 0 to N-1]
    {
        while (true) {
            P(recurso);
            
            P(accesoCola);
            colaRecurso.pop(); //Desencola el recurso
            V(accesoCola);
            
            delay(N); //Usa el recurso
            
            P(accesoCola);
            colaRecurso.push(/*recurso*/);
            V(accesoCola);
            
            V(recurso);
        }
        
    }
}