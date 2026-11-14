# Guía de editor — Buscador de Luz (beta de Ignar)

**Qué es este documento:** la lista exacta de lo que falta hacer **dentro del editor de Unity**.
Todo el código, los prefabs y el cableado de escenas ya están hechos y compilan sin errores.

**Entrega:** 20 de noviembre de 2026.

**Tiempo estimado:** 6–8 horas repartidas. Si sigues los pasos en orden, al final tienes el nivel
jugable de principio a fin, con menús, audio y build de Windows.

**Versión:** Unity 6.5 (6000.5.8f1) · URP 2D · Input System + Input Manager (ambos activos)

---

## 0. Resumen en 1 minuto

| # | Paso | Tiempo |
|---|---|---|
| 1 | Abrir el proyecto y comprobar que la consola está limpia | 10 min |
| 2 | Asignar sprites a los prefabs (`Assets/Prefabs`) | 30–45 min |
| 3 | Colocar 3 checkpoints | 10 min |
| 4 | Colocar cristales y las 2 zonas secretas | 20 min |
| 5 | Colocar trampas de Zona 2 (puentes, válvulas, rocas, magma) | 30 min |
| 6 | Montar la arena de Molrak + puerta + zona de combate | 30 min |
| 7 | Terminar el recorrido con tilemaps (Zonas 1, 2 y 3) | 2–4 h |
| 8 | Meter música y SFX | 30 min |
| 9 | Cableado opcional (HUD, pausa, menú) | 15 min |
| 10 | Build de Windows y prueba completa | 45 min |

> **Lo que NO tienes que hacer:** crear scripts, prefabs, botones, paneles ni enlazar el
> `AudioManager`. Todo eso se genera solo al pulsar Play.

---

## 1. Controles del juego (para probar)

| Acción | Tecla |
|---|---|
| Mover | `A` / `D` o flechas |
| Saltar (doble salto) | `Espacio` |
| Atacar | `J` |
| Dash (0,3 s invencible, recarga 1,5 s) | `K` |
| Sprint | `Shift izquierdo` |
| Pulso de Calor | `Q` (solo tras obtener el Fragmento) |
| Pausa | `Escape` |

---

## 2. Qué ya está hecho (no lo toques)

- **46 scripts** en `Assets/Scripts`, todos en español y con comentarios.
- **14 prefabs** en `Assets/Prefabs`: cristales, fragmento, roca, muro, puerta, puente,
  válvula, Molrak, Checkpoint, Golemita, Chispa y rastro de lava.
- **Escena `Ignar`** con Kael, cámara, HUD de 5 corazones, pausa, Game Over, palanca,
  compuerta, géiser, spawn de chispas y 1 checkpoint.
- **Escena `MainMenu`** con todos los botones cableados (Nueva partida, Continuar,
  Opciones, Créditos, Mejoras, Mapa, Salir).
- **Build Settings** con `MainMenu` e `Ignar` (Zona2 está fuera del build a propósito).
- **Se genera solo al pulsar Play** (no cablear nada):
  - `AudioManager` + música de la escena desde `Assets/Resources/Audio/Musica`.
  - `PlayerDash` y `PulsoDeCalor` en Kael.
  - Slot del Fragmento, icono del Pulso y barra de recarga del HUD.
  - Panel de Opciones (pausa y menú), Créditos, Mejoras y Mapa.
  - Pantalla de "Fragmento obtenido".
  - Los 3 puntos de spawn del grupo de Chispas.
  - La `ZonaCombateJefe` encuentra sola a Molrak y a la puerta.
  - El botón "OPCIONES" dentro de la pausa.

---

## 3. Antes de tocar nada (10 min)

1. Abre el proyecto con Unity 6. Espera a que termine la importación (barra abajo a la derecha).
2. Si aparece el aviso **"Import TMP Essentials"**, acéptalo.
   (Ventana: `Window > TextMeshPro > Import TMP Essential Resources`.)
3. Abre `Assets/Scenes/MainMenu.unity` y mira la **Console** (`Window > General > Console`).
   - Debe estar vacía o solo con avisos amarillos. Si hay **errores rojos**, mándamelos.
