//variables
var table;
var seleccion;
//Cargar al inicio
$(document).ready(function () { 
    var t = $("#comboTipo").val().toString();
    if (t == "Tipo Servicio") {
        //Ocultar
        $('#divTablaSolicitudes').addClass('invisible');
        $('#divTipoServicio').addClass('invisible');
        $('#divTipoSolicitud').addClass('invisible');
        $('#tablaSolicitudes').DataTable().destroy();
        //Mostrar
        $('#divTablasTipo').removeClass('invisible');
        $('#divTablaServicios').removeClass('invisible');
        document.getElementById('titulo').innerHTML = "Tipos de Servicio";
        //Cargar tabla
        table = $('#tablaServicios').DataTable()
    }
    else if (t == "Tipo Solicitud") {
        //Ocultar
        $('#divTablaServicios').addClass('invisible');
        $('#divTipoServicio').addClass('invisible');
        $('#divTipoSolicitud').addClass('invisible');
        $('#tablaServicios').DataTable().destroy();
        //Mostrar
        $('#divTablasTipo').removeClass('invisible');
        $('#divTablaSolicitudes').removeClass('invisible');
        document.getElementById('titulo').innerHTML = "Tipos de Solicitud";
        //Cargar tabla
        table = $('#tablaSolicitudes').DataTable()
    }
});

//Selecionar tipo 
$("#comboTipo").change(function () {
    var t = $("#comboTipo").val().toString();
    if (t == "Tipo Servicio") {
        //Ocultar
        $('#divTablaSolicitudes').addClass('invisible');
        $('#divTipoServicio').addClass('invisible');
        $('#divTipoSolicitud').addClass('invisible');
        $('#tablaSolicitudes').DataTable().destroy();
        //Mostrar
        $('#divTablasTipo').removeClass('invisible');
        $('#divTablaServicios').removeClass('invisible');
        document.getElementById('titulo').innerHTML = "Tipos de Servicio";
        //Cargar tabla
        table = $('#tablaServicios').DataTable()
    }
    else if (t == "Tipo Solicitud") {
        //Ocultar
        $('#divTablaServicios').addClass('invisible');
        $('#divTipoServicio').addClass('invisible');
        $('#divTipoSolicitud').addClass('invisible');
        $('#tablaServicios').DataTable().destroy();
        //Mostrar
        $('#divTablasTipo').removeClass('invisible');
        $('#divTablaSolicitudes').removeClass('invisible');
        document.getElementById('titulo').innerHTML = "Tipos de Solicitud";
        //Cargar tabla
        table = $('#tablaSolicitudes').DataTable()
    }

});

//Seleccionar Objeto
$('.table tbody').on('click', 'tr', function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
        $('#btnUpdateTipoSolicitud').addClass('invisible');
        $('#btnUpdateTipoServicio').addClass('invisible');
    } else {
        table.$('tr.selected').removeClass('selected');
        $(this).addClass('selected');
        if ($('#divTablaSolicitudes').hasClass('invisible')) {
            $('#btnUpdateTipoServicio').removeClass('invisible');
            $('#btnUpdateTipoSolicitud').addClass('invisible');
        }
        else if ($('#divTablaServicios').hasClass('invisible')) {
            $('#btnUpdateTipoSolicitud').removeClass('invisible');
            $('#btnUpdateTipoServicio').addClass('invisible');
        }

    }
    var fila = table.row('.selected').data();
    seleccion = fila;
});

//CrearTipoServicio
$('#btnCreateTipoServicio').click(function () {
    //Ocultar
    $('#divTablasTipo').addClass('invisible');
    $('#tablaServicios').DataTable().destroy();
    $('#tablaSolicitudes').DataTable().destroy();
    //Mostrar
    $('#createTipoServicio').removeClass('invisible');
    $('#divTipoServicio').removeClass('invisible');
    document.getElementById('titulo').innerHTML = "Crear Tipo de Servicio";
});
//ModificarTipoServicio
$('#btnUpdateTipoServicio').click(function () {
    if (seleccion != null) {
        //Ocultar
        $('#divTablasTipo').addClass('invisible');
        $('#tablaServicios').DataTable().destroy();
        $('#tablaSolicitudes').DataTable().destroy();
        //Mostrar
        $('#updateTipoServicio').removeClass('invisible');
        $('#divTipoServicio').removeClass('invisible');
        $('#inputIdTipoServicio').val(seleccion[0]);
        $('#inputNombreTipoServicio').val(seleccion[1]);
        $('#inputValorTipoServicio').val(seleccion[2]);
        if (seleccion[3] == "Deshabilitado") {
            $('#comboEstadoTipoServicio').val(0);
        }
        else if (seleccion[3] == "Habilitado") {
            $('#comboEstadoTipoServicio').val(1);
        }

        $('#inputDescripcionTipoServicio').val(seleccion[4]);
        $('#inpuTest').val($('#comboEstadoTipoServicio').val());
        document.getElementById('titulo').innerHTML = "Modificar Tipo de Solicitud";
    }
    else {
        alert("Selecciona lo que deseas modificar");
    }
});

$('#btnUpdateTipoSolicitud').click(function () {
    if (seleccion != null) {
        //Ocultar
        $('#divTablasTipo').addClass('invisible');
        $('#tablaServicios').DataTable().destroy();
        $('#tablaSolicitudes').DataTable().destroy();
        //Mostrar
        $('#updateTipoSolicitud').removeClass('invisible');
        $('#divTipoSolicitud').removeClass('invisible');
        $('#inputIdTipoSolicitud').val(seleccion[0]);
        $('#inputNombreTipoSolicitud').val(seleccion[1]);
        if (seleccion[2] == "Deshabilitado") {
            $('#comboEstadoTipoSolicitud').val(0);
        }
        else if (seleccion[2] == "Habilitado") {
            $('#comboEstadoTipoSolicitud').val(1);
        }
        $('#inputDescripcionTipoSolicitud').val(seleccion[3]);
        document.getElementById('titulo').innerHTML = "Modificar Tipo de Solicitud";
    }
    else {
        alert("Selecciona lo que deseas modificar");
    }
});
$('#comboEstadoTipoServicio').change(function () {
    $('#inpuTest').val($('#comboEstadoTipoServicio').val());
});
$('.btnCancel').click(function () {
    var t = $("#comboTipo").val().toString();
    if (t == "Tipo Servicio") {
        //Ocultar
        $('#divTablaSolicitudes').addClass('invisible');
        $('#divTipoServicio').addClass('invisible');
        $('#divTipoSolicitud').addClass('invisible');
        $('#tablaSolicitudes').DataTable().destroy();
        //Mostrar
        $('#divTablasTipo').removeClass('invisible');
        $('#divTablaServicios').removeClass('invisible');
        document.getElementById('titulo').innerHTML = "Tipos de Servicio";
        //Cargar tabla
        table = $('#tablaServicios').DataTable()
    }
    else if (t == "Tipo Solicitud") {
        //Ocultar
        $('#divTablaServicios').addClass('invisible');
        $('#divTipoServicio').addClass('invisible');
        $('#divTipoSolicitud').addClass('invisible');
        $('#tablaServicios').DataTable().destroy();
        //Mostrar
        $('#divTablasTipo').removeClass('invisible');
        $('#divTablaSolicitudes').removeClass('invisible');
        document.getElementById('titulo').innerHTML = "Tipos de Solicitud";
        //Cargar tabla
        table = $('#tablaSolicitudes').DataTable()
    }
});


