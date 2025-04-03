
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<List<Token>> playerTokens = new List<List<Token>>();
    static List<string> playerNames = new List<string>();
    private static Random rand = new Random();
    static List<(int x, int y)> portals = new List<(int x, int y)>(); // Lista para almacenar posiciones de portales


    static void Main()
    {
        try
        {
            const int size = 25; //Esto define el tamaño del tablero
            List<Token> tokens = InitializeTokens(); //Inicializa los tokens de los personajes
            const int playerCount = 2; //Define el número de jugadores
            int tokenCount = GetTokenCount(); //Obtiene la cantidad de fichas por jugador

            //Ciclo para obtener los nombres de los jugadores y seleccionar sus fichas
            for (int i = 0; i < playerCount; i++)
            {
                string playerName;
                do
                {
                    Console.Write($"Ingrese el nombre del Jugador {i + 1}: ");
                    playerName = Console.ReadLine().Trim();//Eliminar espacios en blanco.
                    if (string.IsNullOrEmpty(playerName))
                    {
                        System.Console.WriteLine("El nombre no puede estar vacío, inténtalo de nuevo");
                    }
                    else if (playerNames.Contains(playerName))
                    {
                        System.Console.WriteLine("El nombre ya está en uso. Por favor, ingresa un nombre diferente.");
                    }
                }
                while (string.IsNullOrEmpty(playerName) || playerNames.Contains(playerName));
                playerNames.Add(playerName);
                playerTokens.Add(SelectTokens(tokens, tokenCount, playerTokens.SelectMany(t => t).ToList(), playerNames[i]));
                if (i == 0)
                {
                    Console.Clear();
                }
            }

            char[,] board = GenerateBoard(size);//Genera el tablero del juego
            
            DisplayInstructions();//Esto es para mostrar las instrucciones del juego
            DisplayStoryIntro();//Esto para mostrar la historia, es obvio XD
            DisplayBoard(board, playerTokens);//Muestra el tablero inicial
            PlayGame(board, playerTokens);// Inicia el ciclo de juego
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Se produjo un error: {ex.Message}. El juego continuará.");
        }
    }



    static void DisplayStoryIntro()
    {
        System.Console.WriteLine("\n==== HISTORIA ===\n" +
        "En el mundo shinobi, un antiguo artefacto conocido como el 'Sello de los Cuatro Pilares' ha sido robado.\n" +
        "Este poderoso sello es crucial para mantener el equilibrio entre las fuerzas del bien y el mal, ya que contiene la esencia de los bijuus.\n" +
        "Si cae en manos equivocadas, podría desatar un caos inimaginable sobre las aldeas ocultas. " +
        "Varios grupos de ninjas se han envuelto en la búsqueda, entre ellos, dos bandos controlados por fuerzas inimaginables que están en contra una de la otra.\n" +
        "Los jugadores asumirán el papel de dichos bandos que están en enemistad, con habilidades únicas, estos deben atravesar un territorio peligroso lleno de trampas y obstáculos.\n" +
        "El objetivo es llegar al Santuario del Sello de los Cuatro Pilares, donde se guarda el artefacto robado, antes de que los ninjas ladrones logren activar su poder.\n" +
        "Pero cuidado: Se reitera que el camino está lleno de obstáculos, trampas muy peligrosas, portales que pueden llevarlos a lugares desconocidos, aunque también hay pergaminos que pueden ayudarlos en la misión.\n" +
        "¿Tienen lo necesario para superar los desafíos y restaurar la paz en el mundo ninja? ¡La aventura comienza ahora!");
        System.Console.WriteLine("Presione cualquier tecla para continuar.");
        Console.ReadKey();//Espera a que el usuario presione una tecla

    }

    static void DisplayInstructions()
    {
        Console.WriteLine("\n=== INSTRUCCIONES DEL JUEGO ===");
        Console.WriteLine("1. Cada jugador selecciona sus fichas.");
        Console.WriteLine("2. En cada turno, los jugadores pueden mover sus fichas o usar habilidades especiales.");
        Console.WriteLine("3. El objetivo es llegar a la meta (G) en el tablero. El primer jugador que llegue con al menos una ficha ganará la partida.");
        Console.WriteLine("4. Las fichas tienen habilidades únicas que pueden usar para ayudarse o perjudicar a los oponentes.");
        Console.WriteLine("5. Otros elementos del mapa serán las trampas (T), obstáculos (X), portales (P), y bonificaciones (B)");
        Console.WriteLine("6. Los portales estarán conectados entre sí, es decir, si entras con una ficha a alguno, spawnearás de manera aleatoria en cualquier otro portal.");
        Console.WriteLine("7. Ten cuidado con las trampas. Cada una tendrá un efecto diferente.");
        Console.WriteLine("8. Por mucho que lo intentes, no podrás atravesar los obstáculos, no pierdas tu tiempo en ello.");
        Console.WriteLine("9. Los beneficios tendrán ciertos efectos que te ayudarán. Serán recogidos automáticamente cuando pases por sus casillas aunque no caigas en ellas.");
        Console.WriteLine("10. Presta mucha atención al camino, cada ficha tendrá una velocidad específica que se traduce en la cantidad de casillas que se desplaza por movimiento en su turno.");
        Console.WriteLine("11. Aunque la velocidad de tu ficha sobrepase la posición de una trampa, si te mueves justo en esa dirección el efecto SERÁ ACTIVADO.");
        Console.WriteLine("12. Lo anteriormente mencionado también aplica a los portales y la meta. Relájate ¿Ves? No todo es malo XD.");
        Console.WriteLine("13. Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    static int GetTokenCount()
    {
        int tokenCount;
        do
        {
            Console.WriteLine("¿Cuántas fichas desea por jugador? (1 o 2):");
        } while (!int.TryParse(Console.ReadLine() ?? string.Empty, out tokenCount) || (tokenCount < 1 || tokenCount > 2));//Esto es para validar la entrada.
        return tokenCount;//devuelve la cantidad válida de fichas seleccionadas.
    }
   
   static char[,] GenerateBoard(int size)
{
    char[,] board;
    do
    {
        board = new char[size, size];
        
        // Inicializar todo como vacío
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                board[i, j] = '.';

        // Colocar elementos aleatorios primero
        PlaceRandomElements(board, 'X', 65);
        PlaceRandomElements(board, 'T', 3);
        PlaceRandomElements(board, 'B', 5);
        PlaceRandomElements(board, 'P', 3);

        // Colocar la meta al final (Porque me salian DOS) XD 
        PlaceGoal(board); 

    } while (!IsBoardAccessible(board)); // Validar accesibilidad
    
    return board;
}

static void PlaceGoal(char[,] board)
{
    int goalX, goalY;
    int maxAttempts = 100; // Evitar bucles infinitos
    do
    {
        goalX = rand.Next(board.GetLength(0));
        goalY = rand.Next(board.GetLength(1));
        maxAttempts--;
    } 
    while ((board[goalX, goalY] != '.' || !IsGoalAccessible(board, goalX, goalY)) && maxAttempts > 0);

    if (maxAttempts > 0)
        board[goalX, goalY] = 'G';
    else
        throw new Exception("No se pudo colocar la meta en una posición válida.");
}

static bool IsGoalAccessible(char[,] board, int goalX, int goalY)
{
    // La meta debe tener al menos una casilla adyacente libre
    int[] dx = { -1, 1, 0, 0 };
    int[] dy = { 0, 0, -1, 1 };
    for (int i = 0; i < 4; i++)
    {
        int x = goalX + dx[i];
        int y = goalY + dy[i];
        if (x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1) && board[x, y] != 'X')
            return true;
    }
    return false;
}


static bool IsBoardAccessible(char[,] board)
{
    int size = board.GetLength(0);
    bool[,] visited = new bool[size, size];
    int goalX = -1, goalY = -1;

    // Encontrar la posición de la meta 'G'
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            if (board[i, j] == 'G')
            {
                goalX = i;
                goalY = j;
                break;
            }
        }
        if (goalX != -1) break;
    }

    if (goalX == -1) return false; // No hay meta

    // DFS desde la meta
    DFS(board, goalX, goalY, visited);

    // Verificar que todas las casillas accesibles (. T B P) estén visitadas
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            if (board[i, j] != 'X' && !visited[i, j])
                return false;
        }
    }
    return true;
}