4. Abre `Assets/Scenes/Ignar.unity`, pulsa **Play** y comprueba:
   - Kael se mueve y salta.
   - `Escape` abre la pausa y aparece el botón **OPCIONES**.
   - Los botones del menú (si sales con Escape > Menú principal) responden.
5. Para pixel art (recomendado, RNF-03): selecciona cada PNG en `Assets/Imagenes` y en el
   Inspector pon **Filter Mode: Point (no filter)** y **Compression: None**.
   Los sprites de Kael ya están configurados.

> **Importante:** guarda la escena (`Ctrl+S`) antes de cada prueba. Unity no guarda solo.

---

## 4. Paso 1 — Sprites de los prefabs (30–45 min)

Abre cada prefab con **doble clic** en `Assets/Prefabs`. Asigna el sprite en el
**Sprite Renderer** y, cuando el script tenga campos de sprite, también en esos campos.
Usa la hoja `Assets/Imagenes/Ignar_Objetos_Individuales.png`: despliégala en el Project y
arrastra el sprite concreto (los nombres son `Ignar_Objetos_Individuales_0` … `_133`, así que
elige a ojo el que corresponda). Ajusta el **BoxCollider2D** con **Edit Collider** para que
encaje con el dibujo.

| Prefab | SpriteRenderer | Campos del script | Collider / escala |
|---|---|---|---|
| `CristalPequeno` | cristal pequeño azul/blanco | `tipo: CristalPequeno`, cura 0,5 | Trigger 0,5×0,5 (ajusta al arte) |
| `CristalGrande` | cristal grande | `tipo: CristalGrande`, cura 1 | Trigger 0,8×0,8 |
| `CristalMejora` | cristal distinto (naranja/rojo) | `tipo: CristalMejora`, `reduccionCooldown: 0,5` | Trigger 0,7×0,7 |
| `FragmentoAethon` | **ya tiene sprite** (no cambiar) | — | Trigger 1,2×1,2 |
| `RocaVolcanica` | roca pequeña | `daño: 1`, `tiempoEntreDaño: 1` | Trigger 0,6×0,6 |
| `MuroRompible` | roca/pared | `spriteIntacto` y `spriteRoto` | Sólido 1×2 (estíralo con la escala) |
| `PuertaJefe` | puerta metálica | `spriteCerrada` y `spriteAbierta` | Sólido 1×4 |
| `PuenteDerrumbable` | tablones/piedra | — | Sólido 3×0,5 (estíralo a lo ancho del hueco) |
| `Valvula` | válvula | `spriteInactiva` y `spriteActiva`, `indice` | Trigger 0,8×0,8 |
| `Molrak` | ver nota de abajo | — | Sólido 2,5×5 |
| `Checkpoint` | **ya tiene sprite** (cristal) | — | Trigger 1,6×3 |
| `Gomelita` / `ChispaVoladora` / `RastroLava` | ya tienen sprite | — | Ya ajustados |

**Notas:**
- **Hojas de sprites disponibles** en `Assets/Imagenes/`:
  - `Ignar_Objetos_Individuales.png`: 134 objetos sueltos (cristales, rocas, puertas, válvulas).
  - `Purple Crystal Sprite Sheet_ Inactive and Active States.png`: cristal con estados
    **inactivo/activo**, ideal para `Checkpoint` y `Valvula`.
  - `Picture6.png`: los dos cristales de checkpoint que ya usa la escena.
  - `Lava Rock Pixel Art Tileset(1).png` y `Volcanic Lava Platformer Tileset.png`: tiles.
- **Molrak:** si no tienes arte, duplica la Golemita, escálala ×2 y ponle un tinte rojo
  (`Color` del Sprite Renderer). El script `MolrakIA` no necesita Animator (es null-safe).
- **MuroRompible:** el script pone el sprite intacto al empezar, así que el Sprite Renderer
  puede quedarse vacío; aun así asígnalo para verlo en el editor.
- **Valvula y PuertaJefe:** igual, el script pone el sprite inicial desde sus campos.
- Los prefabs de items **sí necesitan** el Sprite Renderer asignado (si no, son invisibles).
- No cambies los valores de `tipo`, `cantidadCuracion`, `daño` ni `vidaMaxima`: ya están
  configurados según los requerimientos.

