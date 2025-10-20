//let json = {
//    Nombre: nombre,
//    Edad: edad,
//    Grado: grado
//}

//let textoPlano = "{"Nombre": "mateo","Edad": "2","Grado": "10"}"

async function obtener(url) {

    let respuesta = await fetch(url)

    let datos = await respuesta.json();

    return datos;

}

async function crearEstudianteAsync(nombre, edad, grado) {

    let respuesta = await fetch("Estudiantes/CrearEstudiante", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Nombre: nombre,
            Edad: edad,
            Grado: grado
        })
    })

}

async function actualizarEstudiante(id, nombre, edad, grado) {

    let respuesta = await fetch("Estudiantes/ActualizarEstudiante", {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            Id: id,
            Nombre: nombre,
            Edad: edad,
            Grado: grado
        })
    })
}

async function borrar(url) {

    await fetch(url, { method: "DELETE" })

}

async function crearLibroAsync(nombre, portada, libro) {

    let form = new FormData();

    form.append("Nombre", nombre);
    form.append("RutaPortada", portada);
    form.append("RutaLibro", libro);

    await fetch("Libros/CrearLibro", {
        method: "POST",
        body: form
    })

}

async function actualizarLibroAsync(id, nombre, portada, libro) {

    let form = new FormData();

    form.append("Id", id);
    form.append("Nombre", nombre);
    form.append("RutaPortada", portada);
    form.append("RutaLibro", libro);

    await fetch("Libros/ActualizarLibro", {
        method: "POST",
        body: form
    })

}