# Plan de Trabajo — Buscador de Luz (Beta Ignar)

**Entrega:** 20 de noviembre de 2026 · **Hoy:** 11 de noviembre de 2026 · **Días hábiles:** 9
**Alcance:** Planeta 1 completo (3 zonas + Molrak + Pulso de Calor) jugable de inicio a fin, con menús, audio y build de Windows.

---

## 1. Estado de partida

| Área | Estado |
|---|---|
| Movimiento, doble salto, animaciones | ✅ Listo |
| Combate J (2 animaciones) | ✅ Listo |
| Vida, HUD de 5 corazones, lava, enemigos | ✅ Listo (con bugs corregidos el 11 nov) |
| Checkpoint + respawn + Game Over + pausa | ✅ Listo (Fase 0) |
| Cámara Cinemachine + parallax | ✅ Listo |
| Palanca, compuerta, géiser, rastro de lava | ✅ Código listo, falta colocarlos en nivel |
| Items, Fragmento de Aethon | ✅ Código + prefabs listos (falta sprite) |
| Molrak | ✅ IA de 3 fases + vida + drop del Fragmento |
| Pulso de Calor + muros rompibles | ✅ Código + prefab de muro |
| Zonas secretas, puentes, válvulas, rocas, magma | ✅ Código + prefabs listos, falta colocarlos |
| Dash / sprint | ✅ `PlayerDash` + sprint en `PlayerMovement` |
| Audio (música + SFX) | ✅ `AudioManager` automático; faltan los clips |
| Zona 2 (`Zona2.unity`) | ⚠️ Boceto fuera del build; el recorrido va dentro de `Ignar` |
| Build Windows | ❌ No generado (Día 9) |
| Documentos (Word/Excel) + presentación | ❌ No generados (Día 10) |

> **Documentos de apoyo generados:**
> - `Instrucciones_Editor.md`: lista exacta de lo que hay que colocar y cablear en Unity.
> - `Documentos/`: contenido del Documento de Diseño, Feature Log, Sprint Plan/Backlog,
>   Burn-down, Task Slips, Buglist y guion de la presentación.

---

## 2. Regla de reparto

| Responsable | Trabajo |
|---|---|
| **Yo (agente)** | Scripts C#, prefabs por YAML, cableado de escenas por YAML, Build Settings, corrección de bugs, verificación con `dotnet build` |
| **Tú (editor Unity)** | Colocar objetos y referencias en el Inspector, tilemaps, sprites/animaciones, probar jugando, generar audio |

> Cada día termina con: (1) compilación sin errores, (2) prueba de 5 minutos en Play, (3) commit.

---

## 3. Plan día por día

### ✅ Día 1 — 11 nov · FASE 0: Estabilizar (HECHO)
| # | Tarea | RF | Estado |
|---|---|---|---|
| 0.1 | Renombrar `PauseManager.CS` → `.cs` | — | ✅ |
| 0.2 | Flujo único muerte → Game Over → respawn en checkpoint | RF-08, RF-45 | ✅ |
| 0.3 | Invencibilidad 1 s con parpadeo | RF-15/16 | ✅ |
| 0.4 | Daño de lava con intervalo de 1 s | RF-15 | ✅ |
| 0.5 | Guardado real del checkpoint en `PlayerPrefs` | RF-19 | ✅ |
| 0.6 | Cablear `PanelPausa` y `PanelGameOver` en la escena | RF-44/45 | ✅ |
| 0.7 | Bloqueo de pausa durante Game Over | RF-45 | ✅ |
| 0.8 | Validaciones null y limpieza de `Debug.Log` | RNF-06 | ✅ |

**Criterio de terminado:** compila sin errores ✔ · Escape abre pausa ✔ (probar en Play).

---

### ✅ Día 2 — 12 nov · FASE 1A: Items, HUD y drops (HECHO)
| # | Tarea | RF | Scripts | Quién |
|---|---|---|---|---|
| 1.1 | `ItemRecolectable` (pequeño / grande / mejora) con efecto y sonido | RF-35, 36, 37 | `ItemRecolectable.cs` | Yo |
| 1.2 | Slot de fragmento en el HUD (se ilumina al obtenerlo) | RF-41 | `HUDManager.cs` | Yo + tú (UI) |
| 1.3 | Ícono de Pulso de Calor con barra de cooldown | RF-42 | `HUDManager.cs` | Yo + tú (UI) |
| 1.4 | Drop del 20 % de la Golemita (hoy es un comentario vacío) | RF-22 | `GolemitaHealth.cs` | Yo |
| 1.5 | Contadores de estadísticas al recoger/matar | — | `DatosJuego` | Yo |
| 1.6 | Colocar cristales y zonas secretas en Zona 1 y 2 | RF-29 | — | Tú |

