# Guion de la presentación (5 minutos)

**Objetivo:** mostrar que la beta de *Buscador de Luz — Ignar* es un juego completo y jugable,
explicar las decisiones de diseño y cerrar con el estado de la entrega.

**Formato:** 8 diapositivas + demo en vivo de 2 minutos. Si la demo falla, ten un vídeo de
respaldo de 90 segundos grabado desde el build.

**Consejo:** habla mirando al público, no a la pantalla. La demo va en el minuto 2, cuando la
atención está alta.

---

## Diapositiva 1 — Portada (0:00 – 0:20)

**En pantalla:** título, logo, "Beta del Planeta 1: Ignar", tu nombre.

> "Buenos días. Soy [TU NOMBRE] y este es *Buscador de Luz*, un plataformero 2D de acción y
> exploración. Hoy presento la beta del Planeta 1: Ignar."

---

## Diapositiva 2 — La idea (0:20 – 0:50)

**En pantalla:** Kael, el Cristal de Luz y Aethon apagándose.

> "Kael es el último Buscador de Luz. Aethon, la estrella central, se está apagando, y él viaja
> por planetas congelados en el tiempo para recuperar sus fragmentos. La historia se cuenta con
> el mundo: ruinas, forjas y criaturas. Solo hay texto en tres momentos: la apertura, el
> fragmento y el final."

**Pilar:** exploración recompensada + combate legible.

---

## Diapositiva 3 — Ignar en tres zonas (0:50 – 1:30)

**En pantalla:** mapa del planeta con las tres zonas y una captura de cada una.

> "Ignar es un mundo volcánico de forjas abandonadas. Se divide en tres zonas: **Forjas
> Dormidas**, donde se aprenden el salto y el combate; **Canales de Magma**, con puentes que se
> derrumban, lluvia de rocas y un puzzle de tres válvulas; y **El Núcleo**, con plataformas sobre
> lava y la arena de Molrak. La sesión dura entre 10 y 20 minutos."

**Menciona:** 3 checkpoints, zonas secretas con cristales grandes y de mejora.

---

## Diapositiva 4 — Kael (1:30 – 2:00)

**En pantalla:** tabla de controles y las habilidades.

> "Kael tiene cinco corazones, doble salto, ataque con J, dash con invencibilidad y sprint. Al
> final del planeta desbloquea el **Pulso de Calor**, una onda que empuja enemigos y rompe muros
> de roca. Es la habilidad que conecta este planeta con el siguiente."

**Controles en pantalla:** A/D, Espacio, J, K, Shift, Q, Escape.

---

## Diapositiva 5 — Demo en vivo (2:00 – 4:00)

**En pantalla:** el juego en el build de Windows.

Guion de la demo (90–120 s):

1. Menú → **Nueva partida**.
2. Corre, salta un géiser y golpea a una Golemita (que suelte un cristal).
3. Recoge un cristal pequeño: se ve el **medio corazón** en el HUD.
4. Cruza un puente que se derrumba.
5. Entra en la arena: la puerta se cierra, Molrak despierta.
6. (Si da tiempo) baja su vida con `vidaMaxima` reducida y recoge el Fragmento.
7. Muestra la pantalla **"FRAGMENTO DE AETHON OBTENIDO"** y el icono del Pulso en el HUD.

**Frase de cierre de la demo:** "Y con el Fragmento, Ignar despierta y Kael obtiene el Pulso de
Calor."

> **Si algo falla:** pulsa Escape, vuelve al menú y reproduce el vídeo de respaldo. No te
> disculpes dos veces: sigue con la siguiente diapositiva.

---

## Diapositiva 6 — Producción (4:00 – 4:30)

**En pantalla:** burn-down, sprint backlog y buglist resumidos.

> "El proyecto se organizó en cuatro sprints y una fase final de nueve días. Trabajé con un
> backlog por requerimientos: 49 funcionales y 10 no funcionales. Cada día cerraba con
> compilación sin errores, una prueba de cinco minutos y un commit. La lista de bugs está en el
> Excel: 20 incidencias, todas cerradas o con plan."

**Datos rápidos:**
- 46 scripts en C#, 14 prefabs reutilizables.
- 3 zonas, 3 enemigos, 1 jefe de 3 fases.
- Guardado con `PlayerPrefs` y opciones persistentes.

---

## Diapositiva 7 — Estado y siguientes pasos (4:30 – 4:50)

**En pantalla:** checklist de los 10 criterios de aceptación.

> "La beta cumple los diez criterios: build de Windows, movimiento y combate, corazones
> incluido el medio, checkpoints, Game Over, nivel recorrible, final con el Fragmento, menú,
> documentos y esta presentación. Lo siguiente sería el Planeta 2, Veluun, y pulir el arte de
> Molrak."

---

## Diapositiva 8 — Cierre (4:50 – 5:00)

**En pantalla:** "Gracias" + contacto + enlace al repositorio.

> "Gracias por jugar. ¿Preguntas?"

---

## Lista de comprobación antes de presentar

- [ ] Build de Windows abierto y probado en el equipo de la presentación.
- [ ] Resolución 1920×1080 y pantalla completa.
- [ ] Mando/teclado funcionando; `Escape` vuelve al menú.
- [ ] Vídeo de respaldo de 90 s en el escritorio.
- [ ] Cronómetro ensayado: 5:00 exactos.
- [ ] Créditos con el nombre real (sin `[TU NOMBRE]`).
- [ ] Documento de Diseño y Excel impresos o en PDF, por si los piden.
