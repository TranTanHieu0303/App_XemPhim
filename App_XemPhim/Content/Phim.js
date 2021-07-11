function chonnuoc() {
    let selectbox = document.getElementById('Nuoc');
    var selecttext = selectbox.options[selectbox.selectedIndex].text;
    console.log(selecttext);
    var selectValue = selectbox.value;
    let inp = document.getElementById('Tendn');
    $('#Tendn').val(selecttext);
    inp.setAttribute('data-province', selectValue);
}
function AdDM() {
    var mTEN = $('#TenDM').val();
    $.ajax(
        {
            url: "/Home/AddDanhMuc",
            data: JSON.stringify({TEN:mTEN}),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == 1)
                    confirm('Thêm thành công');
                else
                    confirm('Thêm không thành công');
                location.reload()

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
}
function AddTL() {
    var mTEN = $('#TenTL').val();
    $.ajax(
        {
            url: "/Home/AddTheLoai",
            data: JSON.stringify({ TEN: mTEN }),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == 1)
                    confirm('Thêm thành công');
                else
                    confirm('Thêm không thành công');
                location.reload()

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });

}
function AddQG() {
    var mTEN = $('#TenQG').val();
    $.ajax(
        {
            url: "/Home/AddQuocGia",
            data: JSON.stringify({ TEN: mTEN }),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result == 1)
                    confirm('Thêm thành công');
                else
                    confirm('Thêm không thành công');
                location.reload()

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });

}
function AddPhim() {
    var frm = $('#frm_AddPhim').serialize("DM");
    console.log(frm);
    var TenP = $('#TenP').val();
    var MoTa = $('#MoTa').val();
    var DienVien = $('#DienVien').val();
    var ThoiLuong = $('#ThoiLuong').val();
    var Sotap = $('#Sotap').val();
    var Trailer = $('#Trailer').val();
    var LinkP = $('#LinkP').val();
    var fileup = $('#Hinh').get(0);
    var files = fileup.files;
    var formdata = new FormData();
    formdata.append(files[0].name, files[0]);
    formdata.append("form", frm);
    formdata.append("TenPhim", TenP);
    formdata.append("MoTa", MoTa);
    formdata.append("DienVien", DienVien);
    formdata.append("Thoiluong", ThoiLuong);
    formdata.append("SoTap", Sotap);
    formdata.append("TraiLer", Trailer);
    formdata.append("LinkP", LinkP);

    $.ajax(
        {
            url: "/Home/AddPhimAsync",
            data: formdata,
            type: "POST",
            contentType: false,
            processData: false,
            dataType: "json",
            success: function (result) {
                if (result == 1)
                    confirm('Thêm thành công');
                else
                    confirm('Thêm không thành công');
                location.reload()

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }

        });
}