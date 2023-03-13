function valid() {
    var result = false;
    alert(result);
    var brand = document.getElementById("Brand");
    var modelname = document.getElementById("ModelName");
    var size = document.getElementById("Size");
    var expecteddelivery = document.getElementById("ExpectedDelivery");
    var cod = document.getElementById("COD");
    var vendorcost = document.getElementById("VendorCost");
    var sellingprice = document.getElementById("SellingPrice");
    var seller = document.getElementById("Seller");
    var importedfrom = document.getElementById("ImportedFrom");
    var warehouselocation = document.getElementById("WarehouseLocation");

    if (brand.value.trim() == "") {
        brand.style.border = "solid 1px red";
        document.getElementById("brand").style.visibility = "visible";

    }

    if (modelname.value.trim() == "") {
        brand.style.border = "solid 1px red";
        document.getElementById("modelname").style.visibility = "visible";
    }

    if (size.value.trim() == "") {
        brand.style.border = "solid 1px red";
        document.getElementById("size").style.visibility = "visible";
    }

    if (expecteddelivery.value.trim()== "") {
        brand.style.border = "solid 1px red";
        document.getElementById("expecteddelivery").style.visibility = "visible";
    }

    if (cod.value.trim() == "") {
        brand.style.border = "solid 1px red";
        document.getElementById("cod").style.visibility = "visible";
    }

    if (vendorcost.value == 0) {
        brand.style.border = "solid 1px red";
        document.getElementById("vendorcost").style.visibility = "visible";
    }

    if (sellingprice.value == 0) {
        brand.style.border = "solid 1px red";
        document.getElementById("sellingprice").style.visibility = "visible";
    }

    if (seller.value.trim() == "") {
        brand.style.border = "solid 1px red";
        document.getElementById("seller").style.visibility = "visible";
    }

    if (importedfrom.value.trim() == "") {
        brand.style.border = "solid 1px red";
        document.getElementById("importedfrom").style.visibility = "visible";
    }

    if (warehouselocation.value.trim() == "") {
        brand.style.border = "solid 1px red";
        document.getElementById("warehouselocation").style.visibility = "visible";
    }
    if (brand.value.trim() == "" || modelname.value.trim() == "" || size.value.trim() == "" || expecteddelivery.value.trim() == "" || cod.value.trim() == "" || vendorcost.value == 0 || sellingprice.value == 0 || seller.value.trim() == "" || importedfrom.value.trim() == "" || warehouselocation.value.trim()=="") {
        result= false;
    }
    else {
        result= true;
    }
    alert("Fn completed" + result);
    return result;

}

function admin() {
    var adminemail = document.getElementById("AdminEmail")
        
    var adminpassword = document.getElementById("AdminPassword")

    if (adminemail.value.trim() == "") {
        adminemail.style.border = "solid 1px red";
        document.getElementById("adminemail").style.visibility = "visible";
    }

    if (adminpassword.value.trim() == "") {
        adminpassword.style.border = "solid 1px red";
        document.getElementById("adminpassword").style.visibility = "visible";
    }
    if (adminemail.value.trim() == "" || adminpassword.value.trim() == "") {
        return false;

    }
    else {
        return true;
    }
}

function checkemail() {
    var email = $("#AdminEmail").val();
    $.ajax({
        type: "POST",
        url: "/Product/CheckEmailAvailability",
        data: '{adminemail:"' + email + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (TotalCount) {
            var message = $("#spanmsg");
            if (TotalCount) {
                message.html("Email ID is not available");
                message.css("color", "red");
            }
            else {
                message.html("Email ID is available");
                message.css("color", "green");
            }
        }
    });
}

function deleteid(url)
{
    var id = $("#ProductID").val();
    $.ajax({
        type: "POST",
        url: "/Product/DeleteInventoryItem",
        data: 
        sucess:
    })

    
}