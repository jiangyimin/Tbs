(function() {
    var routeTypeId;
    var isEnableEdit = false;
    var typelist;
    $(function() {
        crudMS.options.service = abp.services.app.route;
        crudMS.masterField = "#carryoutDate";
        crudMS.depotId = $('#depotId').val();

        crudMS.parentField = '#routeId';
        crudMS.parentValue = 0;

        crudMS.enableEdit = function () {
            isEnableEdit = true;
            $('#dlgActivate').dialog('open');
            $('#Password').textbox('setValue','');           
        }

        // set editEnabled is false when internal 15 * 10 = 150s(2.5min)
        var tm = window.setInterval("crudMS.editDisabledCount()", 15000);

        typelist = abp.setting.get('WorkFlow.TaskTypeListForRoutesFrom');

        $('#dd').datebox({
            onChange: onDateChanged
        })

        $('#dg').datagrid({
            onSelect: function (index, row) {
                crudMS.parentValue = row.id;

                if (row.routeTypeId != routeTypeId) {
                    routeTypeId = row.routeTypeId;
                    abp.services.app.combo.getRouteRoleItems(routeTypeId).done(function (data) {
                        $('#routeRoleId').combobox({
                            data: data,
                            valueField: 'id',
                            textField: 'name'
                        })
                    })
                }

                $('#dgSon1').datagrid({
                    url: 'WorkersGridData/' + row.id 
                })
                $('#dgSon2').datagrid({
                    url: 'TasksGridData/' + row.id 
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
        
        $('#tbSon2').children('a[name="routeIdentify"]').click(function (e) {
            showRouteIdentify();
        });

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

        abp.services.app.combo.getWorkerItems(crudMS.depotId).done(function (data) {
            $('#workerId').combobox({
                data: data,
                valueField: 'id',
                textField: 'displayText'
            })
        });

        $.getComboItems('#taskTypeId', 'TaskType', 'Id', 'Cn', 'Name');

        $.getComboItems('#routeTypeId', 'RouteType', 'Id', 'Name').done(function (data) {
            onDateChanged();
        });
    });

    // contentChange
    function onDateChanged()
    {
        crudMS.masterValue = $('#dd').datebox('getValue');
        var params = [];
        params.push({name: 'depotId', value: crudMS.depotId});        
        params.push({name: 'carryoutDate', value: crudMS.masterValue});        
        var qs = abp.utils.buildQueryString(params);
        // alert(qs);
        $('#dg').datagrid({
            url: 'GridDataActive' + qs
        });

        abp.services.app.settle.isSettled(crudMS.depotId, crudMS.masterValue).done(function (data) {
            if (data == true) {
                crudMS.options.readonly = true;
                $('#tbSon2').children('a[name="add"]').linkbutton("disable");
            }
            else {
                crudMS.options.readonly = false;
                $('#tbSon2').children('a[name="add"]').linkbutton("enable");
            }
        })

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
        // alert(operatePassword);
        if ( operatePassword == null || operatePassword.length === 0) {
            abp.notify.error("请先设置用户操作认证密码");
            return;
        }

        if ( operatePassword != $('#Password').val())  {
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

    function activeTasks(s)
    {
        crudMS.options.service.activate(crudMS.depotId, crudMS.masterValue, s).done(function (data) {
            abp.notify.info('已激活'+data+'条线路');
            $('#dlgActivate').dialog('close');            
            if (data > 0) crudMS.reload();
        });
    }

    function showRouteIdentify()
    {
        if (crudMS.parentValue == 0) {
            abp.notify.error('请选择线路');
            return;
        }
        $('#dlgRouteIdentify').dialog('open');
        $('#dgRouteIdentify').datagrid({
            url: '/Routes/IdentifiesGridData/' + crudMS.parentValue
        });       
    }

    crudMS.beforeFormatOperationSon2 = function(val, row, index)
    {
        var types = $('#taskTypeId').combobox('getData');

        for (var i = 0; i < types.length; i++) {
            if (parseInt(types[i].value) == row.taskTypeId && typelist.indexOf(types[i].displayText.substr(0, 2)) >= 0) {
                // abp.notify.error('不允许操作此类任务类型');
                crudMS.editOperationShow[2] = crudMS.deleteOperationShow[2] = false;
                return;
            }       
        }
        crudMS.editOperationShow[2] = crudMS.deleteOperationShow[2] = true;
    }

})();