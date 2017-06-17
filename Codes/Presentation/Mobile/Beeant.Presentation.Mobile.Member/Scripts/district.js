function InitDistrict(districts,districtId) {
    var selectProvince = $("#ddlProvince");
    var selectCity = $("#ddlCity");
    var selectDistrict = $("#ddlCounty");

    
    function loadDistrict(ctrl, parentId) { //加载区域
        $(districts).each(function(index, value) {
            if (value.ParentId == parentId) {
                ctrl.append("<option value='" + value.Id + "'>" + value.Name + "</option>");
            }
        });
    }
    function initializtion(typeName) {
        if (typeName == 'All') {
            selectProvince.empty();
            selectProvince.append("<option value=''>请选择</option>");
            selectCity.empty();
            selectCity.append("<option value=''>请选择</option>");
            selectDistrict.empty();
            selectDistrict.append("<option value=''>请选择</option>");
        }
        else  if (typeName == 'Province') {
            selectCity.empty();
            selectCity.append("<option value=''>请选择</option>");
            selectDistrict.empty();
            selectDistrict.append("<option value=''>请选择</option>");
        }
        else  if (typeName == 'City') {
            selectDistrict.empty();
            selectDistrict.append("<option value=''>请选择</option>");
        }
    }

    initializtion('All');
    loadDistrict(selectProvince, 0);
    var cityid,provinceId;
    if(Number(districtId)>0) {
        $(districts).each(function(index, value) {
            if (value.Id == districtId) {
                cityid = value.ParentId;
                return false;
            }
        });
            
        $(districts).each(function(index, value) {
            if (value.Id == cityid) {
                provinceId = value.ParentId;
                return false;
            }
        });
            
        loadDistrict(selectDistrict, cityid);
        selectDistrict.val(districtId);
        loadDistrict(selectCity, provinceId);
        selectCity.val(cityid);
        selectProvince.val(provinceId);
    }

    selectProvince.change(function() {
        if ($(this).val() != "") {
            initializtion('Province');
            loadDistrict(selectCity, $(this).val());
        } else {
            selectCity.empty();
            selectDistrict.empty();
        }
    });
    selectCity.change(function() {
        if ($(this).val() != "") {
            initializtion('City');
            loadDistrict(selectDistrict, $(this).val());
        } else {
            selectDistrict.empty();
        }
    });
}

