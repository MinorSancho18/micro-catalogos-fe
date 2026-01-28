// Clientes JavaScript

let tabla;
let modoEdicion = false;

$(document).ready(function () {
    cargarTabla();
});

function cargarTabla() {
    tabla = $('#tablaClientes').DataTable({
        ajax: {
            url: '/Clientes/Listar',
            dataSrc: '',
            error: function (xhr) {
                mostrarError('Error al cargar los datos', xhr);
            }
        },
        columns: [
            { data: 'idCliente' },
            { data: 'nombre' },
            { data: 'numeroCedula' },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-info" onclick="verDetalle(${row.idCliente})">
                            Ver
                        </button>
                        <button class="btn btn-sm btn-warning" onclick="editar(${row.idCliente})">
                            Editar
                        </button>
                        <button class="btn btn-sm btn-danger" onclick="eliminar(${row.idCliente})">
                            Eliminar
                        </button>
                    `;
                }
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
        }
    });
}

function abrirModalAgregar() {
    modoEdicion = false;
    $('#modalFormularioLabel').text('Agregar Cliente');
    $('#formCliente')[0].reset();
    $('#idCliente').val('');
    $('#modalFormulario').modal('show');
}

function editar(id) {
    modoEdicion = true;
    $('#modalFormularioLabel').text('Editar Cliente');

    $.ajax({
        url: `/Clientes/Obtener?id=${id}`,
        type: 'GET',
        success: function (data) {
            $('#idCliente').val(data.idCliente);
            $('#nombre').val(data.nombre);
            $('#numeroCedula').val(data.numeroCedula);
            $('#modalFormulario').modal('show');
        },
        error: function (xhr) {
            mostrarError('Error al obtener el cliente', xhr);
        }
    });
}

function verDetalle(id) {
    $.ajax({
        url: `/Clientes/Obtener?id=${id}`,
        type: 'GET',
        success: function (data) {
            $('#detalleId').text(data.idCliente);
            $('#detalleNombre').text(data.nombre);
            $('#detalleNumeroCedula').text(data.numeroCedula);
            $('#modalDetalle').modal('show');
        },
        error: function (xhr) {
            mostrarError('Error al obtener el cliente', xhr);
        }
    });
}

function guardarCliente() {
    const cliente = {
        idCliente: parseInt($('#idCliente').val()) || 0,
        nombre: $('#nombre').val(),
        numeroCedula: $('#numeroCedula').val()
    };

    const url = modoEdicion ? `/Clientes/Actualizar?id=${cliente.idCliente}` : '/Clientes/Crear';
    const type = modoEdicion ? 'PUT' : 'POST';

    $.ajax({
        url: url,
        type: type,
        contentType: 'application/json',
        data: JSON.stringify(cliente),
        success: function () {
            $('#modalFormulario').modal('hide');
            Swal.fire({
                icon: 'success',
                title: 'Éxito',
                text: modoEdicion ? 'Cliente actualizado correctamente' : 'Cliente creado correctamente'
            });
            tabla.ajax.reload();
        },
        error: function (xhr) {
            mostrarError('Error al guardar el cliente', xhr);
        }
    });
}

function eliminar(id) {
    Swal.fire({
        title: '¿Está seguro?',
        text: 'Esta acción no se puede revertir',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Clientes/Eliminar?id=${id}`,
                type: 'DELETE',
                success: function () {
                    Swal.fire({
                        icon: 'success',
                        title: 'Eliminado',
                        text: 'Cliente eliminado correctamente'
                    });
                    tabla.ajax.reload();
                },
                error: function (xhr) {
                    mostrarError('Error al eliminar el cliente', xhr);
                }
            });
        }
    });
}

function mostrarError(titulo, xhr) {
    let mensaje = 'Error desconocido';

    if (xhr.status === 400) {
        const errors = xhr.responseJSON?.errors || xhr.responseJSON;
        mensaje = formatearErroresValidacion(errors);
    } else if (xhr.status === 404) {
        mensaje = 'Registro no encontrado';
    } else if (xhr.status === 409) {
        mensaje = 'El registro ya existe';
    } else if (xhr.status === 500) {
        mensaje = xhr.responseJSON?.message || 'Error en el servidor';
    }

    Swal.fire({
        icon: 'error',
        title: titulo,
        text: mensaje
    });
}

function formatearErroresValidacion(errors) {
    if (typeof errors === 'string') {
        return errors;
    }

    let mensajes = [];
    for (let campo in errors) {
        if (Array.isArray(errors[campo])) {
            mensajes.push(...errors[campo]);
        } else {
            mensajes.push(errors[campo]);
        }
    }
    return mensajes.join(', ');
}
