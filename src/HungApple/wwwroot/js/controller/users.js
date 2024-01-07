var users = {
    init: function () {
        $('#sltProvinceId').on('change', function () {
            $.ajax({
                url: '/Users/GetListDistrictsByProvincesId',
                type: 'get',
                dataType: 'json',
                data: {
                    provincesId: $(this).val()
                },
                success: function (res) {
                    var html = '<option value="0">--Chọn Quận/Huyện--</option>';
                    if (res.status) {
                        if (res.data != null && res.data.length > 0) {
                            for (var i = 0; i < res.data.length; i++) {
                                html += '<option value="' + res.data[i].district_id + '">' + res.data[i].district_name + '</option>'
                            }
                        }
                    }
                    $('#sltDistrictId').empty();
                    $('#sltDistrictId').append(html);
                }
            })
        });
        $('#sltDistrictId').on('change', function () {
            $.ajax({
                url: '/Users/GetListWardByDistrictId',
                type: 'get',
                dataType: 'json',
                data: {
                    districtId: $(this).val()
                },
                success: function (res) {
                    var html = '<option value="0">--Chọn Xã/Phường--</option>';
                    if (res.status) {
                        if (res.data != null && res.data.length > 0) {
                            for (var i = 0; i < res.data.length; i++) {
                                html += '<option value="' + res.data[i].ward_id + '">' + res.data[i].ward_name + '</option>'
                            }
                        }
                    }
                    $('#sltWardId').empty();
                    $('#sltWardId').append(html);
                }
            })
        });

    }
}
$(document).ready(function () {
    users.init();
});