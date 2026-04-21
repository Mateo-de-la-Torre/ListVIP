# ListVIP
## Sistema de Gestión de Promotores y Accesos para Eventos

**Minuta de Relevamiento**

*Programación IV — 2° TUP 4 — UTN*

*Integrantes: Mateo de la Torre, Gianfranco Dealbera*

---

## Temática

En el ámbito de los eventos nocturnos, los promotores son personas encargadas de convocar público a fiestas y boliches. A cambio de su trabajo, reciben una comisión por cada persona de su lista que efectivamente ingresa al evento.

Actualmente este proceso se gestiona de forma informal: los promotores arman sus listas por WhatsApp o planillas de Excel, los porteros las reciben en papel, y el cálculo de comisiones queda sujeto a disputas. No existe un registro confiable de quién ingresó realmente, ni una herramienta que le dé al promotor visibilidad sobre su propio rendimiento.

La aplicación propuesta es un sistema de gestión de promotores y accesos para eventos. Su objetivo principal es darle al promotor una herramienta propia donde pueda administrar su lista de invitados, ver en tiempo real quién ingresó y consultar sus comisiones. De manera complementaria, permite al organizador gestionar eventos y promotores, y al portero validar ingresos en la puerta.

---

## Actores del sistema

- **Organizador:** crea y administra eventos, asigna promotores, define comisiones y liquida pagos una vez finalizado el evento.
- **Promotor:** gestiona su lista de invitados para cada evento, hace seguimiento en tiempo real de los ingresos y visualiza sus comisiones.
- **Portero:** valida el ingreso de personas en la puerta, registrando la hora exacta y controlando el aforo disponible.

---

## Funcionalidades

### Gestión de eventos
- Alta, modificación y cancelación de eventos (nombre, fecha, lugar, capacidad máxima, precio de entrada).
- El evento pasa por los siguientes estados: Borrador → Publicado → En curso → Finalizado / Cancelado.

### Gestión de promotores por evento
- El organizador asigna promotores a un evento con una comisión definida por persona ingresada.
- Un promotor puede trabajar en múltiples eventos y un evento puede tener múltiples promotores.
- El promotor debe aceptar la invitación para poder comenzar a cargar su lista.

### Lista de invitados
- El promotor carga nombre y apellido de sus invitados desde su panel.
- El alta de invitados se cierra automáticamente un tiempo determinado antes del inicio del evento.
- Cada invitado tiene un estado: En lista → Ingresó / No se presentó.

### Validación de ingresos
- El portero busca a una persona por nombre y apellido.
- El sistema indica si está en lista, a qué promotor pertenece y si ya ingresó previamente.
- Al confirmar el ingreso, queda registrado con fecha y hora exacta.
- El sistema muestra el aforo disponible en tiempo real y alerta cuando se alcanza la capacidad máxima.

### Liquidación de comisiones
- Al finalizar el evento, el sistema calcula automáticamente por promotor: personas ingresadas × comisión = monto a pagar.
- El organizador marca las comisiones como liquidadas una vez abonadas.
- La comisión tiene dos estados: Pendiente → Liquidada.

### Panel del promotor
- Vista en tiempo real del estado de cada invitado en su lista.
- Historial de eventos trabajados con métricas de rendimiento.
- Visualización de comisiones pendientes y liquidadas.
