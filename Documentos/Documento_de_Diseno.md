# Documento de Diseño — Buscador de Luz

**Estudio:** Eclipsis Games
**Proyecto:** Buscador de Luz — Beta del Planeta 1: Ignar
**Motor:** Unity 6 (URP 2D) · **Plataforma:** PC (Windows)
**Versión:** Beta 0.9 · **Fecha:** noviembre de 2026

---

## 1. Resumen ejecutivo

**Buscador de Luz** es un plataformero 2D de acción y exploración con estética pixel art de
16 bits. El jugador controla a **Kael**, el último Buscador de Luz, que viaja por planetas
congelados en el tiempo para recuperar los Fragmentos de **Aethon**, la estrella que se apaga.

La beta entregable es el **Planeta 1, Ignar**: un mundo volcánico de forjas abandonadas dividido
en tres zonas. Kael avanza de izquierda a derecha, esquiva lava y trampas, resuelve palancas,
compuertas y válvulas, derrota Golemitas y Chispas, y al final se enfrenta a **Molrak, el Golem
Eterno**, para obtener el Fragmento 1 y desbloquear el **Pulso de Calor**.

- **Género:** plataformero 2D de acción y exploración.
- **Duración de la sesión:** 10–20 minutos.
- **Público:** jugadores casuales e intermedios de 13 a 30 años.
- **Referentes:** Hollow Knight y Ori and the Blind Forest, con una curva más accesible.
- **Pilares:** exploración recompensada, combate legible y un mundo que se cuenta sin diálogos.

---

## 2. Pilares de diseño

1. **Aprender haciendo.** Cada mecánica se presenta en un entorno seguro antes de exigirla
   bajo presión: primero un géiser aislado, después varios; primero una válvula, después el puzzle.
2. **El mundo habla.** Ruinas, forjas y criaturas cuentan la historia; solo hay texto en la
   apertura, el Fragmento obtenido y el final.
3. **Recompensa la curiosidad.** Las zonas secretas esconden cristales grandes y mejoras, nunca
   son obligatorias, pero siempre merecen la pena.
4. **Combate claro.** Ataques con anticipación visible, daño con invencibilidad de 1 s y
   retroalimentación inmediata (parpadeo, sonido, retroceso).

---

## 3. Personaje: Kael

| Atributo | Valor |
|---|---|
| Vida | 5 corazones (10 medios corazones internos) |
| Velocidad | 8 unidades/s (sprint ×1,6) |
| Salto | 7 de impulso, doble salto |
| Ataque | 1 de daño, radio 1,6, cooldown 0,4 s |
| Dash | 14 de velocidad, 0,3 s, invencibilidad 0,3 s, recarga 1,5 s |
| Habilidad | Pulso de Calor (onda de radio 3,5; recarga 3 s, mínimo 1 s) |

**Controles**

| Acción | Tecla |
|---|---|
| Mover | `A` / `D` o flechas |
| Saltar / doble salto | `Espacio` |
| Atacar (puñetazo / patada) | `J` |
| Dash | `K` |
| Sprint | `Shift izquierdo` |
| Pulso de Calor | `Q` |
| Pausa | `Escape` |

**Progresión:** Kael empieza con movimiento, salto doble, ataque y dash. El **Pulso de Calor** se
desbloquea al obtener el Fragmento y se conserva entre sesiones con `PlayerPrefs`.

---

## 4. El mundo: Ignar

Planeta volcánico con forjas industriales abandonadas y una civilización congelada en el tiempo.
Paleta naranja, roja y negra; partículas de fuego y ceniza; scroll horizontal y vertical en la
zona final.

### 4.1 Zona 1 — Forjas Dormidas

- **Objetivo:** aprender a moverse, saltar y combatir.
- **Contenido:** charcos de lava, géiseres cada 3 s, palanca y compuerta, primeros muros de roca,
  Golemitas y Chispas, **Checkpoint 1**.
- **Secreto:** pared falsa con un **Cristal de Mejora**.
- **Momento clave:** el primer géiser enseña el ritmo de espera y salto.

