/*
Resolver con ADA el siguiente problema: simular la venta de entradas a un evento musical
por medio de un portal web. Hay N clientes que intentan comprar una entrada para el evento;
los clientes pueden ser regulares o especiales. Cada cliente especial hace un pedido al
portal y espera hasta ser atendido; cada cliente regular hace un pedido y si no es atendido
antes de los 5 minutos, vuelve a hacer el pedido siguiendo el mismo patron (espera a lo sumo
5 minutos y si no lo vuelve a intentar) hasta ser atendido. Despues de ser atendido, si
consiguio comprar la entrada, debe imprimir el comrpobante de la compra. El portal tiene E
entradas para vender y atienden los pedidos de acuerdo al orden de llegada pero dando
prioridad a los clientes especiales. Cuando atiende un pedido, si aun quedan entradas
disponibles, le vende una al cliente que hizo el pedido y le entreega el comprobante.
Nota: no debe modelarse la parte de la impresion del comprobante, solo llamar a una funcion
Imprimir(comprobante) en el cliente que simulara esa parte; la cantidad E de entradas es mucho
menor que la cantidad de clientes (E << C); todas las tareas deben terminar.
*/
