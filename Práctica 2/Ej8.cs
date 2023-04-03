Cerealera (int T, int M) {

    sem maxTotal = 7, maxTrigo = 5, maxMaiz = 5;

    proces CamionTrigo [id: 0 to T] {
        P(maxTrigo);
        P(maxTotal);
        descargarTrigo();
        V(maxTotal);
        V(maxTrigo);
    }

    process CamionMaiz [id: 0 to M] {
        P(maxMaiz);
        P(maxTotal);
        descargarMaiz();
        V(maxTotal);
        V(maxMaiz);
    }
}