### 4.2 Zona 2 — Canales de Magma

- **Objetivo:** dominar el ritmo y la exploración vertical.
- **Contenido:** ríos de lava, lluvia de rocas, géiseres grandes, **puentes que se derrumban**,
  **puzzle de 3 válvulas**, **Checkpoint 2**.
- **Secreto:** catarata de lava con un **Cristal de Luz grande**.
- **Momento clave:** el puzzle abre la compuerta que da paso a El Núcleo.

### 4.3 Zona 3 — El Núcleo

- **Objetivo:** poner a prueba todo lo aprendido y cerrar la historia.
- **Contenido:** lago de lava con plataformas flotantes, explosiones de magma, **Checkpoint 3**
  antes de la puerta, arena de Molrak.
- **Momento clave:** la puerta se cierra a la espalda de Kael; solo sale al vencer.

### 4.4 Mapa de progresión

```
Inicio ──► Zona 1 ──► Palanca/Compuerta ──► Zona 2 ──► Puzzle de válvulas ──► Zona 3
                                                                              │
                                                            Checkpoint 3 ──► Arena de Molrak
                                                                              │
                                                          Fragmento de Aethon + Pulso de Calor
```

---

## 5. Enemigos

### 5.1 Golemita de Lava

- **Vida:** 3. **Comportamiento:** patrulla, detecta a Kael, lo persigue y ataca por contacto.
- **Rastro:** deja lava que daña y desaparece a los 3 s.
- **Recompensa:** 20 % de probabilidad de soltar un Cristal de Luz pequeño.
- **Lectura:** silueta ancha y lenta; el jugador aprende a golpear y retirarse.

### 5.2 Chispa Voladora

- **Vida:** 2. **Comportamiento:** flota, vuela en grupos de 3, ataca en picada y explota al
  contacto.
- **Lectura:** movimiento rápido y frágil; el dash permite atravesar su ataque.
- **Recompensa:** ninguna directa; presión de grupo.

### 5.3 Molrak, el Golem Eterno

Golem gigante de roca y lava (3 veces la altura de Kael) con un núcleo brillante en el pecho.

| Fase | Vida restante | Ataques | Punto débil |
|---|---|---|---|
| 1 | 100–66 % | Embestida horizontal lenta, golpe de puño | Cuerpo |
| 2 | 66–33 % | Lluvia de rocas, salto de impacto | Cuerpo |
| 3 | 33–0 % | Explosión de lava en área | **Núcleo expuesto** (daño ×2) |

Al morir: cae el Fragmento, se abre la puerta, se desbloquea el Pulso de Calor y aparece la
pantalla **"FRAGMENTO DE AETHON OBTENIDO"**.

---

## 6. Items y recogibles

| Item | Efecto | Dónde |
|---|---|---|
| Cristal de Luz pequeño | Cura medio corazón | Drops (20 %) y repartidos |
| Cristal de Luz grande | Cura 1 corazón | Zonas secretas y antes del jefe |
| Cristal de Mejora | Reduce 0,5 s el cooldown | Zona secreta |
| Fragmento de Aethon | Completa Ignar y desbloquea el Pulso | Lo suelta Molrak |
| Cristal de Checkpoint | Guarda la posición (no se recoge) | 3 fijos, uno por zona |

---

## 7. Interfaz

- **HUD:** 5 corazones (lleno / medio / vacío), slot del Fragmento, icono del Pulso con barra de
  recarga circular y tecla `Q`.
- **Menú principal:** Nueva Partida, Continuar, Opciones, Créditos, Mejoras, Mapa, Salir.
- **Pausa (`Escape`):** Reanudar, Opciones, Reiniciar, Menú principal.
- **Game Over:** Reintentar desde el último checkpoint y Menú principal.
- **Opciones:** volumen general y pantalla completa, guardados en `PlayerPrefs`.
- **Pantalla del Fragmento:** mensaje a pantalla completa con fundido, sin detener el juego.

---