static void DFS(char[,] board, int x, int y, bool[,] visited)
{
    if (x < 0 || x >= board.GetLength(0) || y < 0 || y >= board.GetLength(1) || visited[x, y] || board[x, y] == 'X')
        return;

    visited[x, y] = true;

    // Explorar direcciones adyacentes
    DFS(board, x + 1, y, visited);
    DFS(board, x - 1, y, visited);
    DFS(board, x, y + 1, visited);
    DFS(board, x, y - 1, visited);
}

    
    static void PlaceRandomElements(char[,] board, char element, int count)
    {
        for (int i = 0; i < count; i++)
            PlaceRandomElement(board, element); //Esto llama a PlaceRandomElement para colocar elementos aleatorios en el mapa
    }

    static void PlaceRandomElement(char[,] board, char element)
{
    int x, y;
    do
    {
        x = rand.Next(board.GetLength(0));
        y = rand.Next(board.GetLength(1));
    } 
    // Solo posiciones vacías (.) o la meta (G) si el elemento a colocar es 'G'
    while (board[x, y] != '.' && (element != 'G' || board[x, y] != 'G')); 

    // Si el elemento no es la meta, no puede colocarse sobre otra 'G', yo me entiendo, es que cuando lo hice me salían dos metas xD
    if (element != 'G' && board[x, y] == 'G') return; 

    board[x, y] = element;
    
    // Si es un portal, agregar a la lista
    if (element == 'P')
        portals.Add((x, y));
}
    static void PlayGame(char[,] board, List<List<Token>> playerTokens)
    {
        while (true)
        {
            for (int i = 0; i < playerTokens.Count; i++)
            {
                bool allStunned = playerTokens[i].All(token => !token.CanMoveThisTurn);
                Token sakuraToken = playerTokens[i].FirstOrDefault(token => token.Character.Name == "Sakura");

                //Si todas las fichas están aturdidas y no hay Sakura o está aturdida con enfriamiento.
                if (allStunned && (sakuraToken == null || sakuraToken.CanMoveThisTurn || sakuraToken.CooldownRemaining > 0))
                {
                    System.Console.WriteLine($"{playerNames[i]} no puede jugar este turno porque todas sus fichas están aturdidas.");
                    continue;
                }

                Console.WriteLine($"\nTurno de {playerNames[i]}");
                PlayerTurnSelection(playerTokens[i], board);
                DisplayBoard(board, playerTokens);
                if (playerTokens[i].Any(token => board[token.X, token.Y] == 'G'))
                {
                    Console.WriteLine($"¡{playerNames[i]} ha llegado a la meta y ha ganado!");
                    return;
                }
            }
            UpdateCooldowns(playerTokens);
            PauseForRead();
        }
    }

    static void PauseForRead()
    {
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }
    static void PlayerTurnSelection(List<Token> tokens, char[,] board)
    {
        Console.WriteLine("Seleccione la ficha que desea mover:");
        ShowTokenDetails(tokens);

        int choiceIndex;
        while (true)
        {
            string input = Console.ReadLine();
            if (!int.TryParse(input, out choiceIndex) || choiceIndex < 1 || choiceIndex > tokens.Count)
            {
                Console.WriteLine("Selección no válida. Intente nuevamente.");
                ShowTokenDetails(tokens);
                continue;
            }

            Token selectedToken = tokens[choiceIndex - 1];
            if (selectedToken.Character.Name == "Sakura" && !selectedToken.CanMoveThisTurn)
            {
                System.Console.WriteLine($"{selectedToken.Character.Name} está aturdida, pero puedes usar su habilidad para liberar el aturdimiento ¿Deseas hacerlo? (S/N)");
                UseSpecialAbility(selectedToken, tokens);
                break;
            }
            if (!selectedToken.CanMoveThisTurn)
            {
                System.Console.WriteLine($"{selectedToken.Character.Name} no puede moverse este turno debido a estar aturdido. Por favor, seleccione otra ficha.");
                ShowTokenDetails(tokens);//Mostrar detalles
                continue;
            }

            // Validación de enfriamiento antes de permitir el uso de habilidades
            if (selectedToken.CanUseAbility)
            {
                Console.WriteLine($"¿{selectedToken.Character.Name} quiere usar su habilidad especial? (S/N)");
                char abilityChoice;
                do
                {
                    abilityChoice = Console.ReadKey().KeyChar;
                    Console.WriteLine(); // Para saltar a la siguiente línea después de leer la tecla
                    if (char.ToUpper(abilityChoice) == 'S')
                    {
                        UseSpecialAbility(selectedToken, tokens);
                    }
                    else if (char.ToUpper(abilityChoice) != 'N')
                    {
                        Console.WriteLine("Opción no válida. Por favor ingrese S o N.");
                    }
                } while (char.ToUpper(abilityChoice) != 'S' && char.ToUpper(abilityChoice) != 'N');
            }
            else
            {
                Console.WriteLine($"{selectedToken.Character.Name} no puede usar su habilidad especial este turno debido a enfriamiento.");
            }

            PlayerTurn(selectedToken, board);
            break;
        }
    }



    static void ShowTokenDetails(List<Token> tokens)
    {
        Console.WriteLine("\n=== DETALLES DE LAS FICHAS ===");
        for (int i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            Console.WriteLine($"{i + 1}. {token.Character.Name}: Velocidad: {token.Character.Speed}, " +
                              $"Coordenadas: ({token.X}, {token.Y}), " +
                              $"Habilidad en enfriamiento: {(token.CanUseAbility ? "Listo" : $"{token.CooldownRemaining} turnos")}");
            Console.WriteLine($"Descripción de Habilidad: {GetAbilityDescription(token.Character.Name)}\n");
        }

    }

    static string GetAbilityDescription(string characterName) => characterName switch
    {
        "Sasuke" => "Chidori Nagashi: Aturde a todos los oponentes en rango.",
        "Naruto" => "Sage Mode: Aumenta su velocidad permanentemente.",
        "Kakashi" => "Kamui: Reduce la velocidad de los oponentes.",
        "Sakura" => "Curación: Libera a un compañero del aturdimiento.",
        "Hinata" => "64 puntos del Kage: Aturde a un oponente en rango.",
        "Shikamaru" => "Estrategia: Permite un movimiento extra.",
        "Gaara" => "Defensa de Arena: Protege contra aturdimientos durante tres turnos.",
        "Itachi" => "Genjutsu: Aturde a un oponente en rango.",
        "Tobi" => "Teletransportación: Se mueve instantáneamente a una posición aleatoria.",
        "Madara" => "Poder Abismal: Aumenta su velocidad enormemente.",
        _ => "No tiene habilidades especiales."
    };

    static void UseSpecialAbility(Token token, List<Token> currentPlayerTokens)
    {
        //Verificar si la habilidad puede ser utilizada
        if (!token.CanUseAbility)
        {
            System.Console.WriteLine($"{token.Character.Name} no puede usar su habilidad especial este turno debido a su enfriamiento");
            return;
        }
        // Obtener las fichas del oponente
        List<Token> opponentTokens = GetOpponentTokens(currentPlayerTokens);

        // Filtrar las fichas del oponente que están dentro del rango
        List<Token> tokensInRange = opponentTokens.Where(opponent => IsInRange(token.X, token.Y, opponent.X, opponent.Y)).ToList();

        switch (token.Character.Name)
        {
            case "Sasuke":
                if (tokensInRange.Count > 0)
                {
                    Console.WriteLine($"{token.Character.Name} ha activado Chidori Nagashi!");
                    foreach (var opponent in tokensInRange)
                    {
                        if (!opponent.IsProtected)
                        {
                            opponent.CanMoveThisTurn = false;
                            opponent.StunTurnsRemaining = 2;
                            Console.WriteLine($"{opponent.Character.Name} ha sido aturdido por Chidori Nagashi!");
                        }
                        else
                        {
                            System.Console.WriteLine($"{opponent.Character.Name} está protegido contra aturdimientos.");
                        }
                    }
                    token.StartCooldown(); // Comienza el tiempo de enfriamiento
                    ShowTokensInRange(tokensInRange); // Mostrar fichas en rango
                }
                else
                {
                    Console.WriteLine("No hay fichas del oponente en rango. La habilidad Chidori Nagashi no se puede activar.");
                }
                break;

            case "Itachi":
                if (tokensInRange.Count > 0)
                {
                    Console.WriteLine($"{token.Character.Name} ha activado Genjutsu! Aturdiendo a los oponentes.");
                    foreach (var opponent in tokensInRange)
                    {
                        if (!opponent.IsProtected)
                        {
                            opponent.CanMoveThisTurn = false;
                            opponent.StunTurnsRemaining = 2;
                            Console.WriteLine($"{opponent.Character.Name} ha sido atrapado en Genjutsu!");
                        }
                        else
                        {
                            System.Console.WriteLine($"{token.Character.Name} está protegido contra aturdimientos.");
                        }
                    }
                    token.StartCooldown(); // Comienza el tiempo de enfriamiento
                    ShowTokensInRange(tokensInRange); // Mostrar fichas en rango
                }
                else
                {
                    Console.WriteLine("No hay fichas del oponente en rango. La habilidad Genjutsu no se puede activar.");
                }
                break;

            case "Naruto":
                Console.WriteLine($"{token.Character.Name} ha activado Sage Mode! Aumentando su velocidad permanentemente.");
                token.IncreaseSpeed(2);
                token.StartCooldown();
                break;

            case "Kakashi":
                Console.WriteLine($"{token.Character.Name} ha activado Kamui! Reduciendo la velocidad del oponente.");
                foreach (var opponent in opponentTokens)
                {
                    opponent.DecreaseSpeed(1);
                    Console.WriteLine($"{opponent.Character.Name} ahora tiene una velocidad reducida a {opponent.Character.Speed}.");
                }
                token.StartCooldown();
                break;

            case "Sakura":
                if (!token.CanMoveThisTurn)
                {
                    System.Console.WriteLine($"{token.Character.Name} está aturdida ¿Desea usar su habilidad para quitarse el stun? (S/N)");
                    char choice;
                    do
                    {
                        choice = Console.ReadKey().KeyChar;
                        System.Console.WriteLine();
                        if (char.ToUpper(choice) == 'S')
                        {
                            token.CanMoveThisTurn = true;
                            System.Console.WriteLine($"{token.Character.Name} ha sido liberada del stun.");
                            token.StartCooldown();
                            return;
                        }
                        else if (char.ToUpper(choice) != 'N')
                        {
                            System.Console.WriteLine("Opción no válida. Por favor ingrese S o N.");
                        }
                    } while (char.ToUpper(choice) != 'S' && char.ToUpper(choice) != 'N');
                    System.Console.WriteLine("No se usó la habilidad de curación. Seleccione otra ficha para mover.");
                    return;
                }

                Token stunnedToken = currentPlayerTokens.FirstOrDefault(t => !t.CanMoveThisTurn && t != token);
                if (stunnedToken != null)
                {
                    stunnedToken.CanMoveThisTurn = true;
                    Console.WriteLine($"{token.Character.Name} ha activado Curación");
                    Console.WriteLine($"{stunnedToken.Character.Name} ha sido liberado del stun.");
                    token.StartCooldown();
                }
                else
                {
                    System.Console.WriteLine("No hay fichas aturdidas en tu equipo. La habilidad de Sakura no se puede usar.");
                }
                break;
            case "Hinata":
                var opponentTokensForHinata = GetOpponentTokens(currentPlayerTokens);
                List<Token> tokensInRangeForHinata = opponentTokensForHinata.Where(opponent => IsInRange(token.X, token.Y, opponent.X, opponent.Y)).ToList();
                if (tokensInRangeForHinata.Count > 0)
                {
                    System.Console.WriteLine($"{token.Character.Name} ha activado 64 puntos del Kage. Seleccione una ficha para aturdir.");
                    for (int i = 0; i < opponentTokensForHinata.Count; i++)
                        Console.WriteLine($"{i + 1}. {opponentTokensForHinata[i].Character.Name}");

                    int opponentTargetIndex;
                    while (!int.TryParse(Console.ReadLine(), out opponentTargetIndex) || opponentTargetIndex < 1 || opponentTargetIndex > opponentTokensForHinata.Count)
                        Console.WriteLine("Selección no válida. Intente nuevamente.");

                    Token targetOpponentToken = opponentTokensForHinata[opponentTargetIndex - 1];
                    if (!targetOpponentToken.IsProtected)
                    {
                        targetOpponentToken.CanMoveThisTurn = false;
                        targetOpponentToken.StunTurnsRemaining = 2;
                        Console.WriteLine($"{targetOpponentToken.Character.Name} ha sido aturdido por 64 puntos del Kage durante dos turnos!");
                    }
                    else
                    {
                        System.Console.WriteLine($"{targetOpponentToken.Character.Name} está protegido contra aturdimientos.");
                    }
                    token.StartCooldown();
                }
                else
                {
                    Console.WriteLine($"No hay fichas del oponente en rango. La habilidad 64 puntos del Kage no se puede activar");
                }
                break;

            case "Shikamaru":
                Console.WriteLine($"{token.Character.Name} ha activado Estrategia! Permitiendo un movimiento extra.");
                token.CanMoveThisTurn = true; // Permitir el movimiento extra
                token.StartCooldown();
                break;

            case "Gaara":
                if (!token.IsProtected)
                {
                    token.IsProtected = true;
                    token.ProtectTurnsRemaining = 3; // Protege durante tres turnos
                    Console.WriteLine($"{token.Character.Name} ha activado Defensa de Arena! No puede ser aturdido durante los próximos {token.ProtectTurnsRemaining} turnos.");
                    token.StartCooldown();
                }
                else
                    Console.WriteLine($"{token.Character.Name} ya está protegido por Defensa de Arena.");
                break;

            case "Tobi":
                Console.WriteLine($"{token.Character.Name} ha activado Teletransportación! Moviéndose instantáneamente a una posición aleatoria.");
                TeleportToken(token);
                token.StartCooldown();
                break;

            case "Madara":
                Console.WriteLine($"{token.Character.Name} ha activado Poder Abismal! Aumentando su velocidad enormemente.");
                token.IncreaseSpeed(3);
                token.StartCooldown();
                token.SpeedReductionTurnsRemaining = 1;
                break;

            default:
                Console.WriteLine($"{token.Character.Name} no tiene habilidades especiales.");
                break;
        }

        PauseForRead();
    }

    // Nuevo método para mostrar las fichas en rango
    static void ShowTokensInRange(List<Token> tokensInRange)
    {
        if (tokensInRange.Count > 0)
        {
            Console.WriteLine("Fichas del oponente en rango:");
            foreach (var t in tokensInRange)
            {
                Console.WriteLine($"- {t.Character.Name}: Coordenadas ({t.X}, {t.Y})");
            }
        }
    }


    static bool IsInRange(int x1, int y1, int x2, int y2) => Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2)) <= 6;


    static List<Token> GetOpponentTokens(List<Token> currentPlayerTokens) =>
        playerTokens.SelectMany(t => t).Except(currentPlayerTokens).ToList();
    static void PlayerTurn(Token token, char[,] board)
    {
        if (token.IsProtected && token.ProtectTurnsRemaining > 0)
        {
            token.ProtectTurnsRemaining--;
            if (token.ProtectTurnsRemaining == 0)
                token.IsProtected = false; // Se termina la protección
            Console.WriteLine(token.IsProtected ? $"{token.Character.Name} sigue protegido." : $"{token.Character.Name} ya no está protegido.");
        }

        if (!token.CanMoveThisTurn)
        {
            Console.WriteLine($"{token.Character.Name} no puede moverse este turno.");
            token.CanMoveThisTurn = true; // Restablecer para el siguiente turno
            return;
        }

        Console.WriteLine($"Turno de {token.Character.Name}. Posición actual: ({token.X}, {token.Y})");

        // Primer movimiento
        MoveAndInteract(token, board);

        // Si es Shikamaru y ha usado su habilidad, permitir otro movimiento
        if (token.Character.Name == "Shikamaru" && !token.CanUseAbility) // Cambiar a !token.CanUseAbility
        {
            Console.WriteLine("¿Desea realizar un movimiento adicional? (S/N)");
            if (char.ToUpper(Console.ReadKey().KeyChar) == 'S')
            {
                MoveAndInteract(token, board);
            }
        }
    }



    static void MoveAndInteract(Token token, char[,] board)
    {
        MoveToken(token, board);
        PauseForRead();
    }

    static void MoveToken(Token token, char[,] board)
    {
        while (true) // Bucle para permitir múltiples intentos
        {
            Console.WriteLine("Elija una dirección para moverse: arriba (W), abajo (S), izquierda (A), derecha (D).");
            var moveDirection = Console.ReadKey().KeyChar;
            Console.WriteLine(); // Para saltar a la siguiente línea después de leer la tecla

            int newX = token.X;
            int newY = token.Y;

            // Ajustar las coordenadas según la dirección
            switch (char.ToUpper(moveDirection))
            {
                case 'W':
                    newX -= token.Character.Speed; // Mover hacia arriba
                    break;
                case 'S':
                    newX += token.Character.Speed; // Mover hacia abajo
                    break;
                case 'A':
                    newY -= token.Character.Speed; // Mover hacia la izquierda
                    break;
                case 'D':
                    newY += token.Character.Speed; // Mover hacia la derecha
                    break;
                default:
                    Console.WriteLine("Dirección no válida. Intente nuevamente.");
                    continue; // Permite al jugador intentar nuevamente
            }

            // Ajustar las coordenadas para que no excedan los límites del tablero
            newX = Math.Max(0, Math.Min(newX, board.GetLength(0) - 1)); // Limitar a las filas
            newY = Math.Max(0, Math.Min(newY, board.GetLength(1) - 1)); // Limitar a las columnas

            bool validMove = true;

            for (int step = 1; step <= token.Character.Speed; step++)
            {
                int checkX = token.X;
                int checkY = token.Y;

                switch (char.ToUpper(moveDirection))
                {
                    case 'W':
                        checkX -= step; // Verificar arriba
                        break;
                    case 'S':
                        checkX += step; // Verificar abajo
                        break;
                    case 'A':
                        checkY -= step; // Verificar izquierda
                        break;
                    case 'D':
                        checkY += step; // Verificar derecha
                        break;
                }

                // Ajustar límites para verificar si está dentro del tablero
                if (checkX < 0 || checkX >= board.GetLength(0) || checkY < 0 || checkY >= board.GetLength(1))
                {
                    validMove = false; // Salir del bucle si se sale del tablero
                    break;
                }

                if (board[checkX, checkY] == 'X') // Si hay un obstáculo en el camino
                {
                    validMove = false; // No se puede mover más allá de este punto
                    break;
                }

            
                if (board[checkX, checkY] == 'T')
                {
                    System.Console.WriteLine($"{token.Character.Name} ha caído en una trampa al intentar moverse.");
                    token.Move(checkX, checkY);
                    TriggerTrap(token);
                    return;
                }

                if (board[checkX, checkY] == 'B')
                {
                    System.Console.WriteLine($"{token.Character.Name} ha recogido un beneficio.");
                    ApplyRandomBenefit(token);
                    board[checkX, checkY] = '.'; // Eliminar el beneficio al recogerlo.
                }

                if (board[checkX, checkY] == 'G')
                {
                    System.Console.WriteLine($"{token.Character.Name} ha llegado a la meta y ha ganado la partida.");
                    token.Move(checkX, checkY);
                    HandleBoardInteraction(token, board);
                    return;
                }

                if (board[checkX, checkY] == 'P')
                {
                    System.Console.WriteLine($"{token.Character.Name} ha encontrado un portal. Teletransportándose...");
                    TeleportToken(token);
                    return;
                }
            }


            if (!validMove)
            {
                Console.WriteLine("Movimiento bloqueado por un obstáculo. Intentando mover lo más cerca posible...");

                // Intentar mover al máximo permitido sin chocar con el obstáculo
                for (int step = 1; step <= token.Character.Speed; step++)
                {
                    int currentX = token.X;
                    int currentY = token.Y;

                    switch (char.ToUpper(moveDirection))
                    {
                        case 'W':
                            currentX -= 1;
                            break;
                        case 'S':
                            currentX += 1;
                            break;
                        case 'A':
                            currentY -= 1;
                            break;
                        case 'D':
                            currentY += 1;
                            break;
                    }

                    if (IsValidMove(board, currentX, currentY))
                    {
                        token.Move(currentX, currentY);
                        Console.WriteLine($"{token.Character.Name} se ha movido a: ({token.X}, {token.Y})");
                        HandleBoardInteraction(token, board);
                        return;
                    }
                }

                Console.WriteLine("No se pudo mover. Intente otra dirección.");
                continue; // Permite al jugador intentar nuevamente
            }

            // Si el movimiento es válido según las reglas del juego, mover el token paso a paso
            for (int step = 1; step <= token.Character.Speed; step++)
            {
                int currentX = token.X;
                int currentY = token.Y;

                switch (char.ToUpper(moveDirection))
                {
                    case 'W':
                        currentX -= 1;
                        break;
                    case 'S':
                        currentX += 1;
                        break;
                    case 'A':
                        currentY -= 1;
                        break;
                    case 'D':
                        currentY += 1;
                        break;
                }

                if (IsValidMove(board, currentX, currentY))
                {
                    token.Move(currentX, currentY);
                    Console.WriteLine($"{token.Character.Name} se ha movido a: ({token.X}, {token.Y})");

                    HandleBoardInteraction(token, board);

                    if (board[currentX, currentY] == 'G') return;

                    if (board[currentX, currentY] == 'T') return;

                    if (board[currentX, currentY] == 'P') return;

                    System.Threading.Thread.Sleep(500); // Pausa entre movimientos para visualización
                }
            }

            break; // Sale del bucle si el movimiento es válido y se ha completado
        }
    }

    static bool IsValidMove(char[,] board, int x, int y)
    {
        return x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1) && board[x, y] != 'X';
    }

    static bool IsPathClear(int startX, int startY, int endX, int endY, char[,] board)
    {
        int dx = Math.Sign(endX - startX); // Dirección en X
        int dy = Math.Sign(endY - startY); // Dirección en Y

        int x = startX;
        int y = startY;

        while (x != endX || y != endY)
        {
            x += dx;
            y += dy;

            // Verificar si la casilla actual es un obstáculo
            if (board[x, y] == 'X')
            {
                return false; // Hay un obstáculo en el camino
            }
        }

        return true; // El camino está libre
    }


    static void HandleBoardInteraction(Token token, char[,] board)
    {
        if (board[token.X, token.Y] == 'B')//Beneficios
        {
            Console.WriteLine($"{token.Character.Name} ha encontrado un beneficio");
            ApplyRandomBenefit(token);
            board[token.X, token.Y] = '.';//Esto es para borrar el beneficio del tablero
        }
        else if (board[token.X, token.Y] == 'P') // Portal
        {
            Console.WriteLine($"{token.Character.Name} ha encontrado un portal! Teletransportándose...");
            TeleportToken(token);
        }
        else if (board[token.X, token.Y] == 'T') // Trampa
        {
            Console.WriteLine($"{token.Character.Name} ha caído en una trampa!");
            TriggerTrap(token);
            // No eliminamos la trampa del tablero
        }
    }
    static void ApplyRandomBenefit(Token token)
    {
        Random rand = new Random();
        int benefitType = rand.Next(1, 4);
        switch (benefitType)
        {
            case 1:
                int speedIncrease = rand.Next(1, 3);//Aumento de velocidad entre 1 y 2
                token.IncreaseSpeed(speedIncrease);
                Console.WriteLine($"{token.Character.Name} ha aumentado su velocidad en {speedIncrease} turnos");
                break;
            case 2:
                token.CooldownRemaining = Math.Max(0, token.CooldownRemaining - 1); //Reducir el tiempo de enfriamiento en 1.
                System.Console.WriteLine($"{token.Character.Name} ha reducido su tiempo de enfriamiento en 1 turno.");
                break;
            case 3:
                token.IsProtected = true;
                token.ProtectTurnsRemaining = 5;
                //Proteger durante 5 turnos.
                System.Console.WriteLine($"{token.Character.Name} está protegido contra aturdimientos durante {token.ProtectTurnsRemaining} turnos.");
                break;
            default:
                System.Console.WriteLine("No se ha aplicado ningún beneficio.");
                break;
        }
    }

    static List<Token> SelectTokens(List<Token> tokensListToSelectFrom, int count,
                                     List<Token> alreadySelectedTokens,
                                     string playerName)
    {
        var selectedTokens = new List<Token>();
        for (int i = 0; i < count && tokensListToSelectFrom.Count > 0; i++)
        {
            Console.WriteLine($"{playerName}, seleccione su ficha #{i + 1}:");
            for (int j = 0; j < tokensListToSelectFrom.Count; j++)
                Console.WriteLine($"{j + 1}. {tokensListToSelectFrom[j].Character.Name} - {tokensListToSelectFrom[j].Character.Abbility} - {tokensListToSelectFrom[j].Character.Speedt} - {tokensListToSelectFrom[j].Character.CooldownTimet}");
            int choiceIndex;
            while (!int.TryParse(Console.ReadLine(), out choiceIndex) || choiceIndex < 1 || choiceIndex > tokensListToSelectFrom.Count)
            {
                Console.WriteLine("Selección no válida. Intente nuevamente.");
            }
            selectedTokens.Add(tokensListToSelectFrom[choiceIndex - 1]);
            tokensListToSelectFrom.RemoveAt(choiceIndex - 1);

        }
        return selectedTokens;
    }

    static List<Token> InitializeTokens()
    {
        var characters = new List<Character>{
          new Character{Name="Naruto",Abbility="Sage Mode: Aumenta la velocidad permanentemente.",Speedt="Velocidad: 2",CooldownTimet="Tiempo de enfriamiento: 8",Speed=2,CooldownTime=8},
          new Character{Name="Sakura",Abbility="Curación: Cura el efecto de stun.",Speedt= "Velocidad: 1",CooldownTimet="Tiempo de enfriamiento: 3",Speed=1,CooldownTime=3},
          new Character{Name="Sasuke",Abbility="Chidori Nagashi: Causa stun a todas las fichas en un rango específico: '8 casillas' durante 2 turnos",Speedt="Velocidad: 3",CooldownTimet="Tiempo de enfriamiento: 4",Speed=3,CooldownTime=4},
          new Character{Name="Kakashi",Abbility="Kamui: Disminuye la velocidad de las fichas enemigas en 1",Speedt="Velocidad: 2",CooldownTimet="Tiempo de enfriamiento: 6",Speed=2,CooldownTime=6},
          new Character{Name="Hinata",Abbility="64 puntos del Kage: Causa stun a una ficha que selecciones durante 2 turnos",Speedt="Velocidad 1",CooldownTimet="Tiempo de enfriamiento = 3",Speed=1,CooldownTime=3},
          new Character{Name="Shikamaru",Abbility = "Estrategia: Permite un movimiento adicional",Speedt= "Velocidad: 2",CooldownTimet="Tiempo de enfriamiento: 2",Speed=2,CooldownTime=2},
          new Character{Name="Gaara",Abbility="Protección: Lo protege contra stun durante 3 turnos",Speedt = "Velocidad: 1",CooldownTimet="Tiempo de enfriamiento: 5",Speed=1,CooldownTime=5},
          new Character{Name="Itachi",Abbility="Genjutsu: Causa stun a las fichas enemigas en un rango específico: '8 casillas' durante 2 turnos.",Speedt ="Velocidad: 3",CooldownTimet="Tiempo de enfriamiento: 3",Speed=3,CooldownTime=3},
          new Character{Name="Tobi",Abbility="Teletransportación: Teletransporta la ficha a una posición aleatoria.",Speedt="Velocidad:  2",CooldownTimet="Tiempo de enfriamiento: 4",Speed=2,CooldownTime=4},
          new Character{Name="Madara",Abbility="Poder Abismal: Aumenta enormemente su velocidad.",Speedt="Velocidad: 4",CooldownTimet="Tiempo de enfriamiento: 2",Speed=4,CooldownTime=2}
      };
        return characters.Select(character => new Token(character)).ToList();
    }


    static void TeleportToken(Token token)
    {
        // Elegir un portal aleatorio diferente al actual
        var otherPortals = portals.Where(p => p.x != token.X || p.y != token.Y).ToList();

        if (otherPortals.Count > 0)
        {
            var randomPortal = otherPortals[rand.Next(otherPortals.Count)];
            token.Move(randomPortal.x, randomPortal.y); // Teletransportar a la nueva posición
            Console.WriteLine($"{token.Character.Name} se ha teletransportado a: ({token.X}, {token.Y})");
        }
    }


    static void DisplayBoard(char[,] board, List<List<Token>> playerTokens)//este fue el metodo modifciado
    {
        // Lógica para mostrar el tablero
        Console.Clear();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            string line = "";//se agregó esta variable para almacenar el contenido de la fila
            for (int j = 0; j < board.GetLength(1); j++)
            {
                bool isPlayerToken = false;

                for (int p = 0; p < playerTokens.Count; p++)
                {
                    foreach (var token in playerTokens[p])
                    {
                        if (token.X == i && token.Y == j)
                        {
                            // Muestra los tokens de los jugadores como "(1 o 2)"
                            line += $"{p + 1} ";//aqui modificamos la variable en vez de imprimir directamente
                            isPlayerToken = true;
                            break;
                        }
                    }
                    if (isPlayerToken) break;
                }

                if (!isPlayerToken)
                    // Muestra el contenido del tablero
                    line += $"{board[i, j]} ";//aqui modificamos la variable en vez de imprimir directamente

            }
            // Nueva línea después de cada fila del tablero
            Console.WriteLine(line.TrimEnd() + $"  {i}");//aqui finalmente imprimimos la linea completa
        }
    }

    static void TriggerTrap(Token token)
    {
        // Efectos de las trampas
        int trapEffect = rand.Next(1, 4); // Genera un número aleatorio entre 1 y 3
        switch (trapEffect)
        {
            case 1: // Regresa al Token a su posición inicial
                Console.WriteLine($"{token.Character.Name} ha sido víctima de los ataques de un grupo de ninjas muy peligrosos y ha sido devuelto a su posición inicial.");
                token.Move(token.InitialX, token.InitialY);
                break;

            case 2: // Disminuye temporalmente la velocidad del Token
                Console.WriteLine($"{token.Character.Name} ha sido envenenado y su velocidad se ha reducido en 1 durante 7 turnos.");
                token.DecreaseSpeed(1); // Disminuir velocidad en 1
                token.SpeedReductionTurnsRemaining = 7; // Efecto dura 7 turnos
                break;

            case 3: // Aturde al Token por 3 turnos
                if (!token.IsProtected)
                {
                    Console.WriteLine($"{token.Character.Name} ha sido atrapado en un Genjutsu muy fuerte y ha sido aturdido por 3 turnos.");
                    token.CanMoveThisTurn = false;
                    token.StunTurnsRemaining = 3; // Efecto dura 3 turnos
                }
                else
                {
                    System.Console.WriteLine($"{token.Character.Name} está protegido contra aturdimientos y no puede ser aturdido.");
                }
                break;
        }
    }


    class Token
    {
        public Character Character { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool CanUseAbility => CooldownRemaining <= 0;
        public bool CanMoveThisTurn { get; set; }
        public int CooldownRemaining { get; set; }

        public bool IsProtected { get; set; }
        public int ProtectTurnsRemaining { get; set; }
        public int InitialX { get; private set; } // Nueva propiedad para almacenar la posición inicial
        public int InitialY { get; private set; }

        public int SpeedReductionTurnsRemaining { get; set; } // Nueva propiedad para controlar la reducción de velocidad
        public int StunTurnsRemaining { get; set; } // Nueva propiedad para controlar el aturdimiento
        public int OriginalSpeed { get; private set; }
        public Token(Character character)
        {
            Character = character;
            X = rand.Next(25);
            Y = rand.Next(25);
            InitialX = X; // Inicializar posición inicial
            InitialY = Y; // Inicializar posición inicial
            CanMoveThisTurn = true;
            CooldownRemaining = 0;
            IsProtected = false;
            ProtectTurnsRemaining = 0;
            SpeedReductionTurnsRemaining = 0; // Inicializar a 0
            StunTurnsRemaining = 0; // Inicializar a 0
            OriginalSpeed = character.Speed;
        }

        public void Move(int x, int y) { X = x; Y = y; }
        public void StartCooldown() { CooldownRemaining = Character.CooldownTime; }
        public void UpdateCooldown() { if (CooldownRemaining > 0) CooldownRemaining--; }
        public void IncreaseSpeed(int amount) { Character.Speed += amount; }
        public void DecreaseSpeed(int amount) { Character.Speed -= amount; if (Character.Speed < 1) Character.Speed = 1; }
    }

    class Character
    {
        public string Name { get; set; } = string.Empty;
        public int Speed { get; set; }
        public int CooldownTime { get; set; }
        public string Abbility { get; set; } = string.Empty;
        public string Speedt { get; set; }
        public string CooldownTimet { get; set; }
    }

    static void UpdateCooldowns(List<List<Token>> playerTokens)
    {
        foreach (var tokens in playerTokens)
        {
            foreach (var token in tokens)
            {
                token.UpdateCooldown();

                if (token.ProtectTurnsRemaining > 0)
                {
                    token.ProtectTurnsRemaining--;
                    if (token.ProtectTurnsRemaining == 0)
                    {
                        token.IsProtected = false;
                        System.Console.WriteLine($"{token.Character.Name} ya no está protegido contra aturdimientos.");
                    }
                }
                if (token.SpeedReductionTurnsRemaining > 0)
                {
                    token.SpeedReductionTurnsRemaining--;
                }
                else if (token.Character.Speed < token.OriginalSpeed)
                {
                    token.Character.Speed += 1;
                }

                if (token.StunTurnsRemaining > 0)
                {
                    token.StunTurnsRemaining--;
                    if (token.StunTurnsRemaining == 0)
                        token.CanMoveThisTurn = true;
                }
                else
                {
                    token.CanMoveThisTurn = true;
                }

            }
        }
    }
}
//Opcional poner numeros de fila y columna.
