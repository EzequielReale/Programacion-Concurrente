SistemaOperativo(int N) {

    int availableResources = 5;
    void *resources[5];

    process Push (**resources);
    process Pop (**resources);

    process processes() [i = 0 to N-1] {
        while (true) {
            <await (availableResources > 0);
            Pop(**resources);
            availableResources--;>
            //Usa el recurso
            <Push(**resources);
            availableResources++;>
            //Libera el recurso
        }
    }
}