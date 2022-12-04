
$(document).ready(function () {
    $('#divTareas').find('div').each(function () {
        var idCheck = "chk_" + $(this).attr('id');
        var idComentario = "comment_" + $(this).attr('id');
        var estado = "estado_" + $(this).attr('id');
        comentario = document.getElementById(idComentario)
        checkbox = document.getElementById(idCheck)

        if (checkbox.checked == true) {
            comentario.removeAttribute("disabled");

        }
        else {
            comentario.setAttribute("disabled", "disabled");
            $(comentario).val('');
        }

    });
});
$('.checkInput').click(function () {
        
    $('#divTareas').find('div').each(function () {
        var idCheck = "chk_" + $(this).attr('id');
        var idComentario = "comment_" + $(this).attr('id');
        var estado = "estado_" + $(this).attr('id');
        comentario = document.getElementById(idComentario)
        checkbox = document.getElementById(idCheck)

        if (checkbox.checked == true) {
            comentario.removeAttribute("disabled");
        }
        else {
            comentario.setAttribute("disabled", "disabled");
            $(comentario).val('')
        }
                
    });
});