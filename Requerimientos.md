# Buscador de Luz — Documento de Requerimientos

**Estudio:** Eclipsis Games
**Título anterior:** Fragmentos del Cosmos
**Género:** Plataformero 2D de acción y exploración
**Plataforma:** PC (Windows / Mac)
**Motor:** Unity 6 con Universal Render Pipeline (URP)
**Lenguaje:** C#
**Equipo:** 1 persona
**Periodo de producción:** 7 de septiembre de 2026 al 19 de noviembre de 2026
**Entregable:** Videojuego completo y terminado del Planeta 1 — Ignar (19 de noviembre de 2026)
**Proceso de desarrollo:** Huddle (Preproducción, Producción, Postmortem)

---

## Descripción general

**Buscador de Luz** es un plataformero 2D de acción y exploración para PC con estética pixel art de 16 bits. El jugador controla a **Kael**, el último Buscador de Luz, un explorador de armadura azul oscuro con detalles dorados que porta un Cristal de Luz, su arma y fuente de todas sus habilidades. Su misión es recuperar los Fragmentos de **Aethon**, la estrella central del universo que se está apagando, viajando por planetas congelados en el tiempo.

La historia se cuenta de forma visual, sin diálogos extensos: las ruinas, los guardianes y los cambios en el mundo cuando Kael restaura cada planeta narran lo ocurrido. Solo hay tres escenas con texto breve: la apertura, el fragmento obtenido y el final.

**Entregable de esta materia:** el Planeta 1, **Ignar**, un mundo volcánico de forjas industriales abandonadas dividido en tres zonas (Forjas Dormidas, Canales de Magma y El Núcleo). En cada zona el jugador avanza esquivando lava, géiseres y trampas, resuelve mecánicas de palancas, compuertas y válvulas, y enfrenta enemigos. Al final del planeta debe derrotar al jefe **Molrak**, el Golem Eterno, para obtener el Fragmento 1 de Aethon y desbloquear el Pulso de Calor.

**Dinámica de juego:**
1. Kael explora el nivel de izquierda a derecha, con saltos, doble salto y combate cuerpo a cuerpo.
2. Los obstáculos y enemigos le quitan corazones (tiene 5).
3. Los Cristales de Checkpoint guardan su progreso; si muere, reaparece en el último activado.
4. Los enemigos pueden soltar cristales que recuperan vida, y las zonas secretas esconden mejoras.
5. Al derrotar a Molrak, recoge el Fragmento y completa Ignar.

**Público objetivo:** jugadores casuales e intermedios de 13 a 30 años que disfrutan de los plataformeros 2D con exploración y combate (referentes: Hollow Knight y Ori and the Blind Forest, con una experiencia más accesible).

**Propuesta de valor:** un mundo coherente en estética y mecánicas, con una curva de aprendizaje natural y habilidades que se desbloquean al vencer a cada jefe, pensado para sesiones cortas de 10 a 20 minutos por planeta.

---

## 1. Alcance

### 1.1 Dentro del alcance (entregable de la materia)
- Planeta 1 — Ignar, con 3 zonas: Forjas Dormidas, Canales de Magma y El Núcleo.
- Personaje jugable: Kael.
- Enemigos: Golemita de Lava, Chispa Voladora y el jefe Molrak.
- Habilidad desbloqueable: Pulso de Calor.
- Fragmento 1 de Aethon como objetivo del nivel.
- Menú principal (con música), pausa y pantalla de Game Over.
- HUD con vida, fragmento y habilidad.
- Build ejecutable de Windows (`.exe`) que funcione fuera del editor.
- Documento de Diseño (Word), Documentos de Producción (Excel) y presentación.
- Partida completa de 10 a 20 minutos.

### 1.2 Fuera del alcance
- Planetas 2 al 7 (Veluun, Aqualys, Cyndra, Morkheim, Elaris y Aethon).
- Versión para Android.
- Multijugador, tienda, monedas, llaves, munición y power-ups.
- Códigos secretos.

---

## 2. Requerimientos funcionales

