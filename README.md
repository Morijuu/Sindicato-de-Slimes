# Sindicato de Slimes 

### Que es nuestro proyecto?

Sindicato de Slimes es un videojuego 2D en desarrollo el cual consta en atrapar Slimes con distintos atributos para vencer a una serie de Jefes. 
Pertenece al género "Roguelike", donde el jugador deberá jugar cada vez la misma partida pero con distintas variaciones cada vez.
Entre sus mecánicas se encuentra: 
  - Atrapar Slimes.
  - Usar los Slimes para atacar, cada uno es distinto.
  - Minijuegos para conseguir a tus Slimes.
  - Sistema de aleatorización de estadísticas para que cada partida sea única.
  - 3 Bosses separados en tres pisos distintos, cada uno diferente.

### Actualizaciones

###### Primera actualización

Lo primero que se implementó al proyecto fueron el sistema de movimiento del personaje principal y la cámara que sigue al jugador. 
 [Gif del movimiento]
 [Codigo del movimiento]

 En segundo lugar hicimos bocetos de fases primerizas de los slimes e implmentamos uno al proyecto. Junto con un codigo que permitia al jugador interactuar cerca de un Slime para que este lo siguiera. 

 ![Imagen del Concept art de los slimes](Media/Imagenes/ConceptArtSlimes1.png)
 [Gif del personaje haciendo que le sigan]
 [Codigo del seguimiento] 

 ###### Segunda actualización

-Movimiento del jugador

 El jugador se controla mediante un script de movimiento que utiliza un Rigidbody2D. A partir de la lectura de los ejes horizontal y vertical, se calcula un vector de movimiento normalizado que se aplica directamente a la velocidad del rigidbody. Al tratarse de un juego con vista cenital, la gravedad está desactivada y el movimiento se realiza únicamente en los ejes X e Y. Este enfoque permite un desplazamiento fluido y una correcta interacción con colisiones y triggers.


-Sistema de cámara y habitaciones

 La cámara sigue al jugador de forma suave mediante interpolación hacia una posición objetivo. Cuando el jugador cambia de habitación, un sistema de transición se encarga de actualizar la posición de la cámara para que encaje con la nueva sala. Este mismo sistema gestiona el reposicionamiento del jugador y de los slimes seguidores, asegurando que todos aparecen correctamente en la nueva habitación y evitando comportamientos erráticos durante el cambio.


-Sistema de slimes seguidores

 Los slimes utilizan un sistema de seguimiento basado en una lista estática de seguidores. El primer elemento de la lista es siempre el jugador, y cada slime sigue al elemento anterior, formando una cadena. Cada slime decide su movimiento en función de la distancia a su objetivo, manteniendo una distancia mínima para evitar solapamientos. Los slimes pueden ser reclutados por el jugador mediante una interacción, momento en el que se añaden a la lista de seguidores. Existe además un límite máximo de slimes que pueden seguir al jugador simultáneamente.


-Tipos de slime y estadísticas

 El juego define distintos tipos de slime (normal, rápido, pesado y tanque) mediante un enumerado. Estos tipos están asociados a una librería de datos que permite diferenciar su comportamiento o características. Cada slime cuenta con un componente encargado de identificar su tipo, lo que facilita extender el sistema en el futuro para aplicar estadísticas o comportamientos específicos según el tipo.


--Sistema de spawn de slimes

 Hemos implementado un sistema de spawn automático mediante un script independiente que se ejecuta al iniciar la escena. Este sistema genera cinco slimes de forma automática sin necesidad de un GameManager global. El área de aparición está definida visualmente por un objeto con SpriteRenderer, que actúa como zona de spawn. A partir de los límites reales de ese sprite, se calcula una posición aleatoria dentro del rectángulo para cada slime. El tipo de slime que aparece se decide mediante un sistema de probabilidad simple, utilizando valores configurables desde el Inspector. En función del resultado, se selecciona uno de los cuatro prefabs correspondientes a cada tipo y se instancia en la escena en la posición calculada.


