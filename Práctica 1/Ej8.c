Ej8(int n) {
    
    bool acceso[n], espera[n];
    
    
     process Cordinador {
         while (true) {
             for (i=0 to n-1) {
                 if (espera[i]) {
                     espera[i] = false;
                     acceso[i] = true;
                     while (acceso[actual]) skip;
                 }
             }
         }
     }
     
     process SeccionCritica [i=0 to n-1] {
         while (true) {
             espera[i] = true;
             while (!acceso[i]) skip;
             //Sección crítica
             acceso[i] = false;
             //Sección no crítica
         }
     }
}