### 2.1 Personaje (Kael)
| ID | Requerimiento |
|---|---|
| RF-01 | Moverse horizontalmente con A / D o flechas. |
| RF-02 | Saltar y realizar doble salto con Espacio. |
| RF-03 | Voltear el sprite según la dirección de movimiento. |
| RF-04 | Atacar con la tecla J alternando dos animaciones (puñetazo y patada). |
| RF-05 | El ataque detecta enemigos en un radio frente a Kael (AttackPoint) y les aplica daño. |
| RF-06 | Reproducir animaciones de idle, caminar, saltar y atacar. |
| RF-07 | Tener 5 corazones de vida; al llegar a 0 muere. |
| RF-08 | Al quedarse sin vida se muestra la pantalla de Game Over; **Reintentar** reposiciona a Kael en el último checkpoint activado con la vida completa. |
| RF-09 | Dash con la tecla K, 0.3 s de invencibilidad y cooldown de 1.5 s. |
| RF-10 | Sprint al mantener Shift izquierdo. |
| RF-11 | Pulso de Calor (tecla Q, cooldown de 4 s): onda de fuego que empuja enemigos y rompe muros de roca; se desbloquea al recoger el Fragmento de Aethon. |

### 2.2 Cámara
| ID | Requerimiento |
|---|---|
| RF-12 | La cámara sigue a Kael con suavizado (Cinemachine). |
| RF-13 | La cámara no muestra zonas fuera del nivel (límites de cámara). |
| RF-14 | Fondo con efecto parallax. |

### 2.3 Salud, daño y checkpoints
| ID | Requerimiento |
|---|---|
| RF-15 | La lava daña a Kael al contacto, con intervalo entre daños de 1 s. |
| RF-16 | Los enemigos dañan a Kael al contacto o ataque con cooldown de 1 s. |
| RF-17 | Hay 3 Cristales de Checkpoint (uno por zona); al tocarlos cambian de sprite inactivo a activo y guardan la posición. |
| RF-18 | El GameManager guarda el checkpoint activo y reposiciona a Kael al morir. |
| RF-19 | Guardado del progreso con PlayerPrefs al activar un checkpoint y al derrotar al jefe. |

### 2.4 Enemigos
| ID | Requerimiento |
|---|---|
| RF-20 | **Golemita de Lava:** patrulla, detecta a Kael, lo persigue y lo ataca; tiene 3 de vida. |
| RF-21 | La Golemita deja rastro de lava que daña a Kael y desaparece a los 3 s. |
| RF-22 | La Golemita puede soltar un Cristal de Luz pequeño con 20 % de probabilidad al morir. |
| RF-23 | **Chispa Voladora:** flota en su posición, vuela en grupos de 3, ataca en picada y explota al contacto con Kael; tiene 2 de vida. |
| RF-24 | **Molrak:** jefe con 3 fases de combate (ver sección 4). |

### 2.5 Obstáculos y mecánicas del nivel
| ID | Requerimiento |
|---|---|
| RF-25 | Charcos y ríos de lava que dañan al contacto. |
| RF-26 | Géiseres de fuego que disparan una columna cada 3 s y dañan a Kael. |
| RF-27 | Palancas que, al activarse, abren compuertas. |
| RF-28 | Muros de roca que solo se rompen con el Pulso de Calor. |
| RF-29 | Zona secreta oculta tras una pared falsa (Zona 1) y tras una catarata de lava (Zona 2). |
| RF-30 | Puentes que se derrumban al pisarlos (Zona 2). |
| RF-31 | Puzzle de 3 válvulas que deben activarse en el orden correcto para abrir el paso a la Zona 3. |
| RF-32 | Lluvia de rocas volcánicas y géiseres más grandes (Zona 2). |
| RF-33 | Explosiones de magma aleatorias en el suelo (Zona 3) que avisan con un destello en el suelo 0.8 s antes de dañar. |
| RF-34 | Puerta del jefe que se cierra al entrar al combate. |

