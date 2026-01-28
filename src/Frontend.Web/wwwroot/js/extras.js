// Extras JavaScript

let tabla;
let modoEdicion = false;

$(document).ready(function () {
    cargarTabla();
});

function cargarTabla() {
    tabla = $('#tablaExtras').DataTable({
        ajax: {
            url: '/Extras/Listar',
            dataSrc: '',
            error: function (xhr) {
                mostrarError('Error al cargar los datos', xhr);
            }
        },
        columns: [
            { data: 'idExtra' },
            { data: 'descripcion' },
            {
                data: 'costo',
                render: function (data) {
                    return `₡${parseFloat(data).toFixed(2)}`;
                }
            },
            {
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-info" onclick="verDetalle(${row.idExtra})">
                            Ver
                        </button>
                        <button class="btn btn-sm btn-warning" onclick="editar(${row.idExtra})">
                            Editar
                        </button>
                        <button class="btn btn-sm btn-danger" onclick="eliminar(${row.idExtra})">
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
    $('#modalFormularioLabel').text('Agregar Extra');
    $('#formExtra')[0].reset();
    $('#idExtra').val('');
    $('#modalFormulario').modal('show');
}

function editar(id) {
    modoEdicion = true;
    $('#modalFormularioLabel').text('Editar Extra');

    $.ajax({
        url: `/Extras/Obtener?id=${id}`,
        type: 'GET',
        success: function (data) {
            $('#idExtra').val(data.idExtra);
            $('#descripcion').val(data.descripcion);
            $('#costo').val(data.costo);
            $('#modalFormulario').modal('show');
        },
        error: function (xhr) {
            mostrarError('Error al obtener el extra', xhr);
        }
    });
}

function verDetalle(id) {
    $.ajax({
        url: `/Extras/Obtener?id=${id}`,
        type: 'GET',
        success: function (data) {
            $('#detalleId').text(data.idExtra);
            $('#detalleDescripcion').text(data.descripcion);
            $('#detalleCosto').text(`₡${parseFloat(data.costo).toFixed(2)}`);
            $('#modalDetalle').modal('show');
        },
        error: function (xhr) {
            mostrarError('Error al obtener el extra', xhr);
        }
    });
}

function guardarExtra() {
    const extra = {
        idExtra: parseInt($('#idExtra').val()) || 0,
        descripcion: $('#descripcion').val(),
        costo: parseFloat($('#costo').val())
    };

    const url = modoEdicion ? `/Extras/Actualizar?id=${extra.idExtra}` : '/Extras/Crear';
    const type = modoEdicion ? 'PUT' : 'POST';

    $.ajax({
        url: url,
        type: type,
        contentType: 'application/json',
        data: JSON.stringify(extra),
        success: function () {
            $('#modalFormulario').modal('hide');
            Swal.fire({
                icon: 'success',
                title: 'Éxito',
                text: modoEdicion ? 'Extra actualizado correctamente' : 'Extra creado correctamente'
            });
            tabla.ajax.reload();
        },
        error: function (xhr) {
            mostrarError('Error al guardar el extra', xhr);
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
                url: `/Extras/Eliminar?id=${id}`,
                type: 'DELETE',
                success: function () {
                    Swal.fire({
                        icon: 'success',
                        title: 'Eliminado',
                        text: 'Extra eliminado correctamente'
                    });
                    tabla.ajax.reload();
                },
                error: function (xhr) {
                    mostrarError('Error al eliminar el extra', xhr);
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