---

## 5. Paso 2 — Los 3 checkpoints (10 min)

RF-17 pide uno por zona. En `Ignar.unity` ya hay uno llamado **`Christal`** en `x ≈ 201`.
Añade dos más arrastrando `Assets/Prefabs/Checkpoint.prefab` a la escena:

| Checkpoint | Dónde | Coordenada orientativa |
|---|---|---|
| `Checkpoint_Zona1` | Al inicio, junto al spawn de Kael | `x ≈ 0`, sobre el suelo |
| `Christal` (ya existe) | Entrada de los Canales de Magma | `x ≈ 201` |
| `Checkpoint_Zona3` | Antes de la arena de Molrak | `x ≈ 330` |

- Colócalos **sobre una plataforma con collider** y a la altura del suelo (el prefab ya
  respawnea a Kael 1,2 unidades por encima).
- Ajusta `offsetRespawn` si Kael aparece dentro del suelo o flotando.
- Al tocarlo se ilumina, guarda la posición en `PlayerPrefs` y suena (si asignas
  `sonidoActivar`).
- **Prueba:** muérete en lava después de tocar uno y pulsa **REINTENTAR** en el Game Over.

---

## 6. Paso 3 — Cristales y zonas secretas (20 min)

### 6.1 Cristales

Reparte por el nivel arrastrando los prefabs:

- **6–10 `CristalPequeno`** (cura medio corazón) por Zonas 1 y 2, en cornisas y saltos.
- **2–3 `CristalGrande`** (cura un corazón entero): al menos uno dentro de cada zona secreta
  y otro antes de la arena del jefe.
- **2 `CristalMejora`** (reduce 0,5 s la recarga): uno en cada zona secreta.

Colócalos a media altura (`y` del suelo + 1) para que se vean.

### 6.2 Zona secreta 1 — Zona 1 (pared falsa)

**Opción A (recomendada, accesible en la primera partida):**
1. En una pared del tilemap deja un hueco de 2×3 tiles, con entrada a la altura del suelo.
2. Tapa la entrada con **tiles decorativos sin collider** (pared falsa visual) o deja un hueco
   al que se llegue con **dash (K)** o doble salto.
3. Detrás, pon un pasillo de 3–4 tiles con un `CristalGrande` y un `CristalMejora`.

**Opción B (secreto posterior al jefe):**
1. Deja otro hueco de 2×3 tiles y tápalo con `MuroRompible` (sólido).
2. Detrás, coloca un `CristalGrande` como recompensa por volver con el Pulso de Calor.

> `MuroRompible` **solo** se rompe con el **Pulso de Calor (Q)**, que se desbloquea al final de
> Ignar. No uses un muro rompible como única entrada de la zona secreta 1: en la primera partida
> sería inaccesible. La Opción A garantiza que el jugador pueda conseguir el Cristal de Mejora
> durante el recorrido normal.

### 6.3 Zona secreta 2 — Zona 2 (catarata de lava)

1. Coloca una cascada de lava decorativa **sin daño** (sprite o tilemap decorativo que no esté
   en la capa `lava` ni lleve el tag `Lava`).
2. Detrás, deja un pasillo con un `CristalGrande` y un `CristalMejora`.
3. No cierres la entrada con `MuroRompible`: al igual que en la Zona 1, quedaría inaccesible
   hasta después del jefe. El paso debe quedar libre detrás de la catarata.

**Prueba:** recoge un cristal con la vida a la mitad y comprueba que el HUD dibuja el
**medio corazón** (sprite `corazonMedio` ya cableado en el HUD).

---

## 7. Paso 4 — Trampas de Zona 2 (30 min)

### 7.1 Puentes derrumbables (RF-30)

- Arrastra `PuenteDerrumbable.prefab` sobre cada hueco de lava (3–4 puentes).
- Estira la **escala X** para cubrir el hueco; el collider es sólido para poder caminar.
- Se hunde al pisarlo y reaparece a los 6 s (`reaparece: 1`, `tiempoReaparicion: 6`).
- Opcional: asigna `sonidoCrujido` y `sonidoCaida`.

### 7.2 Puzzle de 3 válvulas (RF-31)