**Terminado cuando:** recoger un cristal cura medio corazón / 1 corazón y el HUD lo refleja.

---

### ✅ Día 3 — 13 nov · FASE 1B: Molrak y final del nivel (HECHO en código)
| # | Tarea | RF | Scripts | Quién |
|---|---|---|---|---|
| 1.7 | `MolrakIA`: Fase 1 (embestida horizontal + golpe de puño) | RF-24 | `MolrakIA.cs` | Yo |
| 1.8 | `MolrakHealth` con 3 fases y núcleo expuesto en Fase 3 | RF-24 | `MolrakHealth.cs` | Yo |
| 1.9 | Puerta del jefe que se cierra al entrar | RF-34 | `PuertaJefe.cs` | Yo |
| 1.10 | Al morir Molrak: suelta el Fragmento + guarda progreso | RF-38 | `FragmentoAethon.cs` | Yo |
| 1.11 | Escena/pantalla "Fragmento obtenido" con texto | RF-46 | `PanelFragmento.cs` | Yo + tú (texto) |
| 1.12 | 3 checkpoints (uno por zona) colocados | RF-17 | — | Tú |
| 1.13 | Arena de Molrak montada en El Núcleo | 4.2 | — | Tú |

**Terminado cuando:** se puede llegar al jefe, derrotarlo, recoger el Fragmento y ver el mensaje final.

---

### ✅ Día 4 — 14 nov · FASE 2A: Pulso de Calor y secretos (HECHO en código)
| # | Tarea | RF | Scripts | Quién |
|---|---|---|---|---|
| 2.1 | `PulsoDeCalor`: onda que empuja enemigos y rompe muros | RF-11, 28 | `PulsoDeCalor.cs` | Yo |
| 2.2 | `MuroRompible` con sprite intacto/destruido | RF-28 | `MuroRompible.cs` | Yo |
| 2.3 | Desbloqueo del Pulso al derrotar a Molrak | RF-11 | `GameManager` / `DatosJuego` | Yo |
| 2.4 | Pared falsa (Zona 1) y catarata de lava (Zona 2) | RF-29 | — | Tú |
| 2.5 | Cristal de Mejora (reduce cooldown 0.5 s) | RF-37 | `ItemRecolectable.cs` | Yo |
| 2.6 | Géiseres repartidos en Zona 1 | RF-26 | — | Tú |

**Terminado cuando:** el Pulso rompe un muro y empuja a una Golemita; las 2 zonas secretas existen.

---

### ✅ Día 5 — 15 nov · FASE 2B: Trampas de Zona 2 (HECHO en código)
| # | Tarea | RF | Scripts | Quién |
|---|---|---|---|---|
| 2.7 | `PuenteDerrumbable` (se cae al pisarlo) | RF-30 | `PuenteDerrumbable.cs` | Yo |
| 2.8 | `Valvula` + `PuzzleValvulas` (orden correcto → abre paso) | RF-31 | `Valvula.cs`, `PuzzleValvulas.cs` | Yo |
| 2.9 | `LluviaRocas` volcánicas + géiseres grandes | RF-32 | `LluviaRocas.cs` | Yo |
| 2.10 | Ríos de lava y recorrido de Canales de Magma | 4.1 | — | Tú |
| 2.11 | Colocar puentes, válvulas y rocas | RF-30/31/32 | — | Tú |

**Terminado cuando:** el puzzle de válvulas abre el paso a la Zona 3 y los puentes caen.

---

### ✅ Día 6 — 16 nov · FASE 2C: El Núcleo y nivel completo (HECHO en código)
| # | Tarea | RF | Scripts | Quién |
|---|---|---|---|---|
| 2.12 | `ExplosionMagma` aleatoria en el suelo | RF-33 | `ExplosionMagma.cs` | Yo |
| 2.13 | Lago de lava con plataformas flotantes | 4.1 | — | Tú |
| 2.14 | Scroll vertical en la zona final | RF-13 | Cinemachine | Tú |
| 2.15 | Recorrido completo de inicio a fin sin bloqueos | Criterio 6 | — | Tú |

