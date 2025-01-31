Informe del Proyecto: Juego de Estrategia "Shinobi Quest"
Descripción del Proyecto
"Shinobi Quest" es un cautivador juego de estrategia por turnos que sumerge a los jugadores en un mundo vibrante y lleno de acción, donde los ninjas luchan por recuperar un artefacto robado que pone en peligro la paz de su universo. Este juego no solo es una prueba de habilidades estratégicas, sino también una experiencia inmersiva que combina elementos de narrativa, competencia y toma de decisiones. A medida que los jugadores avanzan en el tablero, deben navegar por trampas, aprovechar beneficios y utilizar portales mágicos, todo mientras intentan superar a su oponente. La mezcla de estrategia y azar garantiza que cada partida sea única y emocionante.
Características del Juego
1. Selección de Jugadores
El juego comienza con la entrada de dos jugadores, quienes deben elegir sus nombres y seleccionar fichas que representan a diferentes personajes del universo ninja. Cada personaje tiene habilidades especiales que pueden influir significativamente en el desarrollo del juego. Esta fase inicial no solo establece la identidad de los jugadores, sino que también les permite personalizar su experiencia de juego desde el principio, creando una conexión emocional con sus personajes.
2. Tablero Dinámico
El tablero de 25x25 casillas se genera aleatoriamente en cada partida, lo que asegura que cada experiencia sea única. Los jugadores se enfrentarán a una variedad de obstáculos (X), trampas (T), beneficios (B) y portales (P) en su camino. Esta aleatoriedad no solo añade un elemento de sorpresa, sino que también fomenta la rejugabilidad; cada partida presenta nuevos desafíos y oportunidades estratégicas.
3. Habilidades Especiales
Cada ficha cuenta con habilidades únicas que pueden ser utilizadas en momentos críticos del juego. Por ejemplo, algunas habilidades permiten aturdir a los oponentes, aumentar la velocidad o curar a compañeros. Este aspecto del juego no solo enriquece la jugabilidad, sino que también fomenta la interacción estratégica entre los jugadores. La gestión adecuada de estas habilidades puede ser decisiva para alcanzar la victoria, lo que añade una capa adicional de profundidad y complejidad al juego.
4. Interacción con el Tablero
Los jugadores no solo se mueven por el tablero; interactúan con él de maneras significativas. Al caer en una trampa, recoger un beneficio o utilizar un portal, las decisiones tomadas pueden tener consecuencias drásticas para el desarrollo del juego. Esta interactividad mantiene a los jugadores alertas y comprometidos en todo momento, ya que cada movimiento puede resultar en una victoria o una derrota inminente.
5. Condiciones de Victoria
El objetivo es claro: ser el primero en alcanzar la meta (G). Sin embargo, el camino está lleno de desafíos y sorpresas que pueden cambiar rápidamente la situación. La dinámica competitiva entre los jugadores añade emoción y tensión al juego, haciendo que cada turno sea crucial para determinar el resultado final.
Instrucciones de Instalación y Ejecución
Requisitos Previos
Para disfrutar plenamente de "Shinobi Quest", asegúrate de tener instalado .NET SDK en tu máquina.
Pasos para Instalar y Jugar
Clonar el Repositorio:
bash
git clone https://github.com/Darknesshide8/Daniel_Es_Gay.git
cd proyectobb
Compilar el Proyecto:
bash
dotnet build
Ejecutar el Juego:
bash
dotnet run
Seguir las Instrucciones en Pantalla: Una vez que el juego se inicie, sigue las instrucciones para ingresar los nombres de los jugadores y seleccionar las fichas.
Dificultades Encontradas
El desarrollo de "Shinobi Quest" no estuvo exento de desafíos significativos que pusieron a prueba mis habilidades como programador y diseñador de juegos. Cada obstáculo superado no solo mejoró el juego, sino que también proporcionó valiosas lecciones sobre programación y diseño.
1. Habilidades de los Personajes
Uno de los problemas más frustrantes fue que las habilidades especiales no funcionaban como se esperaba. Esto generaba frustración tanto para mí como para los testers del juego, ya que algunas habilidades no se activaban correctamente o no tenían efecto en el juego. Para resolver este problema, revisé meticulosamente la lógica detrás de cada habilidad, asegurándome de que las condiciones para su activación fueran claras y bien definidas.
Solución Implementada
Realicé un desglose detallado del código relacionado con cada habilidad.
Añadí comentarios explicativos para clarificar la lógica detrás de cada habilidad.
Implementé pruebas exhaustivas para asegurarme de que cada habilidad funcionara como estaba previsto.
Este proceso no solo mejoró la funcionalidad del juego, sino que también me enseñó la importancia de documentar adecuadamente el código para facilitar futuras modificaciones.
2. Distorsión del Mapa
Durante las pruebas iniciales, descubrí que el mapa a veces se distorsionaba debido a la colocación aleatoria de elementos en posiciones ocupadas. Esto arruinaba la experiencia del jugador al hacer que algunas casillas fueran intransitables o invisibles. Para solucionar este problema, implementé una verificación más rigurosa antes de colocar cualquier elemento en el tablero.
Solución Implementada
Introduje un algoritmo más robusto para verificar si una posición estaba ocupada antes de colocar un nuevo elemento.
Añadí mensajes informativos para ayudar a los jugadores a entender por qué ciertos movimientos no eran posibles.
Esta experiencia subrayó la importancia de validar todas las entradas y acciones dentro del juego para garantizar una experiencia fluida.
3. Cierre Inesperado del Juego
Otro desafío importante fue el cierre inesperado del juego cuando los usuarios presionaban "Enter" sin proporcionar una entrada válida. Esto era frustrante y podía arruinar la experiencia del jugador. Para abordar este problema, añadí validaciones exhaustivas a todas las entradas del usuario.
Solución Implementada
Implementé bucles while para validar las entradas hasta obtener respuestas correctas.
Proporcioné mensajes claros sobre qué tipo de entrada se esperaba.
Este enfoque mejoró significativamente la estabilidad del juego y garantizó una experiencia más satisfactoria para los usuarios.
4. Gestión del Turno
La lógica inicial para gestionar los turnos era confusa y podía llevar a situaciones donde un jugador podía saltarse su turno sin razón aparente. Refiné esta lógica para garantizar que cada jugador tuviera un turno claro y justo.
Solución Implementada
Definí claramente cuándo un jugador puede actuar y cuándo debe esperar su turno.
Añadí mensajes informativos sobre quién es el siguiente jugador y qué acciones puede realizar.
Este refinamiento no solo mejoró la jugabilidad sino también fomentó un ambiente competitivo más justo entre los jugadores.
Explicación de Métodos y Clases
Clases Principales
Program: Clase principal donde reside toda la lógica del juego.
Token: Representa las fichas (personajes) controladas por los jugadores; incluye propiedades como posición, velocidad y estado (aturdido o protegido).
Character: Define las características únicas de cada personaje, incluyendo su habilidad especial.
Métodos Clave
Main()
Este método inicializa el juego solicitando nombres a los jugadores y seleccionando sus fichas. También genera el tablero y muestra instrucciones sobre cómo jugar.
InitializeTokens()
Crea una lista con todos los personajes disponibles para seleccionar como fichas, permitiendo a los jugadores elegir según su estilo estratégico.
GenerateBoard(int size)
Genera un tablero vacío e inserta obstáculos, trampas, beneficios y portales aleatoriamente, asegurando una experiencia única en cada partida.
DisplayInstructions()
Muestra las reglas del juego a los jugadores de manera clara y concisa.
PlayGame(char[,] board, List<List<Token>> playerTokens)
Controla el flujo general del juego alternando turnos entre los jugadores hasta que uno alcance la meta.
MoveToken(Token token, char[,] board)
Permite a un jugador mover su ficha según las instrucciones proporcionadas por el usuario.
UseSpecialAbility(Token token, List<Token> currentPlayerTokens)
Ejecuta la habilidad especial seleccionada por el jugador durante su turno.
Métodos de Interacción
HandleBoardInteraction(Token token, char[,] board): Gestiona interacciones al moverse a una casilla específica (beneficios, trampas o portales).
ApplyRandomBenefit(Token token): Aplica beneficios aleatorios al jugador cuando recoge ítems especiales.
Validaciones y Control de Errores
El código incluye validaciones exhaustivas para asegurar entradas correctas por parte del usuario (por ejemplo, al seleccionar fichas o mover tokens). También maneja excepciones para evitar cierres inesperados del programa.
Conclusión
"Shinobi Quest" es más que un simple juego; es una experiencia estratégica emocionante llena de decisiones críticas y sorpresas inesperadas en cada turno. A través de este proyecto he aprendido valiosas lecciones sobre programación orientada a objetos y diseño lógico mientras creaba un entorno divertido e interactivo para los jugadores.
Te invito a probar "Shinobi Quest". ¡Reúne a tus amigos o familiares y sumérgete en esta aventura ninja! Cada partida promete ser única gracias a su dinámica aleatoria y estratégica. ¿Tienes lo necesario para superar todos los desafíos? ¡La aventura comienza ahora!
Repositorio en GitHub
Puedes encontrar este proyecto en mi repositorio de GitHub aquí. ¡Espero tus comentarios y sugerencias!