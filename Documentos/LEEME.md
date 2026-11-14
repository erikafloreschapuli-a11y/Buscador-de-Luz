# Documentos de producción — Buscador de Luz

Esta carpeta contiene el contenido listo para convertir en los entregables del Huddle.
Todo está en texto plano (Markdown y CSV) para que puedas editarlo rápido.

## Archivos

| Archivo | Qué es | Entregable |
|---|---|---|
| `Documento_de_Diseno.md` | GDD completo de la beta de Ignar | Documento de Diseño (Word/PDF) |
| `Feature_Log.csv` | Los 49 RF + 10 RNF con estado y responsable | Excel: Feature Log |
| `Sprint_Plan.csv` | Los 4 sprints originales + el cierre | Excel: Sprint Plan |
| `Sprint_Backlog.csv` | Tareas del cierre con horas y estado | Excel: Sprint Backlog |
| `Burn_Down.csv` | Puntos restantes ideales vs reales por día | Excel: Burn-down Chart |
| `Task_Slips.csv` | Tarjetas de tarea con estimación | Excel: Task Slips |
| `Buglist.csv` | 20 bugs con severidad y solución | Excel: Buglist |
| `Guion_Presentacion.md` | Guion de 5 minutos con tiempos | Presentación |

## Cómo pasarlo a Word

1. Abre `Documento_de_Diseno.md` en Visual Studio Code o en un editor Markdown.
2. Copia todo el contenido y pégalo en Word.
3. Aplica **Título 1** a las secciones (`##`) y **Título 2** a las subsecciones (`###`).
4. Inserta las imágenes de concepto en las secciones 3, 4 y 9 (Kael, zonas, HUD y Molrak).
5. Añade portada con el logo de Eclipsis Games y exporta a PDF.

## Cómo pasarlo a Excel

1. Abre Excel → **Datos > Obtener datos > Desde texto/CSV**.
2. Elige el archivo. Origen: **65001 UTF-8**. Delimitador: **punto y coma (;)**.
3. Crea una hoja por archivo y renómbralas como en la tabla de arriba.
4. Para el **Burn-down**: selecciona las columnas *Puntos restantes ideales* y *Puntos
   restantes reales* e inserta un **gráfico de líneas** con la fecha como eje X.
5. Para el **Feature Log**: activa **Filtros** y colorea la columna *Estado*
   (verde = Hecho, ámbar = Pendiente editor/audio/build, azul = En verificación).
6. Para el **Buglist**: ordena por *Severidad* y filtra por *Estado*.

> Los CSV usan **punto y coma** porque Excel en español lo espera así. Si tu Excel está en
> inglés, cambia el delimitador a coma al importar.

## Qué falta rellenar a mano

- [ ] Tu nombre real en `Guion_Presentacion.md` y en los créditos del juego
      (`[TU NOMBRE]` y `[CRÉDITOS]`).
- [ ] Capturas del juego para el Documento de Diseño (menú, HUD, cada zona, Molrak).
- [ ] Fechas reales de tus sprints si difieren de las del requerimiento.
- [ ] Los puntos reales del burn-down si tu avance cambia.

## Criterios de aceptación cubiertos

- **9.** Documento de Diseño y Documentos de Producción entregados y coherentes con el juego.
- **10.** Presentación de 5 minutos lista para proyectar.

El resto de criterios (1–8) se validan con el build de Windows y la prueba completa descrita en
`../Instrucciones_Editor.md`.
