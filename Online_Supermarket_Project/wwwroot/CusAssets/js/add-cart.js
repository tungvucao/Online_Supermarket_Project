$(document).ready(function () {
    $('body').on('click', '.add-to-cart', function (e) {
        e.preventDefault();
        var productid = $(this).data('id');
        var quantity = 1;
        var quan2 = $('#txtquantity').val();
        if (quan2 > 1) {
            quantity = quan2;
        }
        $.ajax({
            url: '/api/cart/add',
            datatype: "JSON",
            type: "POST",
            data: {
                productID: productid,
                amount: quantity,
            },
            success: function (response) {
                loadHeaderCart();
                location.reload();
            },
            error: function (error) {
                alert("Error: " + error.responseText);
            }
        });
    });

    function loadHeaderCart() {
        $("#miniCart").load("/AjaxContent/HeaderCart");
        $("#numberCart").load("/AjaxContent/NumberCart");
    }
});