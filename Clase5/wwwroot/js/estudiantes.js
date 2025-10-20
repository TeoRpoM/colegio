let listaEstudiantes = [];

let nombreInput = document.getElementById("nameId");
let edadInput = document.getElementById("dateId");
let formEstudiante = document.getElementById("formUser");
let indiceEditar = document.getElementById("editIndex");
let tablaEstudiantesBody = document.getElementById("userTable");
let tituloModal = document.getElementById("titleId");
let modal = new bootstrap.Modal(document.getElementById("modalId"));
let tabla;

async function arranque() {
    listaEstudiantes = await obtener("Estudiantes/ListaDeEstudiantes")
}

formEstudiante.addEventListener("submit", async (e) => {
    e.preventDefault();

    if (indiceEditar.value === "") {

        await crearEstudianteAsync(nombreInput.value, edadInput.value, "11")

        Swal.fire("Agregado!", "Estudiante agregado correctamente.", "success");

    } else {

        await actualizarEstudiante(indiceEditar.value, nombreInput.value, edadInput.value, "10");

        Swal.fire("Actualizado!", "Estudiante actualizado correctamente.", "success");
    }

    renderizarTabla();
    formEstudiante.reset();
    indiceEditar.value = "";
    modal.hide();
});

async function renderizarTabla() {

    await arranque()

    if (tabla) {
        tabla.destroy();
    }

    console.log(listaEstudiantes)

    tablaEstudiantesBody.innerHTML = "";
    listaEstudiantes.forEach((estudiante, index) => {
        let row = document.createElement("tr");
        row.innerHTML = `
            <td>${estudiante.nombre}</td>
            <td>${estudiante.edad}</td>
            <td>
                <button class="btn btn-primary btn-sm" onclick="editarEstudiante(${index})">
                    <i class="bi bi-pencil"></i>
                </button>
                <button class="btn btn-danger btn-sm" onclick="eliminarEstudiante(${index})">
                    <i class="bi bi-trash"></i>
                </button>
            </td>`;
        tablaEstudiantesBody.appendChild(row);
    });

    tabla = new DataTable("#tablaEstudiante");
}

function editarEstudiante(index) {
    let estudiante = listaEstudiantes[index];
    nombreInput.value = estudiante.nombre;
    edadInput.value = estudiante.edad;
    indiceEditar.value = estudiante.id;
    tituloModal.textContent = "Editar Estudiante";
    modal.show();
}

function crearEstudiante() {
    nombreInput.value = "";
    edadInput.value = "";
    indiceEditar.value = "";
    tituloModal.textContent = "Agregar Estudiante";
    modal.show();
}

function eliminarEstudiante(index) {
    Swal.fire({
        title: "¿Eliminar estudiante?",
        text: "Esta acción no se puede deshacer",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    }).then(async (result) => {
        if (result.isConfirmed) {

            let estudiante = listaEstudiantes[index];

            await borrar(`Estudiantes/BorrarEstudiante/${estudiante.id}`)

            renderizarTabla();

            Swal.fire("Eliminado!", "El estudiante ha sido eliminado.", "success");
        }
    });
}


renderizarTabla();