1. Coloca **3 `Valvula.prefab`** cerca de una compuerta.
   - En cada una pon `indice` = `0`, `1` y `2` (¡distintos!).
2. Crea un GameObject vacío llamado **`PuzzleValvulas`** en el centro de la sala y
   añádele el componente `PuzzleValvulas`.
3. En el Inspector del puzzle:
   - `Orden Correcto`: `0, 1, 2` (el orden en que hay que activarlas).
   - `Valvulas`: arrastra las 3 válvulas.
   - `Compuerta`: arrastra la compuerta que abre el paso (la de la escena o una nueva).
4. En cada válvula, arrastra el `PuzzleValvulas` al campo `puzzle`.

> Si el jugador se equivoca, el puzzle se reinicia solo y suena `sonidoError`.
> Al resolverlo se abre la compuerta y suena `sonidoResuelto`.

### 7.3 Lluvia de rocas (RF-32)

1. Crea un vacío llamado **`LluviaRocas_Zona2`** sobre la zona de paso.
2. Añade el componente `LluviaRocas`.
3. Asigna:
   - `Roca Prefab`: `Assets/Prefabs/RocaVolcanica.prefab`
   - `Intervalo`: 2,5 · `Rocas Por Rafaga`: 1 · `Ancho Area`: 12 · `Altura Aparicion`: 8
4. Colócalo a la altura del techo de la sala. Las rocas caen y dañan 1 corazón.

> El gizmo naranja (con el objeto seleccionado) muestra el área. Ajusta `Ancho Area` a la sala.

### 7.4 Explosiones de magma (RF-33)

1. Crea un vacío llamado **`Explosiones_Zona3`** en el suelo de la zona final.
2. Añade `ExplosionMagma` y ajusta:
   - `Intervalo`: 2,5 · `Variacion Intervalo`: 1 · `Tiempo Aviso`: 0,8
   - `Ancho Area`: 12 · `Radio Daño`: 1,2 · `Daño`: 1
3. Opcional: asigna `avisoPrefab` / `explosionPrefab` (partículas) y los sonidos.

---

## 8. Paso 5 — Arena de Molrak y final (30 min)

1. **Arena:** prepara una plataforma amplia (≈ 30×12 unidades) en la Zona 3, con paredes
   altas para que Kael no se caiga.
2. **Molrak:** arrastra `Assets/Prefabs/Molrak.prefab` al centro de la arena.
   - Comprueba en el prefab que `rocaPrefab` apunta a `RocaVolcanica` y `fragmentoPrefab`
     a `FragmentoAethon` (ya están enlazados).
   - `vidaMaxima: 24`. Para probar rápido, bájalo a 6 y vuelve a subirlo antes de entregar.
3. **Puerta:** arrastra `PuertaJefe.prefab` a la **entrada** de la arena.
   - Empieza **abierta** (`empiezaCerrada: 0`). La zona de combate la cierra al entrar.
   - Oriéntala/escálala para tapar el hueco de entrada.
4. **Zona de combate:**
   - Crea un vacío llamado **`ZonaCombateJefe`** en el centro de la arena.
   - Añade `ZonaCombateJefe`. El script añade y marca como **trigger** el collider, pero
     necesitas un `BoxCollider2D` que cubra la arena (el `RequireComponent` lo añade solo;
     ajusta el tamaño con **Edit Collider**).
   - Puedes dejar `puerta` y `jefe` vacíos: se buscan solos en la escena.
   - Opcional: `musicaCombate` + `fuenteMusica` (si no, usa el `AudioManager`).
5. **Comprobación:** al entrar en la arena se cierra la puerta y Molrak despierta. Al morir
   Molrak suelta el Fragmento, la puerta se abre y al recogerlo aparece la pantalla
   **"FRAGMENTO DE AETHON OBTENIDO"** y se desbloquea el Pulso de Calor.

> **Truco de prueba:** pon `MolrakHealth.vidaMaxima = 6` y, si quieres probar el Pulso antes,
> en el componente `PulsoDeCalor` de Kael (se añade al pulsar Play) desmarca
> `requiereDesbloqueo`.

---

## 9. Paso 6 — Terminar el recorrido con tilemaps (2–4 h)

