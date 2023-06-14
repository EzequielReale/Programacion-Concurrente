/*
Resolver con ADA el siguiente problema. Existe un sitio de algebra lineal especializado en resolver
multiplicaciones de matrices que atiende pedidos de N clientes, el sitio tiene un Servidor encargado
de resolver los pedidos de los clientes de acuerdo con el orden en que se hacen los mismos. Cada
cliente hace un sólo pedido e imprime el resultado (suponga que existe una función ImprimirMatriz
para eso).
NOTA: todas las tareas deben terminar.
*/


procedure AlgebraLineal is

    type Servidor is
        entry calcularMatriz(m1: in array()(), m2: in array()(), result: out array()());
    end Servidor;

    task type Cliente;

    Clientes: array (0..N-1) of Cliente;

    
    task body Cliente is
        matriz1, matriz2: array(0..X-1)(0..Y-1) of Integer := llenarMatriz();
        res: array(0..X-1)(0..Y-1) of Integer;
    begin
        Servidor.calcularMatriz(matriz1, matriz2, res);
        ImprimirMatriz(res);
    end Cliente;


    task body Servidor is
    begin
        for i := 0..N-1 loop
            accept calcularMatriz(m1: in array()(), m2: in array()(), result: out array()()) do
                result := calcularMultiplicacion(m1, m2);
            end calcularMatriz;
        end loop;
    end Servidor

begin
    null
end AlgebraLineal;