**Terminado cuando:** una partida completa dura entre 10 y 20 minutos (RNF-05).

---

### ✅ Día 7 — 17 nov · FASE 3A: Audio, dash y opciones (HECHO en código)
| # | Tarea | RF | Scripts | Quién |
|---|---|---|---|---|
| 3.1 | `AudioManager` (música + SFX, volumen en PlayerPrefs) | RF-47/48 | `AudioManager.cs` | Yo |
| 3.2 | Música M02 (Ignar) y M09 (menú) | RF-47/48 | — | Tú |
| 3.3 | SFX: S03 lava, S04 golem, S05 chispa, S24 checkpoint, S25 fragmento | RF-49 | — | Tú |
| 3.4 | `PlayerDash` (0,3 s invencibilidad, cooldown 1,5 s) | RF-09 | `PlayerDash.cs` | Yo |
| 3.5 | Sprint al mantener botón | RF-10 | `PlayerMovement.cs` | Yo |
| 3.6 | Panel de Opciones en pausa y menú (volumen, pantalla) | RF-43/44 | `OpcionesUI.cs` | Yo + tú |

**Terminado cuando:** hay música en menú y nivel, y todos los SFX suenan.

---

### Día 8 — 18 nov · FASE 3B: Pulido y bugfix (~5 h)
| # | Tarea | RNF | Quién |
|---|---|---|---|
| 3.7 | 60 FPS estables en gama media (profiler) | RNF-02 | Tú |
| 3.8 | Revisar Filter Mode Point y compresión de sprites | RNF-03 | Tú |
| 3.9 | Respuesta inmediata de controles (sin retraso) | RNF-04 | Yo |
| 3.10 | Buglist: jugar 3 veces el nivel completo y anotar fallos | Criterio 1 | Tú |
| 3.11 | Corregir todos los bugs de la lista | — | Yo |
| 3.12 | Prefabs reutilizables de todos los obstáculos | RNF-07 | Yo |

**Terminado cuando:** 3 partidas completas sin bugs bloqueantes.

---

### Día 9 — 19 nov · FASE 4A: Build y validación (~4 h)
| # | Tarea | Criterio | Quién |
|---|---|---|---|
| 4.1 | Build Windows (escenas `MainMenu` + `Ignar`) | RNF-10 | Tú |
| 4.2 | Ejecutar el .exe y probar los 10 criterios de aceptación | 1-8 | Ambos |
| 4.3 | Verificar `Continuar` y guardado entre sesiones | RF-19 | Tú |
| 4.4 | Corregir lo que falle en build | — | Yo |

**Terminado cuando:** el .exe arranca en el menú, se juega entero y no hay errores en consola.

---

### Día 10 — 20 nov · FASE 4B: Entregables (~4 h)
| # | Tarea | Criterio | Quién |
|---|---|---|---|
| 4.5 | Documento de Diseño (Word) con imágenes de concepto | 9 | Tú (contenido listo en `Documentos/`) |
| 4.6 | Documentos de Producción (Excel): Feature Log, Burn-down, Buglist, Sprint Backlog | 9 | Tú (CSV listos en `Documentos/`) |
| 4.7 | Presentación de 5 minutos | 10 | Tú (guion listo en `Documentos/Guion_Presentacion.md`) |
| 4.8 | Commit final + push a GitHub | RNF-08 | Tú |

---

## 4. Backlog por requerimiento (lo que falta)

