// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // Thêm vào giỏ hàng
    $(".add-to-cart").click(function () {
        var bookId = $(this).data("id");

        $.post("/Cart/AddToCart", { bookId: bookId }, function (response) {
            if (response.success) {
                updateCartCount(response.cartCount);
            }
        });
    });

    // Tăng số lượng
    $(document).on("click", ".btn-increase", function () {
        var bookId = $(this).data("id");

        $.post("/Cart/IncreaseQuantity", { bookId: bookId }, function (response) {
            if (response.success) {
                $("#cart-body").html(response.cartHtml);
                updateCartCount(response.cartCount);
            }
        });
    });

    // Giảm số lượng
    $(document).on("click", ".btn-decrease", function () {
        var bookId = $(this).data("id");

        $.post("/Cart/DecreaseQuantity", { bookId: bookId }, function (response) {
            if (response.success) {
                $("#cart-body").html(response.cartHtml);
                updateCartCount(response.cartCount);
            }
        });
    });

    // Xóa sản phẩm
    $(document).on("click", ".btn-remove", function () {
        var bookId = $(this).data("id");

        $.post("/Cart/RemoveItem", { bookId: bookId }, function (response) {
            if (response.success) {
                $("#cart-body").html(response.cartHtml);
                updateCartCount(response.cartCount);
            }
        });
    });

    // Hàm cập nhật số lượng sản phẩm trong badge
    function updateCartCount(count) {
        const badge = $("#cart-count");
        badge.text(count);
        if (count === 0) {
            badge.hide();
        } else {
            badge.show();
        }
    }
});