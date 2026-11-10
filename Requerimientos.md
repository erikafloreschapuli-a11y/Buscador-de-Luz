# Buscador de Luz — Documento de Requerimientos

**Estudio:** Eclipsis Games
**Título anterior:** Fragmentos del Cosmos
**Género:** Plataformero 2D de acción y exploración
**Plataforma:** PC (Windows / Mac)
**Motor:** Unity 6 con Universal Render Pipeline (URP)
**Lenguaje:** C#
**Equipo:** 1 persona
**Periodo de producción:** 7 de septiembre de 2026 al 30 de octubre de 2026
**Entregable:** Beta jugable del Planeta 1 — Ignar (30 de octubre de 2026)
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
- Menú principal, pausa y pantalla de Game Over.
- HUD con vida, fragmento y habilidad.

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
| RF-08 | Al morir, reaparecer en el último checkpoint activado con la vida restaurada. |
| RF-09 | Dash con 0.3 s de invencibilidad y cooldown de 1.5 s *(definido en el GDD, pendiente de integrar al script actual)*. |
| RF-10 | Sprint al mantener un botón *(definido en el GDD, pendiente de integrar al script actual)*. |
| RF-11 | Pulso de Calor: onda de fuego que empuja enemigos y rompe muros de roca; se desbloquea al derrotar a Molrak. |

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
| RF-33 | Explosiones de magma aleatorias en el suelo (Zona 3). |
| RF-34 | Puerta del jefe que se cierra al entrar al combate. |

### 2.6 Items
| ID | Item | Efecto | Dónde aparece |
|---|---|---|---|
| RF-35 | Cristal de Luz pequeño | Recupera medio corazón | Drop de enemigos (20 %) |
| RF-36 | Cristal de Luz grande | Recupera 1 corazón | Zonas secretas |
| RF-37 | Cristal de Mejora | Reduce el cooldown de habilidades 0.5 s | Zonas secretas |
| RF-38 | Fragmento de Aethon | Objetivo del nivel; completa Ignar | Lo suelta Molrak |
| RF-39 | Cristal de Checkpoint | Guarda la posición | 3 fijos en el nivel (no se recoge) |

### 2.7 Interfaz (HUD y pantallas)
| ID | Requerimiento |
|---|---|
| RF-40 | HUD con 5 corazones que reflejan la vida actual de Kael. |
| RF-41 | Slot de fragmento que se ilumina al obtener el Fragmento de Aethon. |
| RF-42 | Ícono del Pulso de Calor con barra de cooldown, visible solo tras desbloquear la habilidad. |
| RF-43 | Menú principal con Nueva Partida, Continuar, Opciones y Salir. |
| RF-44 | Pantalla de pausa con Continuar, Opciones y Menú Principal (tecla Escape). |
| RF-45 | Pantalla de Game Over con Reintentar desde el último checkpoint y Menú Principal. |
| RF-46 | Escena de "Fragmento obtenido" al derrotar a Molrak, con texto en pantalla. |

### 2.8 Audio
| ID | Requerimiento |
|---|---|
| RF-47 | Música de Ignar: M02 "Forjas del Fin" (orquestal épico con percusión pesada). |
| RF-48 | Música del menú principal: M09. |
| RF-49 | Efectos: S03 burbujeo de lava, S04 golpe de golem, S05 chispa voladora, S24 cristal activado, S25 fragmento obtenido. |

---

## 3. Controles (PC)

| Acción | Tecla |
|---|---|
| Moverse | A / D o flechas |
| Saltar / Doble salto | Espacio |
| Atacar | J |
| Dash | Por definir (en el GDD) |
| Pausar | Escape |

Soporte opcional para gamepad Xbox / PlayStation.

---

## 4. Descripción del nivel y del jefe

### 4.1 Planeta 1 — Ignar
Planeta volcánico con forjas industriales abandonadas y una civilización congelada en el tiempo. Paleta naranja y roja con partículas de fuego. Scroll horizontal en exploración y vertical en la zona final.

| Zona | Contenido principal |
|---|---|
| **Zona 1 — Forjas Dormidas** | Entrada, charcos de lava, géiseres cada 3 s, palanca y compuerta, muros de roca, Checkpoint 1, zona secreta con Cristal de Mejora, Golemitas y Chispas. |
| **Zona 2 — Canales de Magma** | Ríos de lava, lluvia de rocas, géiseres grandes, puentes que se derrumban, puzzle de 3 válvulas, Checkpoint 2, zona secreta con Cristal de Luz grande. |
| **Zona 3 — El Núcleo** | Lago de lava con plataformas flotantes, explosiones de magma, Checkpoint 3 antes de la puerta del jefe, arena de Molrak. |

### 4.2 Molrak, el Golem Eterno
Golem gigante de roca y lava (3 veces la altura de Kael) con un núcleo brillante en el pecho. Guardián ancestral que protege el Fragmento.

