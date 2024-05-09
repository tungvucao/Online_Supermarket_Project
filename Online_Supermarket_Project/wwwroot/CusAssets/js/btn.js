document.addEventListener("DOMContentLoaded", function () {
    var plusBtn = document.querySelector('.cart-plus-minuss .qtybutton.plus');
    var minusBtn = document.querySelector('.cart-plus-minuss .qtybutton.minus');
    var quantityInput = document.querySelector('.cart-plus-minuss .qty');

    plusBtn.addEventListener('click', function () {
        var currentValue = parseInt(quantityInput.value);
        quantityInput.value = currentValue + 1;
    });

    minusBtn.addEventListener('click', function () {
        var currentValue = parseInt(quantityInput.value);
        if (currentValue > 1) {
            quantityInput.value = currentValue - 1;
        }
    });
});