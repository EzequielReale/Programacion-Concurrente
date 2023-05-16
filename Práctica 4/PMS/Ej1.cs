process Robot[id: 0 to R-1] {
    text web;

    while (true) {
        web = buscarDireccionInfectada();
        Admin!sitioEncontrado(web);
    }
}

process Admin {
    queue buffer;
    text web;

    do Robot[*]?sitioEncontrado(web) -> push(buffer, web);
    □  not empty(buffer); Analizador?disponible() -> Analizador!sitioParaAnalizar(pop(buffer));
    od
}

process Analizador {
    text web;
    bool resultado;

    while (true) {
        Admin!disponible(); 
        Admin?sitioParaAnalizar(web);
        resultado = analizar(web); //Supongamos que después hace algo con esto
    }
}