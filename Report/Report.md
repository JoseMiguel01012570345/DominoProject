GrandMaster Domino

Para entender la idea detras de este proyecto, primero hay que entender el domino a fondo y comprender todas sus características.
El domino es un juego que se desarrolla por turnos entre uno o más jugadores; en cada turno un jugador agrega al desarrollo del juego una o mas fichas que cumplan con algún criterio definido por las reglas de la variante que se esté jugando en el momento. Según sea esta variante, las fichas se pueden comparar entre sí y por lo tanto puede existir un criterio de orden entre ellas. A su vez, las fichas pueden ser repartidas al comienzo del juego, o irse repartiendo a medida que se desarrolla el juego, este acaba cuando se cumplen las condiciones para que  exista un ganador o cuando se ha llegado a un punto en el que el juego se estanca.Según la variante de domino es la forma en que se determinan los ganadores del juego en caso de haber uno.

Si observamos estas características desde un punto de vista mas general y abstracto, podemos decir que el domino es un juego que inicia, se desarrolla, tiene una forma de determinar su fin, el orden de los jugadores, los ganadores, y de actualizar su estado; y a su vez es capaz de decidir de que manera va a hacer todas estas características.
Para abstraernos de esto, se creó la interface "IGame" y la interface "IGameModesSelector", la cuales reunen todas las características de un juego, o sea:
-- Inicia
-- Actualiza su estado
-- Decide su ganador
-- Decide el orden de sus jugadores
-- Determina cuando termina el juego
-- Decide de que forma se va a realizar cada uno de estos procesos

IGameModeSelector seria el encargado de conectar con el visual que este a su vez conecta con el usuario

La interfaz IDomino intenta resumir las caracteristicas de un juego de domino, la diferencia entre IGame e IDomino es que la primera hace alucion a un juego y la segunda a un juego de domino. Por todo el texto del proyecto se podran encontrar tantas interfaces como abstraciones hemos considerado en un juego de domino incluso se ha tomado en cosideracion implementacinones variables de los jueces. Es decir podemos pensar que un juez es quien decide el orden de los turnos(entre otras cosas) y de aqui considerar tipos de jueces que calculen siguientes jugadores como es el caso del juez que deja jugar en un turno tantas veces como se pueda(hasta que no lleve mas).

Hemos implementado jugadores que se esperarian en una partida de domino. Un jugador que juega aleatorio, otro que juega a botar la ficha con mas numeros, aquel que intenta no pasarse(el agachado), un jugador que simula el juego hasta cuatro turnos por delante y usa las probabilidades para siempre poder jugar,entre otros. Por tanto la interfaz que define estos jugadores se llama IPlayer e implenta un metodo que recive un juego de domino y devuelve una pieza a jugar.

Hemos considerado la posibilidad de que en un juego no se repartan las fichas comunmente sino que podemos hacer que los jugadores escojan sus fichas a su antojo o que la vallan selecionando en el momento de jugar, muchas son las posibilidades y se  considera esta modalidad porque es atractiva para un juego.

Pensamos que implementar jugadores inmbatibles fuera una opcion por eso IPiecesHolder implementa un metodo que promete mostrar las fichas de todos los jugadores. Esto ultimo pareceria un error pero si se deceara que los jugadores no vean las fichas de los demas entonces se podria crear un juez que no permitirar esto al momento de dar el turno al jugador.

El como termina un juego fue tambien otra de nuestras consideraciones pues el juego se puede decidir de diversar formas para citar ejemplos, podemos acabar el juego cuando un jugador alcanza una cantidad de pases que han sido establecidos.

IDominoState es una interfaz que define el estado del domino, como este fluctua y los parametros que se iran registrando para que el juego y sus jugadores puedan servirse y funcionar correctamente. Para ello ella trae como definicion los lugares por donde se puede jugar , cantidad de fichas colocadas ect...

Quienes ganan y porque ganan es una idea que es recojida por la interfaz IComputerWinner que basicamente lo que pretende es selecionar de las formas implementadas de computar el ganador con el entorno visual de programa.

Como comienza el juego y como las fichas se acomplan son otras definiciones recojidas por IGrandMaster DominoControllerSelector e IStarter

Todo esto es encapsulado en un final por una clase denominada Domino que resume todas las caracteristicas del juego y las representa como un objeto generico el cual se va poder llevar a cada metodo para que estos a su vez puedan hacer uso de sus campos y modificarlo. Esta es probablemente la clase mas importante porque es la clase que nace de todo el conjunto de conceptos que se consideraron y que representa un Juego de Domino.

El conjunto de clases estaticas corresponden a las clases de implementacion estas que objetan y le dan vida a un concepto.

Hemos especificado en Domino un conjunto de campos de metodos que deben ser inicializados porque de no ser asi el juego no tendria forma de iniciarse. En caso de que no se inicien manualmente estos seran inicializados predeterminadamente. La clase de implementacion ClassiDomino y todos los ficheros que se pueden ver que tienen el prefijo Classic hacen alucion al domino clasico que se inicilizara en caso de no modificarse ningun campo como variente de domino. Hemos pensado que en caso de que una se intentase crear fichas que no involucren numeros como palabras o graficos de alguna especie podamos hacer Cast y traducir aquella representacion a un numero porque los numeros lo representan todo. Por eso siempre podemos trabajar implentar nuestras clases con tipos enteros y despues de hacer los calculos pertinentes hacer de nuevo Cast y traducir el resultad a la entidad nuevamente.

Para resumir algunas ideas diremos que hemos intentado hacer que nuestro proyecto se comporte como un IEnumerator el cual se deplaza con una propiedad CurrentPlayer y que valla en cada iteracion definiendoce. Al final es eso lo que es un juego que se desarrolla por turnos.

Nota:La interfaz grafica de este proyecto esta definida en Windows eso implica que el codigo de arranque solo iniciara en Windows