| RF | Requerimiento | Script nuevo | Día |
|---|---|---|---|
| RF-09 | Dash con invencibilidad y cooldown | `PlayerDash.cs` | 7 |
| RF-10 | Sprint | `PlayerMovement.cs` | 7 |
| RF-11 | Pulso de Calor | `PulsoDeCalor.cs` | 4 |
| RF-22 | Drop del 20 % de la Golemita | `GolemitaHealth.cs` | 2 |
| RF-24 | Molrak 3 fases | `MolrakIA.cs`, `MolrakHealth.cs` | 3 |
| RF-28 | Muros rompibles | `MuroRompible.cs` | 4 |
| RF-30 | Puentes derrumbables | `PuenteDerrumbable.cs` | 5 |
| RF-31 | Puzzle de 3 válvulas | `Valvula.cs`, `PuzzleValvulas.cs` | 5 |
| RF-32 | Lluvia de rocas | `LluviaRocas.cs` | 5 |
| RF-33 | Explosiones de magma | `ExplosionMagma.cs` | 6 |
| RF-34 | Puerta del jefe | `PuertaJefe.cs` | 3 |
| RF-35/36/37 | Cristales pequeño, grande y de mejora | `ItemRecolectable.cs` | 2 |
| RF-38 | Fragmento de Aethon | `FragmentoAethon.cs` | 3 |
| RF-41/42 | Slot de fragmento y cooldown de habilidad | `HUDManager.cs` | 2 |
| RF-46 | Pantalla "Fragmento obtenido" | `PanelFragmento.cs` | 3 |
| RF-47/48/49 | Audio | `AudioManager.cs` | 7 |
| RF-43/44 | Opciones en menú y pausa | `OpcionesUI.cs` | 7 |

---

## 5. Checklist exclusivo del editor (tú)

> Detalle paso a paso, con coordenadas y valores, en **`Instrucciones_Editor.md`**.

- [ ] Colocar **3 checkpoints** en `Ignar` (hoy solo hay 1; usa `Checkpoint.prefab`)
- [ ] Colocar cristales y las 2 zonas secretas
- [ ] Montar la arena de Molrak y la puerta
- [ ] Construir Zona 2 (ríos, puentes, válvulas) y Zona 3 (lago, plataformas)
- [ ] Repartir géiseres y lluvia de rocas
- [ ] Importar música y SFX a `Assets/Resources/Audio/Musica` y `.../SFX`
- [ ] Asignar sprites de items, muro, válvula, puerta y Molrak
- [ ] Generar el build de Windows
- [ ] Convertir el contenido de `Documentos/` a Word/Excel y presentación (texto ya listo)

### Ya hecho por el agente (no requiere editor)

- [x] Todos los scripts, prefabs y el cableado de las escenas
- [x] HUD, paneles de Opciones, Créditos, Mejoras, Mapa y pantalla del Fragmento
- [x] `AudioManager` automático con música por escena desde `Resources`
- [x] `PlayerDash` y `PulsoDeCalor` añadidos al jugador al iniciar
- [x] Build Settings (`MainMenu` + `Ignar`)
- [x] Compilación verificada: 0 errores, 0 avisos nuevos

---

## 6. Riesgos y contingencias

| Riesgo | Probabilidad | Contingencia |
|---|---|---|
| No hay tiempo para las 3 fases de Molrak | Alta | Dejar **solo Fase 1** (ya previsto en el doc, §4.2) |
| No hay sprites de Molrak | Alta | Reutilizar la Golemita escalada ×3 con tinte rojo |
| No hay audio libre | Media | Usar packs gratuitos (freesound / OpenGameArt) con licencia CC0 |
| Zona 2 no se termina | Media | Unir Zona 1 y 3 en un solo recorrido y dejar Zona 2 fuera del build |
| `Zona2.unity` queda huérfana | Media | Decidir el 12 nov: integrarla en `Ignar` o eliminarla del proyecto |
| Dash/sprint rompen el diseño del nivel | Baja | Dejarlos al final (día 7); si falla, se desactivan sin borrar código |

---

## 7. Criterios de aceptación (checklist final, 19 nov)

- [ ] 1. El juego corre desde un build de Windows sin errores de compilación
- [ ] 2. Kael se mueve, salta, ataca y recibe daño correctamente
- [ ] 3. Los 5 corazones del HUD reflejan la vida real
- [ ] 4. Lava y enemigos dañan; Kael derrota Golemitas y Chispas
- [ ] 5. Al morir reaparece en el último checkpoint; al perder todo aparece Game Over
- [ ] 6. El nivel de Ignar es recorrible de inicio a fin
- [ ] 7. Final jugable: recoger el Fragmento de Aethon
- [ ] 8. Menú principal accesible al abrir el juego
- [ ] 9. Documento de Diseño y Documentos de Producción entregados
- [ ] 10. Presentación de 5 minutos lista

---

## 8. Definición de "terminado" (cada día)

1. `dotnet build` sin errores.
2. 5 minutos de Play sin excepciones en consola.
3. Commit con mensaje `Día N: <tarea>`.
4. Tarea marcada en este documento.