| Fase | Ataques |
|---|---|
| Fase 1 | Embestida horizontal lenta y golpe de puño hacia abajo. |
| Fase 2 | Lluvia de rocas desde arriba y salto de impacto que sacude el suelo. |
| Fase 3 | Explosión de lava en área; el núcleo queda expuesto como punto débil. |

Al derrotarlo: cae el Fragmento 1 de Aethon, se desbloquea el Pulso de Calor, Ignar despierta visualmente y se abre el Umbral a Veluun.

> **Plan de contingencia:** si el tiempo no alcanza, implementar Molrak solo con la Fase 1 y dejar las fases 2 y 3 como mejora posterior.

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
| RNF-08 | Control de versiones con Git y GitHub (repositorio `buscador-de-luz`). |
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

### 6.2 Configuración del proyecto
- Plantilla: Universal 2D.
- Pixels Per Unit y Filter Mode Point en los sprites; Pivot en Bottom para los personajes.
- Layer `Suelo` para el piso; Tag `Player` en Kael; Tag `Lava` en el tilemap de lava.
- Tilemap de suelo con Tilemap Collider 2D y Composite Collider 2D (Rigidbody2D Static).
- Active Input Handling en `Both` (Unity 6).

### 6.3 Scripts principales
| Script | Función |
|---|---|
| `PlayerMovement` | Movimiento, salto, doble salto y animaciones |
| `PlayerCombat` | Ataque con J y detección de golpes |
| `PlayerHealth` | Vida, daño, muerte y respawn |
| `KaelDaño` / `DañoContacto` | Daño por lava y obstáculos |
| `GameManager` | Checkpoint activo y respawn |
| `Checkpoint` | Activación y cambio de sprite del cristal |
| `GolemitaIA` / `GolemitaHealth` | Comportamiento y vida de la Golemita |
| `RastroLava` | Rastro de lava que daña y desaparece |
| `ChispaIA` / `ChispaHealth` | Comportamiento y vida de la Chispa |
| `Geiser` | Temporizador de 3 s y daño |
| `Palanca` / `Compuerta` | Mecánica de bloqueo |
| `HUDManager` | Actualización de corazones y fragmento |
| `GameOverManager`, `MainMenu`, `PauseMenu` | Pantallas y flujo del juego |

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

| Sprint | Fechas | Días | Meta |
|---|---|---|---|
| 1 | 7 – 20 sep | 14 | Setup del proyecto y mecánicas base de Kael |
| 2 | 21 sep – 4 oct | 14 | Zona 1 Forjas Dormidas completa |
| 3 | 5 – 18 oct | 14 | Zona 2 Canales de Magma y Zona 3 El Núcleo |
| 4 | 19 – 30 oct | 12 | Molrak, Pulso de Calor, polish y beta jugable |

Artefactos de producción (Excel): Detalles del Proyecto, Sprint Plan, Feature Log, Project Chart, Sprint Backlog, Burn-down Chart, Task Chart, Buglist y Task Slips.

---

## 9. Estado actual del desarrollo

### Implementado
- Proyecto en Unity y repositorio en GitHub.
- Movimiento, salto y doble salto con animaciones.
- Cámara que sigue a Kael y fondo parallax.
- Tilemap de suelo y de lava con daño por contacto.
- Sistema de vida y HUD de 5 corazones.
- Checkpoint con respawn.
- Golemita de Lava (patrulla, persecución, ataque, rastro de lava, vida y drop).
- Chispa Voladora (script y animaciones; en pruebas).
- Combate de Kael con la tecla J.
- Paneles de Game Over y Pausa creados en el Canvas.

### Pendiente
- Game Over y menú principal completamente funcionales.
- Fragmento de Aethon y slot del HUD.
- Molrak (mínimo Fase 1).
- Pulso de Calor.
- Géiseres, palanca y compuerta, zona secreta y muros rompibles.
- Dash y sprint.
- Audio.
- Build para Windows.
- Documento de Diseño (Word) con imágenes de concepto y Documentos de Producción (Excel) con gráficas.

---

## 10. Criterios de aceptación de la beta (30 de octubre)

1. El juego se ejecuta desde un build de Windows sin errores de compilación.
2. Kael se mueve, salta, ataca y recibe daño correctamente.
3. Los 5 corazones del HUD reflejan la vida real.
4. La lava y los enemigos dañan a Kael; Kael puede derrotar a la Golemita y a la Chispa.
5. Al morir, Kael reaparece en el último checkpoint; si pierde toda la vida aparece Game Over.
6. El nivel de Ignar es recorrible de inicio a fin.
7. Existe un final jugable: recoger el Fragmento de Aethon (al derrotar a Molrak o al llegar al final de la zona).
8. Menú principal accesible al abrir el juego.
9. Documento de Diseño y Documentos de Producción entregados y coherentes con el juego.
10. Presentación de 5 minutos lista para proyectar.