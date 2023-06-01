/*
Se requiere modelar un puente de un solo sentido, el puente solo soporta el peso de 5
unidades de peso. Cada auto pesa 1 unidad, cada camioneta pesa 2 unidades y cada camión
3 unidades. Suponga que hay una cantidad innumerable de vehículos (A autos, B
camionetas y C camiones).
*/

//A) Realice la solución suponiendo que todos los vehículos tienen la misma prioridad

procedure Ej1A is

    task Puente is
        entry entrarAuto();
        entry entrarCamioneta();
        entry entrarCamion();
        entry salirAuto();
        entry salirCamioneta();
        entry salirCamion();
    end Puente;

    task type Auto;

    task type Camioneta;
    
    task type Camion;

    autos: array(1..A) of Auto;
    camionetas: array(1..B) of Camioneta;
    camiones: array(1..C) of Camion;

    task body Auto is
    begin
        Puente.entrarAuto();
        delay(random(10, 60));
        Puente.salirAuto();
    end Auto;

    task body Camioneta is
    begin
        Puente.entrarCamioneta();
        delay(random(10, 60));
        Puente.salirCamioneta();
    end Camioneta;

    task body Camion is
    begin
        Puente.entrarCamion();
        delay(random(10, 60));
        Puente.salirCamion();
    end Camion;

    task body Puente is
        peso: integer := 0;
    begin
        loop
            select
                when (peso < 5) =>
                accept entrarAuto() is
                    peso += 1;
                end entrarAuto;
            or
                when (peso < 4) =>
                    accept entrarCamioneta() is
                        peso += 2;
                    end entrarCamioneta;
            or
                when (peso < 3) =>
                    accept entrarCamion() is
                        peso += 3;
                    end entrarCamion;
            or
                accept salirAuto() is
                    peso -= 1;
                end salirAuto;
            or 
                accept salirCamioneta() is
                    peso -= 2;
                end salirCamioneta;
            or 
                accept salirCamion() is
                    peso -= 3;
                end salirCamion;
            end select;
        end loop;
    end Puente;

end Ej1A;

//B) Modifique la solución para que tengan mayor prioridad los camiones que el resto de los vehículos

procedure Ej1B is

    task Puente is
        entry entrarAuto();
        entry entrarCamioneta();
        entry entrarCamion();
        entry salirAuto();
        entry salirCamioneta();
        entry salirCamion();
    end Puente;

    task type Auto();
    task type Camioneta();
    task type Camion();

    autos: array(1..A) of Auto;
    camionetas: array(1..B) of Camioneta;
    camiones: array(1..C) of Camion;


    task body Auto is
    begin
        Puente.entrarAuto();
        delay(random(10, 60));
        Puente.salirAuto();
    end Auto;


    task body Camioneta is
    begin
        Puente.entrarCamioneta();
        delay(random(10, 60));
        Puente.salirCamioneta();
    end Camioneta;


    task body Camion is
    begin
        Puente.entrarCamion();
        delay(random(10, 60));
        Puente.salirCamion();
    end Camion;


    task body Puente is
        peso: integer := 0;
    begin
        loop
            select
                when ((peso < 5) and (entrarCamion'count = 0)) =>
                    accept entrarAuto() do
                        peso += 1;
                    end entrarAuto;
                or
                    when ((peso < 4) and (entrarCamion'count = 0)) =>
                        accept entrarCamioneta() do
                            peso += 2;
                        end entrarCamioneta;
                or
                    when (peso < 3) =>
                        accept entrarCamion() do
                            peso += 3;
                        end entrarCamion;
                or
                    accept salirAuto() do
                    peso -= 1;
                    end salirAuto;
                or 
                    accept salirCamioneta() do
                    peso -= 2;
                    end salirCamioneta;
                or 
                    accept salirCamion() do
                    peso -= 3;
                    end salirCamion;
            end select;
        end loop;
    end Puente;

end Ej1B;
