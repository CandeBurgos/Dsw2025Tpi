# Trabajo Práctico Integrador
## Desarrollo de Software

### Integrantes 
>Burgos Candela Araceli - 58210 - Candela.Burgos@alu.frt.utn.edu.ar
>
>Herrera Lautaro Fabian - 58305 - Lautaro.Herrera@alu.frt.utn.edu.ar
>
>Hidalgo Bevacqua Lucia Victoria - 57931 - Lucia.Hidalgobevacqua@alu.frt.utn.edu.ar
>
>#### Requisitos:

.NET 8 SDK

Visual Studio 2022 o superior

#### Pasos para ejecutar:

Clonar o descomprimir el proyecto.

Abrir la solución Dsw2025Tpi.sln con Visual Studio.

Asegurarse de que el proyecto de inicio sea Dsw2025Tpi.Api.

### Endpoints

#### /api/products

##### >GET /api/products
Función: Obtiene todos los productos activos.

Ejecución: Llama al método GetProducts() del servicio, que filtra por IsActive == true.

Respuesta:

-200 OK con la lista de productos si hay resultados.

-204 No Content si no hay productos activos.

##### >GET /api/products/{id}
Función: Obtiene un producto específico por su ID.

Ejecución: Llama a GetProductById(id) en el servicio, que valida que el producto exista y esté activo.

Respuesta:

-200 OK con los datos del producto.

-404 Not Found si no existe o está inactivo.

##### >POST /api/products
Función: Crea un nuevo producto.

Ejecución:

El servicio valida campos obligatorios (SKU, nombre, precio, stock).

Verifica que el SKU no exista.

Crea y guarda el producto.

Respuesta:

-201 Created con el producto creado y su ID.

-400 Bad Request si faltan datos o el SKU está duplicado.

-500 Internal Server Error si ocurre otro error.

##### >PUT /api/products/{id}
Función: Actualiza un producto existente.

Ejecución:

Busca el producto por ID.

Valida datos y que el SKU no esté repetido en otro producto.

Actualiza los campos del producto.

Respuesta:

-200 OK con el producto actualizado.

-404 Not Found si el producto no existe.

400 Bad Request por errores de validación.

-500 Internal Server Error si hay error general.

##### >PATCH /api/products/{id}
Función: Activa o desactiva un producto (toggle IsActive).

Ejecución: Llama a ToggleProductStatus(id) del servicio.

Respuesta:

-204 No Content si se actualizó correctamente.

-404 Not Found si el producto no existe.


#### /api/orders

##### >POST /api/orders
Función: Crea una orden nueva.

Ejecución:

-Valida que las direcciones de envío/facturación no estén vacías.

-Verifica que haya al menos un ítem.

-Verifica que los productos existan y tengan stock suficiente.

-Crea y guarda la orden.

-Descuenta el stock de los productos.

Respuesta:

-201 Created con la orden completa.

-400 Bad Request por errores en los datos o stock insuficiente.

-500 Internal Server Error en caso de error inesperado.