### 2.6 Items
| ID | Item | Efecto | Dónde aparece |
|---|---|---|---|
| RF-35 | Cristal de Luz pequeño | Recupera medio corazón | Drop de enemigos (20 %) |
| RF-36 | Cristal de Luz grande | Recupera 1 corazón | Zonas secretas |
| RF-37 | Cristal de Mejora | Reduce 0.5 s la recarga del Pulso de Calor y del ataque | Zonas secretas |
| RF-38 | Fragmento de Aethon | Objetivo del nivel; completa Ignar | Lo suelta Molrak |
| RF-39 | Cristal de Checkpoint | Guarda la posición | 3 fijos en el nivel (no se recoge) |

### 2.7 Interfaz (HUD y pantallas)
| ID | Requerimiento |
|---|---|
| RF-40 | HUD con 5 corazones que reflejan la vida actual de Kael. |
| RF-41 | Slot de fragmento que se ilumina al obtener el Fragmento de Aethon. |
| RF-42 | Ícono del Pulso de Calor con barra de cooldown, visible solo tras desbloquear la habilidad. |
| RF-43 | Menú principal con música y botones Nueva Partida, Continuar, Opciones, Créditos y Salir. Nueva Partida borra el progreso y carga Ignar directamente. |
| RF-44 | Pantalla de pausa con Continuar, Opciones (volumen y pantalla completa) y Menú Principal (tecla Escape). |
| RF-45 | Pantalla de Game Over con Reintentar desde el último checkpoint y Menú Principal. |
| RF-46 | Escena de "Fragmento de Aethon obtenido" al recoger el Fragmento, con texto en pantalla y desbloqueo del Pulso de Calor. |

### 2.8 Audio
| ID | Requerimiento |
|---|---|
| RF-47 | Música de Ignar: M02 "Forjas del Fin" (orquestal épico con percusión pesada). |
| RF-48 | Música del menú principal: M09. |
| RF-49 | Efectos: S03 burbujeo de lava, S04 golpe de golem, S05 chispa voladora, S24 checkpoint activado, S25 fragmento obtenido, S26 recoger cristal, S32/S33 explosiones, S35 dash, S37 game over y golpes/impactos de Kael. |

---

## 3. Controles (PC)

| Acción | Tecla |
|---|---|
| Moverse | A / D o flechas |
| Saltar / Doble salto | Espacio |
| Atacar (alterna puñetazo y patada) | J |
| Dash | K |
| Correr / Sprint | Shift izquierdo |
| Pulso de Calor (tras el Fragmento) | Q |
| Pausar | Escape |

Soporte opcional para gamepad Xbox / PlayStation.

---

## 4. Descripción del nivel y del jefe

### 4.1 Planeta 1 — Ignar
Planeta volcánico con forjas industriales abandonadas y una civilización congelada en el tiempo. Paleta naranja y roja con partículas de fuego. Scroll horizontal en exploración y vertical en la zona final.

| Zona | Contenido principal |
|---|---|
| **Zona 1 — Forjas Dormidas** | Entrada, charcos de lava, géiseres cada 3 s, palanca y compuerta, muros de roca (algunos como secreto posterior al jefe), Checkpoint 1, zona secreta accesible con dash o doble salto con Cristal de Luz grande y Cristal de Mejora, Golemitas y Chispas. |
| **Zona 2 — Canales de Magma** | Ríos de lava, lluvia de rocas, géiseres grandes, puentes que se derrumban, puzzle de 3 válvulas, Checkpoint 2, zona secreta tras la catarata de lava con Cristal de Luz grande y Cristal de Mejora. |
| **Zona 3 — El Núcleo** | Lago de lava con plataformas flotantes, explosiones de magma con aviso previo, Checkpoint 3 antes de la puerta del jefe, arena de Molrak. |

### 4.2 Molrak, el Golem Eterno
Golem gigante de roca y lava (3 veces la altura de Kael) con un núcleo brillante en el pecho. Guardián ancestral que protege el Fragmento.

| Fase | Ataques |
|---|---|
| Fase 1 | Embestida horizontal lenta y golpe de puño hacia abajo. |
| Fase 2 | Lluvia de rocas desde arriba y salto de impacto que sacude el suelo. |
| Fase 3 | Explosión de lava en área; el núcleo queda expuesto como punto débil. |

Al derrotarlo: cae el Fragmento 1 de Aethon; al recogerlo se desbloquea el Pulso de Calor, aparece el mensaje "Fragmento de Aethon obtenido", Ignar despierta visualmente y se abre el Umbral a Veluun.