-Diseño y modularidad del sistema

 La arquitectura del proyecto está basada en scripts con responsabilidades claras. El movimiento, la cámara, el sistema de seguidores y el sistema de spawn están desacoplados entre sí. El sistema de spawn no interfiere con el sistema de seguidores, ya que los slimes solo se añaden a la cadena cuando son reclutados por el jugador. Esta separación permite mantener el código simple, legible y fácil de ampliar en futuras iteraciones del juego.

-Movimiento aleatorio (Wander)

 Se ha implementado un sistema de movimiento aleatorio para los slimes que les permite desplazarse de forma autónoma dentro del área de spawn. Los slimes alternan entre periodos de movimiento y reposo, eligiendo destinos aleatorios dentro de los límites definidos. Este comportamiento evita movimientos continuos y repetitivos y aporta mayor naturalidad a las entidades.

-Activación del miedo por proximidad (Fear)

 Se ha añadido un sistema de miedo que se activa cuando el jugador entra en un radio determinado. Al detectarse la proximidad del jugador, el slime cambia su estado de comportamiento y deja de deambular para reaccionar ante la amenaza. El radio de activación es configurable desde la escena, lo que permite ajustar fácilmente la sensibilidad del sistema.

-Huida en dirección opuesta al jugador (Flee)

 Cuando el miedo está activo, los slimes se desplazan en dirección opuesta a la posición del jugador, alejándose de él de forma coherente. Este movimiento se calcula mediante vectores de dirección y se mantiene limitado dentro del área de spawn, evitando que los slimes salgan del escenario.

-Relación entre rareza y miedo

 El nivel de miedo de cada slime se calcula a partir de su probabilidad de aparición. Los slimes con menor porcentaje de spawn son considerados más raros y presentan una reacción de huida más intensa. Esta relación es dinámica, de modo que cualquier cambio en las probabilidades de spawn afecta directamente al comportamiento sin necesidad de modificar el código.

-Uso de distribución normal en la velocidad

 La velocidad de huida incorpora una variación basada en una distribución normal. Esto provoca que la mayoría de los slimes tengan velocidades cercanas a una media, mientras que unos pocos se comportan de forma más extrema. Esta variabilidad introduce un comportamiento más orgánico y menos predecible.

-Diseño modular del comportamiento

 El comportamiento de los slimes se ha encapsulado en un único script encargado de gestionar el movimiento aleatorio y la huida. El sistema de spawn se limita a la creación de las entidades y a la asignación de parámetros, manteniendo una separación clara de responsabilidades y facilitando el mantenimiento y la ampliación del proyecto.

-Carga suicida de los slimes al boss

 Cuando aparece el boss, todos los slimes que están siguiendo al jugador abandonan la fila y se lanzan directamente hacia él. Este comportamiento sustituye cualquier sistema anterior de combate o disparos.

-Desaparición al impactar

 Cada slime desaparece automáticamente al tocar al boss, sin usar daño, proyectiles ni colisiones complejas. El impacto se detecta por distancia para asegurar un comportamiento estable.

-Muerte del boss por sacrificio total

 El boss no tiene vida ni recibe daño progresivo. Muere únicamente cuando todos los slimes que acompañaban al jugador se han sacrificado, reforzando una mecánica de “todo o nada”.

-Simplificación del sistema de combate

 Se ha eliminado la dependencia de tipos de slime, estadísticas de daño y scripts de combate anteriores. Todos los slimes se comportan igual durante el encuentro con el boss, priorizando claridad y legibilidad del código.

-Testeo y corrección

 Scripts y bugs arreglados tras varias pruebas del juego.

-Acabado del juego

 Implementación de UI. Incorporación de nuevas mecánicas: slimes que matan, muerte del personaje y pantalla de game over, pantalla de win. Opción de reinicio de partida y de salida del juego.