El nivel actual va de `x ≈ -20` a `x ≈ 520`. Referencias que ya existen en la escena:

| Objeto | Posición | Para qué sirve |
|---|---|---|
| Kael (spawn) | `x ≈ -18` | Inicio |
| SpawnadorChispas | `x ≈ 52` | Grupo de 3 chispas |
| Geiser | `x ≈ 215` | Géiser de lava |
| Christal (checkpoint) | `x ≈ 201` | Entrada de Zona 2 |
| Compuerta | `x ≈ 469` | Compuerta de palanca |
| Lava_Tilemap | todo el ancho | Río de lava |

**Reglas para los tilemaps nuevos:**
- Cualquier tilemap por el que se camine debe estar en la capa **`Suelo` (6)**: el
  `PlayerMovement` usa esa capa para el doble salto (`CapaSuelo`).
- La lava debe estar en la capa **`lava` (8)** y con el tag `Lava`.
- Usa **Composite Collider 2D** en el tilemap de suelo para que Kael no se enganche.
- Los pinchos, rocas y lava ya dañan por script; no necesitan código.

**Reparto sugerido:**
- **Zona 1 (Forjas Dormidas):** `x -20 → 150`. Plataformas, géiseres, palanca + compuerta
  (ya existe una), primera zona secreta.
- **Zona 2 (Canales de Magma):** `x 150 → 330`. Puentes derrumbables, lluvia de rocas,
  puzzle de válvulas y la catarata secreta.
- **Zona 3 (El Núcleo):** `x 330 → 520`. Lago de lava con plataformas flotantes,
  explosiones de magma y la arena de Molrak al final.

**Cámara:** la `CinemachineCamera` ya sigue a Kael y `LimitesCamara` limita el encuadre.
Si el nivel crece, estira el `PolygonCollider2D` de `LimitesCamara` para que la cámara no
enseñe el vacío.

---

## 10. Paso 7 — Audio (30 min)

### 10.1 Música (automática)

Copia los archivos en `Assets/Resources/Audio/Musica/` con estos nombres exactos:

```
Assets/Resources/Audio/Musica/MainMenu.ogg   -> menú
Assets/Resources/Audio/Musica/Ignar.ogg      -> nivel
Assets/Resources/Audio/Musica/Tema.ogg       -> (opcional) reserva
```

El `AudioManager` se crea solo y carga el tema de la escena actual. No hay que arrastrar nada.
El volumen se guarda en `PlayerPrefs` desde el panel de Opciones.

> **Prioridad:** el criterio pide que el **menú principal abra con música**. Aunque el audio
> general es opcional, `MainMenu.ogg` e `Ignar.ogg` son los dos archivos que conviene tener sí o sí.

### 10.2 SFX (a mano, opcional)

Copia los efectos a `Assets/Resources/Audio/SFX/` y arrástralos a estos campos:

| Script / prefab | Campo | Sonido sugerido |
|---|---|---|
| `CristalPequeno`, `CristalGrande`, `CristalMejora` | `sonidoRecoger` | S26_Cristal |
| `Checkpoint` (prefab y `Christal`) | `sonidoActivar` | S24_Checkpoint |
| `MuroRompible` | `sonidoRotura` | S27_Muro |
| `Valvula` | `sonidoActivar` | S28_Valvula |
| `PuzzleValvulas` | `sonidoCorrecto`, `sonidoError`, `sonidoResuelto` | S29 / S30 |
| `PuenteDerrumbable` | `sonidoCrujido`, `sonidoCaida` | S31_Puente |
| `PuertaJefe` | `sonidoAbrir`, `sonidoCerrar` | S34_Puerta |
| `ExplosionMagma` | `sonidoAviso`, `sonidoExplosion` | S33 / S32 |
| `MolrakHealth` | `sonidoMuerte` | S38_Molrak |
| `PlayerDash` (en Kael) | `sonidoDash` | S35_Dash |
| `PulsoDeCalor` (en Kael) | `sonidoPulso` | S36_Pulso |
| `GameOverManager` | `sonidoGameOver` | S37_GameOver |
| `PanelFragmento` | `sonidoFragmento` | S25_Fragmento |
| `PlayerCombat` (en Kael) | `sonidoAtaque`, `sonidoImpacto` | golpes |

