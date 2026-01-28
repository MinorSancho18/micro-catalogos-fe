// Vehiculos JavaScript

let tabla;
let modoEdicion = false;
let categorias = [];

$(document).ready(function () {
    cargarCategorias();
    cargarTabla();
});

function cargarCategorias() {
    $.ajax({
        url: '/Vehiculos/ObtenerCategorias',
        type: 'GET',
        success: function (data) {
            categorias = data;
        },
        error: function (xhr) {
            mostrarError('Error al cargar las categorías', xhr);
        }
    });
}

function cargarTabla() {
    tabla = $('#tablaVehiculos').DataTable({
        ajax: {
            url: '/Vehiculos/Listar',
            dataSrc: '',
            error: function (xhr) {
                mostrarError('Error al cargar los datos', xhr);
            }
        },
        columns: [
            { data: 'idVehiculo' },
            { data: 'descripcion' },
            {
                data: null,
                render: function (data, type, row) {
                    return row.categoriaDescripcion || '';
                }
            },
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
                        <button class="btn btn-sm btn-info" onclick="verDetalle(${row.idVehiculo})">
                            Ver
                        </button>
                        <button class="btn btn-sm btn-warning" onclick="editar(${row.idVehiculo})">
                            Editar
                        </button>
                        <button class="btn btn-sm btn-danger" onclick="eliminar(${row.idVehiculo})">
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
    $('#modalFormularioLabel').text('Agregar Vehículo');
    $('#formVehiculo')[0].reset();
    $('#idVehiculo').val('');
    llenarComboCategoria();
    $('#modalFormulario').modal('show');
}

function llenarComboCategoria(idSeleccionado = null) {
    const select = $('#idCategoria');
    select.empty();
    select.append('<option value="">Seleccione una categoría...</option>');

    categorias.forEach(cat => {
        const option = $('<option></option>')
            .attr('value', cat.idCategoria)
            .text(`${cat.codigo} - ${cat.descripcion}`);

        if (idSeleccionado && cat.idCategoria === idSeleccionado) {
            option.prop('selected', true);
        }

        select.append(option);
    });
}

function editar(id) {
    modoEdicion = true;
    $('#modalFormularioLabel').text('Editar Vehículo');

    $.ajax({
        url: `/Vehiculos/Obtener?id=${id}`,
        type: 'GET',
        success: function (data) {
            $('#idVehiculo').val(data.idVehiculo);
            $('#descripcion').val(data.descripcion);
            $('#costo').val(data.costo);
            llenarComboCategoria(data.idCategoria);
            $('#modalFormulario').modal('show');
        },
        error: function (xhr) {
            mostrarError('Error al obtener el vehículo', xhr);
        }
    });
}

function verDetalle(id) {
    $.ajax({
        url: `/Vehiculos/Obtener?id=${id}`,
        type: 'GET',
        success: function (data) {
            $('#detalleId').text(data.idVehiculo);
            $('#detalleDescripcion').text(data.descripcion);
            $('#detalleCategoria').text(data.categoriaDescripcion || 'N/A');
            $('#detalleCosto').text(`₡${parseFloat(data.costo).toFixed(2)}`);
            $('#modalDetalle').modal('show');
        },
        error: function (xhr) {
            mostrarError('Error al obtener el vehículo', xhr);
        }
    });
}

function guardarVehiculo() {
    const vehiculo = {
        idVehiculo: parseInt($('#idVehiculo').val()) || 0,
        descripcion: $('#descripcion').val(),
        idCategoria: parseInt($('#idCategoria').val()),
        costo: parseFloat($('#costo').val())
    };

    const url = modoEdicion ? `/Vehiculos/Actualizar?id=${vehiculo.idVehiculo}` : '/Vehiculos/Crear';
    const type = modoEdicion ? 'PUT' : 'POST';

    $.ajax({
        url: url,
        type: type,
        contentType: 'application/json',
        data: JSON.stringify(vehiculo),
        success: function () {
            $('#modalFormulario').modal('hide');
            Swal.fire({
                icon: 'success',
                title: 'Éxito',
                text: modoEdicion ? 'Vehículo actualizado correctamente' : 'Vehículo creado correctamente'
            });
            tabla.ajax.reload();
        },
        error: function (xhr) {
            mostrarError('Error al guardar el vehículo', xhr);
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
                url: `/Vehiculos/Eliminar?id=${id}`,
                type: 'DELETE',
                success: function () {
                    Swal.fire({
                        icon: 'success',
                        title: 'Eliminado',
                        text: 'Vehículo eliminado correctamente'
                    });
                    tabla.ajax.reload();
                },
                error: function (xhr) {
                    mostrarError('Error al eliminar el vehículo', xhr);
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