> **Plan de contingencia:** si el tiempo no alcanza, reducir la vida de Molrak y simplificar la frecuencia de sus ataques, pero mantener las 3 fases para conservar la coherencia con el Documento de Diseño.

---

## 5. Requerimientos no funcionales

| ID | Requerimiento |
|---|---|
| RNF-01 | El juego corre en PC con Windows (y Mac) sin instalación adicional. |
| RNF-02 | Rendimiento estable de 60 FPS en una PC de gama media. |
| RNF-03 | Estilo visual pixel art de 16 bits, consistente en todos los sprites (Filter Mode Point, sin compresión). |
| RNF-04 | Respuesta inmediata a los controles (movimiento, salto y ataque sin retraso perceptible). |
| RNF-05 | Sesión de juego de Ignar de 10 a 20 minutos. |
| RNF-06 | Código organizado por responsabilidades (movimiento, combate, vida, enemigos, HUD, GameManager). |
| RNF-07 | Enemigos y obstáculos reutilizables como prefabs. |
| RNF-08 | Control de versiones con Git y GitHub (repositorio `Buscador-de-Luz`). |
| RNF-09 | Presupuesto de 0 dólares: Unity Personal y assets gratuitos o generados. |
| RNF-10 | La beta debe poder exportarse como ejecutable para Windows. |

---

## 6. Requerimientos técnicos

### 6.1 Software
| Herramienta | Uso |
|---|---|
| Unity 6 (URP) | Motor del juego |
| C# / Visual Studio o VS Code | Programación |
| Cinemachine | Cámara que sigue a Kael |
| 2D Tilemap + Tile Palette | Construcción de los niveles |
| Animator / Animation | Animaciones de personaje y enemigos |
| Canvas (UI) | HUD y menús |
| Git + GitHub | Control de versiones |
| Gemini | Generación de sprites y fondos |
| Aseprite / Piskel / Photopea | Edición y recorte de sprites |
| FMOD / Unity Audio Mixer | Audio |
| Microsoft Word y Excel | Documento de Diseño y Documentos de Producción (Huddle) |

### 6.2 Hardware
| Componente | Desarrollo | Mínimo para jugar |
|---|---|---|
| Sistema operativo | Windows 10 / 11 de 64 bits | Windows 10 de 64 bits |
| Procesador | Gama media (4 núcleos o más) | Doble núcleo a 2 GHz o superior |
| Memoria RAM | 16 GB recomendados | 4 GB |
| Gráficos | Compatible con DirectX 11 | Compatible con DirectX 11 |
| Almacenamiento | 10 GB libres para el proyecto | 500 MB libres para el build |
| Periféricos | Teclado y mouse | Teclado (gamepad opcional) |

### 6.3 Configuración del proyecto
- Plantilla: Universal 2D.
- Pixels Per Unit y Filter Mode Point en los sprites; Pivot en Bottom para los personajes.
- Layer `Suelo` para el piso; Tag `Player` en Kael; Tag `Lava` en el tilemap de lava.
- Tilemap de suelo con Tilemap Collider 2D y Composite Collider 2D (Rigidbody2D Static).
- Active Input Handling en `Both` (Unity 6).

