let listaLibros = [];

let nombreInput = document.getElementById("nameId");
let portadaInput = document.getElementById("portadaId");
let libroInput = document.getElementById("libroId");
let formLibro = document.getElementById("formUser");
let indiceEditar = document.getElementById("editIndex");
let tablaLibrosBody = document.getElementById("userTable");
let tituloModal = document.getElementById("titleId");
let modal = new bootstrap.Modal(document.getElementById("modalId"));
let tabla;

async function arranque() {
    listaLibros = await obtener("Libros/ListaDeLibros")
}

formLibro.addEventListener("submit", async (e) => {
    e.preventDefault();

    if (indiceEditar.value === "") {

        await crearLibroAsync(nombreInput.value, portadaInput.files[0], libroInput.files[0])

        Swal.fire("Agregado!", "Libro agregado correctamente.", "success");

    } else {

        await actualizarLibroAsync(indiceEditar.value, nombreInput.value, portadaInput.files[0], libroInput.files[0]);

        Swal.fire("Actualizado!", "Libro actualizado correctamente.", "success");
    }

    renderizarTabla();
    formLibro.reset();
    indiceEditar.value = "";
    modal.hide();
});

async function renderizarTabla() {

    await arranque()

    if (tabla) {
        tabla.destroy();
    }

    console.log(listaLibros)

    tablaLibrosBody.innerHTML = "";
    listaLibros.forEach((libro, index) => {
        let row = document.createElement("tr");
        row.innerHTML = `
            <td>${libro.nombre}</td>
            <td><a target="_blank" href="${libro.rutaPortada}"> Abrir</a></td>
            <td><a target="_blank" href="${libro.rutaArchivo}"> Abrir</a></td>
            <td>
                <button class="btn btn-primary btn-sm" onclick="editarLibro(${index})">
                    <i class="bi bi-pencil"></i>
                </button>
                <button class="btn btn-danger btn-sm" onclick="eliminarLibro(${index})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>`;
        tablaLibrosBody.appendChild(row);
    });

    tabla = new DataTable("#tablaLibro");
}

function editarLibro(index) {
    let Libro = listaLibros[index];
    nombreInput.value = Libro.nombre;
    indiceEditar.value = Libro.id;
    tituloModal.textContent = "Editar Libro";
    modal.show();
}

function crearLibro() {
    nombreInput.value = "";
    indiceEditar.value = "";
    tituloModal.textContent = "Agregar Libro";
    modal.show();
}

function eliminarLibro(index) {
    Swal.fire({
        title: "¿Eliminar Libro?",
        text: "Esta acción no se puede deshacer",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    }).then(async (result) => {
        if (result.isConfirmed) {

            let libro = listaLibros[index];

            await borrar(`Libros/BorrarLibro/${libro.id}`)

            renderizarTabla();

            Swal.fire("Eliminado!", "El Libro ha sido eliminado.", "success");
        }
    });
}


renderizarTabla();