## 8. Audio

| Uso | Pista | Estado |
|---|---|---|
| Menú | M09 (ambiente tranquilo) | Pendiente de importar |
| Nivel | M02 "Forjas del Fin" (orquestal épico) | Pendiente de importar |
| Lava | S03 burbujeo | Pendiente |
| Golem | S04 golpe | Pendiente |
| Chispa | S05 | Pendiente |
| Checkpoint | S24 | Pendiente |
| Fragmento | S25 | Pendiente |

El `AudioManager` se crea automáticamente y carga la música de
`Assets/Resources/Audio/Musica/<Escena>`. Los SFX se asignan por Inspector.

---

## 9. Arte

- **Estilo:** pixel art de 16 bits, paleta volcánica (naranja, rojo, negro, gris piedra).
- **Kael:** idle, caminar, correr, saltar, ataque 1, ataque 2.
- **Enemigos:** Golemita y Chispa con idle, movimiento, ataque, daño y muerte.
- **Molrak:** idle, caminar, ataques por fase, daño y muerte (o Golemita escalada ×2 con tinte
  rojo como contingencia).
- **Escenarios:** tileset volcánico, tres capas de parallax por zona, géiser, palanca, compuerta,
  muro rompible, puente, válvula y cristales.
- **Importación:** Filter Mode Point y sin compresión (RNF-03).

---

## 10. Flujo de juego

1. El juego abre en el **menú principal** con música.
2. **Nueva Partida** borra el progreso y carga `Ignar`; **Continuar** respeta el guardado.
3. Kael recorre las tres zonas, activa checkpoints y recoge cristales.
4. Al morir aparece **Game Over**; **Reintentar** lo devuelve al último checkpoint.
5. En la arena, la puerta se cierra y Molrak despierta.
6. Al vencerlo, recoge el Fragmento, ve el mensaje final y obtiene el Pulso de Calor.
7. Puede volver al menú, ver Créditos/Mejoras/Mapa o salir.

---

## 11. Requerimientos y estado

| Bloque | Requerimientos | Estado |
|---|---|---|
| Personaje | RF-01 a RF-11 | Implementado (dash y sprint incluidos) |
| Cámara | RF-12 a RF-14 | Implementado |
| Salud y checkpoints | RF-15 a RF-19 | Implementado |
| Enemigos | RF-20 a RF-24 | Implementado |
| Obstáculos | RF-25 a RF-34 | Implementado (falta colocarlos en el nivel) |
| Items | RF-35 a RF-39 | Implementado |
| Interfaz | RF-40 a RF-46 | Implementado |
| Audio | RF-47 a RF-49 | Código listo; faltan los clips |
| No funcionales | RNF-01 a RNF-10 | En verificación (build y rendimiento) |

---

## 12. Riesgos y contingencias

| Riesgo | Contingencia |
|---|---|
| Falta de arte para Molrak | Golemita escalada ×2 con tinte rojo |
| Falta de música con licencia | Packs CC0 de OpenGameArt / Freesound |
| El nivel no se termina a tiempo | Unir Zona 1 y Zona 3; Zona 2 fuera del build |
| Problemas de rendimiento | Reducir partículas y luces 2D; profiler |
| El puzzle bloquea el avance | Añadir una palanca de emergencia que abra la compuerta |

---

## 13. Criterios de aceptación de la beta

1. El juego corre desde un build de Windows sin errores.
2. Kael se mueve, salta, ataca con J, hace dash con K, corre con Shift y recibe daño.
3. Los 5 corazones reflejan la vida real (incluido el medio corazón).
4. La lava y los enemigos dañan; Kael puede derrotarlos.
5. Al quedarse sin vida aparece Game Over; Reintentar lo devuelve al último checkpoint con la vida completa.
6. El nivel es recorrible de inicio a fin.
7. Final jugable: recoger el Fragmento de Aethon.
8. Menú principal accesible al abrir el juego.
9. Documento de Diseño y Documentos de Producción entregados.
10. Presentación de 5 minutos lista.