### 6.4 Scripts principales
| Script | Función |
|---|---|
| `PlayerMovement` | Movimiento, salto, doble salto, sprint y animaciones |
| `PlayerCombat` | Ataque con J y detección de golpes |
| `PlayerHealth` | Vida, daño, muerte y Game Over |
| `PlayerDash` | Dash con K, invencibilidad y recarga |
| `PulsoDeCalor` | Habilidad Q, empuje y rotura de muros |
| `DañoContacto` | Daño por lava y obstáculos |
| `GameManager` | Checkpoint activo y respawn |
| `Checkpoint` | Activación y cambio de sprite del cristal |
| `GolemitaIA` / `GolemitaHealth` | Comportamiento y vida de la Golemita |
| `RastroLava` | Rastro de lava que daña y desaparece |
| `ChispaIA` / `ChispaHealth` | Comportamiento y vida de la Chispa; `SpawnChispas` crea grupos de 3 |
| `Geiser1` | Temporizador de 3 s y daño |
| `Palanca` / `Compuerta` | Mecánica de bloqueo |
| `MuroRompible` | Muro que solo destruye el Pulso de Calor |
| `PuzzleValvulas` / `Valvula` | Puzzle de 3 válvulas en orden |
| `PuenteDerrumbable` / `LluviaRocas` / `ExplosionMagma` | Trampas de las Zonas 2 y 3 |
| `MolrakIA` / `MolrakHealth` | Jefe de 3 fases, núcleo expuesto y drop del Fragmento |
| `FragmentoAethon` / `PanelFragmento` | Recogida, mensaje final y desbloqueo del Pulso |
| `ItemRecolectable` | Cristales de Luz y de Mejora |
| `HUDManager` | Actualización de corazones, fragmento y cooldown |
| `MainMenuManager`, `PauseManager`, `GameOverManager`, `OpcionesUI` | Pantallas y flujo del juego |
| `AudioManager` | Música y efectos por escena desde `Resources/Audio` |

---

## 7. Requerimientos de arte y audio (assets)

### 7.1 Sprites
| Elemento | Detalle |
|---|---|
| Kael | Idle, walk, jump, ataque 1 (puñetazo), ataque 2 (patada) |
| Golemita de Lava | Idle, walk, attack, hurt, death + rastro de lava |
| Chispa Voladora | Idle, fly, dive, hurt, death/explosión |
| Molrak | Idle, walk, ataques por fase, hurt, death |
| Tileset volcánico | Suelo, plataformas, paredes, lava |
| Fondo parallax | 3 capas por zona |
| Géiser | Base inactiva y animación de fuego |
| Palanca y compuerta | Inactiva/activa y cerrada/abierta |
| Muro rompible | Intacto y destruido |
| Cristal de Checkpoint | Inactivo y activo |
| Items | Cristal pequeño, grande, de mejora y Fragmento de Aethon |
| HUD | Corazón lleno y vacío, slot de fragmento, ícono de Pulso de Calor |

### 7.2 Imágenes de concepto para el Documento de Diseño
Kael, Molrak, Golemita, Chispa, Zonas 1 a 3, mapa general de Ignar, HUD, menú principal y escena Kael vs Molrak.

---

## 8. Planeación (Sprint Plan — Huddle)

### 8.1 Sprints
| Sprint | Fechas | Días | Meta |
|---|---|---|---|
| 1 | 7 – 20 sep | 14 | Setup del proyecto y mecánicas base de Kael |
| 2 | 21 sep – 4 oct | 14 | Zona 1 Forjas Dormidas completa |
| 3 | 5 – 18 oct | 14 | Zona 2 Canales de Magma y Zona 3 El Núcleo |
| 4 | 19 oct – 10 nov | 23 | Molrak, Pulso de Calor, trampas y polish |
| 5 | 11 – 19 nov | 9 | Nivel completo, sprites, audio, build, documentos y entrega |

### 8.2 Responsables y roles
Proyecto de una sola persona. La desarrolladora asume todos los roles:

| Rol | Responsabilidades |
|---|---|
| Game Designer | Documento de Diseño, mecánicas, niveles y balance |
| Project Manager | Sprint Plan, Feature Log, Backlogs y gráficas de seguimiento |
| Programadora | Scripts en C#, enemigos, jefe, HUD y menús |
| Artista | Sprites, tilesets, fondos e interfaz (generados con Gemini y editados) |
| Nivel Designer | Tilemaps, colocación de trampas, checkpoints e items |
| Tester | Pruebas Alfa por sprint, Buglist y pruebas del build |

### 8.3 Entregables
| Entregable | Formato |
|---|---|
| Videojuego Buscador de Luz | Build de Windows (`.exe`) |
| Documento de Diseño | Word |
| Documentos de Producción | Excel (Feature Log, Sprint Plan, Backlogs, gráficas y Buglist) |
| Presentación del proyecto | Página web proyectada |
| Código fuente | Repositorio de GitHub |

