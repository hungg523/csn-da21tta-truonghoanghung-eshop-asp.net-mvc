var products = {
    init: function () {
        $('#qty-up').click(function () {
            var quantity = parseInt($('#ipQuantity').val());
            $('#ipQuantity').val(quantity);
            $('#ipQuantityPayment').val(quantity);
        });
        $('#qty-down').click(function () {
            var quantity = parseInt($('#ipQuantity').val());         
            $('#ipQuantity').val(quantity);
            $('#ipQuantityPayment').val(quantity);
        });
    }
}
$(document).ready(function () {
    products.init();
});