Los nombres de archivo son solo una convención: en `Assets/Resources/Audio/SFX/LEEME.txt`
tienes la lista completa. Todos los campos son opcionales; el juego funciona sin sonido.

---

## 11. Paso 8 — Cableado opcional (15 min)

Todo esto se genera solo, pero si quieres usar tu propio arte, asígnalo en el Inspector:

### HUD (`HUDManager`, en el objeto `HUDManager`)
- `Slot Fragmento`: una `Image` en el HUD (se crea sola si está vacía).
- `Fragmento Apagado` / `Fragmento Encendido`: sprites del slot.
- `Icono Habilidad`: objeto del icono del Pulso (se crea solo).
- `Barra Cooldown`: `Image` con **Image Type: Filled** (se crea sola).
- `Corazon Medio`: ya está cableado con el sprite de medio corazón.

### Pausa (`PauseManager`, en el objeto `Pausa`)
- `Panel Opciones`: panel de opciones (se crea solo).
- `Paneles Que Bloquean`: ya tiene el `PanelGameOver`.

### Menú (`MainMenuManager`, en `MainMenu.unity`)
- `Panel Principal`, `Panel Opciones`, `Panel Créditos`, `Panel Mejoras`, `Panel Mapa`:
  si los dejas vacíos se crean con textos provisionales.
- **Créditos:** busca el texto `[TU NOMBRE]` y `[CRÉDITOS]` en el panel generado y
  cámbialo por los tuyos (o crea el panel a mano y asígnalo).
- `Boton Continuar` ya está cableado.

### Game Over (`GameOverManager`)
- `Panel Game Over` ya está cableado.
- Opcional: `audioSource` + `sonidoGameOver`.

### Panel del Fragmento
- Se crea solo en la escena de juego. Si quieres personalizarlo, crea un vacío
  `PanelFragmento` con el componente y edita `mensaje` / `submensaje` / tiempos.

---

## 12. Paso 9 — Build de Windows (15 min)

1. `File > Build Profiles` (en Unity 6) o `File > Build Settings`.
2. Plataforma: **Windows** · Arquitectura: **x86_64**.
3. En **Scene List** deben estar, en este orden y marcadas:
   - `Assets/Scenes/MainMenu.unity`
   - `Assets/Scenes/Ignar.unity`
   - `Zona2.unity` **no** debe estar marcada.
4. `Player Settings`:
   - **Product Name:** `Buscador de Luz`
   - **Company Name:** tu nombre o estudio
   - **Default Screen Width/Height:** 1920 × 1080
   - **Fullscreen Mode:** Fullscreen Window (o Exclusive Fullscreen)
   - **Color Space:** Linear (ya está)
5. `Build` → crea una carpeta `Builds/Windows/` y genera el `.exe`.
6. Abre el `.exe` y prueba el juego **fuera del editor**.

---

## 13. Paso 10 — Prueba completa (30 min)

Haz una partida entera y marca cada punto:

- [ ] 1. El menú abre con música y responden Nueva Partida, Continuar, Opciones, **Créditos** y Salir.
- [ ] 2. **Nueva partida** entra a Ignar; **Continuar** aparece tras jugar una vez.
- [ ] 3. Kael se mueve, salta dos veces, ataca con `J`, hace dash con `K` y sprint con `Shift`.
- [ ] 4. Los 5 corazones bajan y suben (cristales curan medio corazón y un corazón).
- [ ] 5. La lava y los enemigos dañan; Kael derrota Golemitas y Chispas (grupos de 3).
- [ ] 6. Al morir aparece **Game Over**; **REINTENTAR** respawnea en el último checkpoint.
- [ ] 7. Los 3 checkpoints se iluminan al tocarlos.
- [ ] 8. Las dos zonas secretas dan un Cristal de Luz grande y un Cristal de Mejora.
- [ ] 9. Los puentes caen, el puzzle de válvulas abre el paso y las rocas caen.
- [ ] 10. Las explosiones de magma avisan antes de dañar.
- [ ] 11. La puerta del jefe se cierra al entrar en la arena.
- [ ] 12. Molrak pasa por sus **3 fases**, muere, suelta el Fragmento y se ve el mensaje final.
- [ ] 13. El slot del fragmento se ilumina y el ícono del Pulso aparece con su barra de recarga.
- [ ] 14. Tras el Fragmento, `Q` lanza el Pulso de Calor y rompe un muro.
- [ ] 15. `Escape` pausa; el volumen y la pantalla completa se guardan.
- [ ] 16. Créditos, Mejoras y Mapa muestran información.
- [ ] 17. La partida completa dura entre 10 y 20 minutos.

