(function() {
    $(function() {
        crudMS.options.service = abp.services.app.preRoute;
        crudMS.masterField = "#routeTypeId";        
        crudMS.depotId = $('#depotId').val();

        crudMS.parentField = '#preRouteId';
        crudMS.parentValue = 0;
        crudMS.enableEdit = function () {
            isEnableEdit = true;
            $('#dlgActivate').dialog('open');
            $('#Password').textbox('setValue','');           
        }

        // set editEnabled is false when internal 30 * 10 = 300s(5min)
        var tm = window.setInterval("crudMS.editDisabledCount()", 30000);

        // ui
        abp.services.app.combo.getVehicleItems(crudMS.depotId).done(function (data) {
            $('#vehicleId').combobox({
                data: data,
                valueField: 'id',
                textField: 'displayText'
            })
        });

        abp.services.app.combo.getOutletItems(crudMS.depotId).done(function (data) {
            $('#outletId').combobox({
                data: data,
                valueField: 'id',
                textField: 'displayText'
            })
        });

        $.getComboItems('#taskTypeId', 'TaskType', 'Id', 'Cn', 'Name');
        
        $('#type').combobox({
            onChange: onRouteTypeChanged
        })

        $('#dg').datagrid({
            onSelect: function (index, row) {
                crudMS.parentValue = row.id;
                $('#dgSon1').datagrid({
                    url: 'PreRoutes/RouteTaskGridData/' + row.id 
                })
           }
        })

        // Buttons click event
        $('#dlgActivate-buttons').children('a[name="validFinger"]').click(function (e) {
            validFinger();
        });

        $('#dlgActivate-buttons').children('a[name="validPassword"]').click(function (e) {
            validPassword();
        });

        onRouteTypeChanged();
    });

    // contentChange
    function onRouteTypeChanged()
    {
        crudMS.masterValue = $('#type').val();
        var params = [];
        params.push({name: 'depotId', value: crudMS.depotId});        
        params.push({name: 'routeTypeId', value: crudMS.masterValue});        
        var qs = abp.utils.buildQueryString(params);

        $('#dg').datagrid({
            url: 'PreRoutes/GridData' + qs
        });

        crudMS.selectRow();
    };

    function validFinger()
    {
        fmActivate.ZAZFingerActivex.spDeviceType = 2;
        fmActivate.ZAZFingerActivex.spComPort = 1;
        fmActivate.ZAZFingerActivex.spBaudRate = 6;
        fmActivate.ZAZFingerActivex.CharLen = 512;
        fmActivate.ZAZFingerActivex.FingerCode = "";
        fmActivate.ZAZFingerActivex.TimeOut = 7;
        fmActivate.ZAZFingerActivex.ZAZSetIMG(256, 288);
        var mesg = fmActivate.ZAZFingerActivex.ZAZGetImgCode();
        if (mesg == "0") {
            var dst = fmActivate.ZAZFingerActivex.FingerCode.substring(0, 512);
            var spResult = fmActivate.ZAZFingerActivex.ZAZMatch(fmActivate.Finger.value, dst);
            abp.notify.info("验证分值为: " + spResult);
            if (spResult > 52)
                if (isEnableEdit == false) 
                    activeTasks('激活');
                else {
                    crudMS.options.editEnabled = true;
                    crudMS.options.tmCount = 0;
                    abp.notify.info("操作授权成功")
                    $('#dlgActivate').dialog('close');
                }
            else
                abp.notify.error("指纹验证错误");
        }
    }

    function validPassword()
    {
        var operatePassword = fmActivate.OperatePassword.value;
        var deadline = fmActivate.PwdDeadline.value;

        if ( operatePassword == null || operatePassword.length === 0) {
            abp.notify.error("请先设置用户操作认证密码");
            return;
        }

        if ( operatePassword != $('#Password').val() )  {
            abp.notify.error("密码错误");
            return;
        }

        if (isEnableEdit == false) 
            activeTasks('密活');
        else {
            crudMS.options.editEnabled = true;
            crudMS.options.tmCount = 0;
            abp.notify.info("操作授权成功")
            $('#dlgActivate').dialog('close');
        }
    }

})();