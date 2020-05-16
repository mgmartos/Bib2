//<script type="text/javascript">
    $(document).ready(function () {

        //Función hover, sus parámetros son dos funciones, una para la entrada del ratón u una para la salida del ratón.
        //         En todas las tablas se ilumina la fila sobre la que pasa el ratón.
        $(function () {
            $("table td").hover(
                function () {
                    $("td", $(this).closest("tr")).addClass("hover_row");
                },
                function () {
                    $("td", $(this).closest("tr")).removeClass("hover_row");
                }
            );

        });

    //         En todas las tablas pone las filas pares con un color de fondo y las impares con otro
    $('table').find('tr:even').css({'background-color': '#f5f5f5' })
            .end().find('tr:odd').css({ 'background-color': '#add8e6' });



        // function que adorna la transición en anclas

        //$('a[href*=#]').click(function () {
        //    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '')
        //        && location.hostname == this.hostname) {
        //        var $target = $(this.hash);
        //        $target = $target.length && $target || $('[name=' + this.hash.slice(1) + ']');
        //       if ($target.length) {
        //            var targetOffset = $target.offset().top;
        //            $('html,body').animate({ scrollTop: targetOffset }, 1000);
        //            return false;
        //        }
        //    }
        //});


           });                       // fin de Ready

    //</script>

