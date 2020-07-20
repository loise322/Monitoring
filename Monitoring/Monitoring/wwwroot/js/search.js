$(document).ready(function() {
    $('#inputSearch').on('input', function() {
        let value = $('#inputSearch').val();
        let length = value.length;
        $('.table-body-tr').each(function() {    
            $('.col-my').each(function () {
                if ($(this).hasClass('nameMetric')) {
                    if ($(this).text().substr(0, length) != value) {
                        $(this).parent().css("display", "none");
                    }
                    if ($(this).text().substr(0, length) == value && $(this).parent().attr("style") == "display: none;") {
                        $(this).parent().css("display", "flex");
                    }    
                }
            }) 
        }) 
    })
})