**Si algo falla:** apunta qué paso, qué esperabas y qué pasó, y mándame el error de la
Console. Con eso lo corrijo en el código sin que tengas que tocar nada más.

---

## 14. Problemas frecuentes

| Síntoma | Causa / solución |
|---|---|
| Los scripts no aparecen en el menú **Add Component** | Unity aún compila. Espera y revisa la Console. |
| Sprites en rosa/magenta | El material del Sprite Renderer debe ser `Sprite-Lit-Default` (URP 2D). |
| Los botones no responden | Debe haber un `EventSystem` con `InputSystemUIInputModule` (ya está en las escenas). |
| El texto TMP sale en blanco o con cuadros | `Window > TextMeshPro > Import TMP Essential Resources`. |
| No suena la música | El archivo debe estar en `Assets/Resources/Audio/Musica` y llamarse igual que la escena (`MainMenu.ogg`, `Ignar.ogg`). |
| Kael no salta dos veces | El tilemap de suelo no está en la capa `Suelo` (6). |
| Kael se cae del nivel | Falta collider en el tilemap o el `Composite Collider 2D` no está regenerado. |
| El jefe no despierta | La `ZonaCombateJefe` necesita un `BoxCollider2D` que cubra la arena (es trigger). |
| El Fragmento no aparece al morir Molrak | En el prefab `Molrak`, `fragmentoPrefab` debe apuntar a `FragmentoAethon`. |
| El checkpoint respawnea dentro del suelo | Sube `offsetRespawn.y` (por defecto 1,2). |
| La cámara enseña el vacío | Estira el `PolygonCollider2D` de `LimitesCamara`. |

---

## 15. Orden recomendado si tienes poco tiempo

La entrega es el **20 de noviembre**. Prioriza en este orden:

1. **Recorrido jugable** (tilemaps de Zona 1 → 3) — sin esto no hay juego.
2. **3 checkpoints** y **arena de Molrak con sus 3 fases** — sin esto no hay final.
3. **Sprites de los prefabs** — sin esto hay cuadrados blancos.
4. **Build de Windows** — el entregable mínimo.
5. **Música del menú y de Ignar** — el criterio pide que el menú abra con música.
6. **Documento de Diseño (Word), Documentos de Producción (Excel) y presentación.**
7. Audio restante, zonas secretas y pulido — mejoran la nota, pero no bloquean la entrega.

---

## 16. Entregables fuera de Unity

El criterio no solo pide el juego: también pide los documentos en su formato final.

| Entregable | Estado en `Documentos/` | Acción |
|---|---|---|
| Documento de Diseño | `Documento_de_Diseno.md` | Abrir en Word, añadir imágenes de concepto y guardar como `.docx` |
| Documentos de Producción | `*.csv` (Sprint Plan, Feature Log, Burn-down, Task Slips, Buglist) | Importar a Excel, crear las gráficas y guardar como `.xlsx` |
| Presentación | `Guion_Presentacion.md` | Montar las diapositivas y ensayar los 5 minutos |
| Build | No generado | Generar `Builds/Windows/Buscador de Luz.exe` (paso 12) |

> Comprueba que los documentos mencionen lo mismo que el juego: 5 corazones, dash en K,
> sprint en Shift, Pulso en Q, 3 checkpoints, 3 fases de Molrak y el mensaje
> "Fragmento de Aethon obtenido".

---

## 17. Qué necesito de ti

- Los errores de consola (texto completo) si algo no compila.
- Capturas de lo que se ve mal en Play.
- Los nombres definitivos para los créditos.
- Aviso cuando el nivel esté recorrible para hacer la pasada de pulido y la build final.

Con eso cierro los Días 8–10 del plan: bugfix, build y documentos.