Artefactos de producción (Excel): Detalles del Proyecto, Sprint Plan, Feature Log, Project Chart, Sprint Backlog, Burn-down Chart, Task Chart, Buglist y Task Slips.

---

## 9. Estado actual del desarrollo

> **Nota:** marca como implementado solo lo que hayas visto funcionar en Play. Actualiza esta lista antes de entregar.

### Implementado
- Proyecto en Unity y repositorio en GitHub.
- Movimiento, salto, doble salto, sprint y animaciones.
- Dash con K (0.3 s de invencibilidad, recarga 1.5 s).
- Combate de Kael con la tecla J, alternando puñetazo y patada.
- Cámara que sigue a Kael y fondo parallax con límites.
- Tilemap de suelo y de lava con daño por contacto.
- Sistema de vida y HUD de 5 corazones (lleno, medio y vacío).
- Checkpoint con respawn, guardado en PlayerPrefs, Game Over y pausa con volumen.
- Menú principal con Nueva Partida, Continuar, Opciones, Créditos y Salir.
- Golemita de Lava (patrulla, persecución, ataque, rastro de lava, vida y drop del 20 %).
- Chispa Voladora (script, animaciones y grupos de 3).
- Molrak (IA de 3 fases, núcleo expuesto en Fase 3, vida y drop del Fragmento).
- Pulso de Calor, muros rompibles y panel de "Fragmento obtenido".
- Géiser, palanca, compuerta, puentes, válvulas, lluvia de rocas y explosiones de magma (código y prefabs).
- AudioManager automático con volumen guardado.
- Objetos de la escena `Ignar` que ya existen: Kael, cámara, HUD, pausa, Game Over, palanca, compuerta, géiser, spawn de chispas y 1 checkpoint.

### Pendiente
- Colocar en la escena los 2 checkpoints restantes, los cristales, las 2 zonas secretas, las trampas de Zona 2 y la arena de Molrak.
- Terminar el recorrido con tilemaps de las tres zonas y ajustar los límites de cámara.
- Asignar sprites a los prefabs y configurar las animaciones que falten.
- Copiar los clips de música y efectos a `Assets/Resources/Audio`.
- Generar el build de Windows y probarlo fuera del editor.
- Actualizar el Documento de Diseño (Word) y los Documentos de Producción (Excel) a la entrega del 19 de noviembre, e incluir las imágenes de concepto.
- Ajustar la presentación a la versión final del juego.

---

## 10. Criterios de aceptación (entrega del 19 de noviembre)

1. El juego se ejecuta desde un build de Windows sin errores de compilación.
2. Al abrirlo aparece el menú principal con música y los botones Nueva Partida, Continuar, Opciones, Créditos y Salir responden.
3. Nueva Partida entra directo al nivel de Ignar.
4. Kael se mueve con A/D, salta y hace doble salto con Espacio, ataca con J alternando puñetazo y patada, hace dash con K y corre con Shift.
5. Los 5 corazones del HUD reflejan la vida real, incluido el medio corazón.
6. La lava y los enemigos dañan a Kael; Kael puede derrotar a la Golemita y a la Chispa.
7. Al quedarse sin vida aparece Game Over; Reintentar reaparece en el último checkpoint con la vida completa.
8. Los 3 Cristales de Checkpoint (uno por zona) se iluminan al tocarlos.
9. El nivel de Ignar es recorrible de inicio a fin por las tres zonas, con parallax y sin mostrar el vacío.
10. Se resuelven la palanca, el puzzle de 3 válvulas y las trampas de las Zonas 2 y 3.
11. Molrak tiene 3 fases, muere y suelta el Fragmento; al recogerlo aparece "Fragmento de Aethon obtenido" y se desbloquea el Pulso de Calor con Q.
12. El HUD muestra los corazones, el slot del fragmento iluminado y el ícono del Pulso con su barra de recarga.
13. Escape abre la pausa y el volumen se puede ajustar.
14. Hay música de menú e Ignar y efectos para cristales, checkpoints, dash, golpes, explosiones y game over.
15. Una partida completa dura entre 10 y 20 minutos.
16. Documento de Diseño (Word), Documentos de Producción (Excel) y presentación entregados y coherentes con el juego.