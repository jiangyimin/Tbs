(function() {
    var status;
    $(function() {
        crudMS.options.service = abp.services.app.whAffair;
        crudMS.masterField = "#carryoutDate";
        crudMS.depotId = $('#depotId').val();

        crudMS.parentField = '#whAffairId';
        crudMS.parentValue = 0;

        crudMS.beforeFormatOperation = function (val, row, index) {
            if (row.status != "安排")
                crudMS.deleteOperationShow[0] = crudMS.editOperationShow[0] = false;
            else
                crudMS.deleteOperationShow[0] = crudMS.editOperationShow[0] = true;             
        }

        crudMS.beforeFormatOperationSon1 = function (val, row, index) {
            if (row.checkIn != null || row.checkOut != null) {
                crudMS.editOperationShow[1] = crudMS.deleteOperationShow[1] = false;
            }
            else {
                crudMS.editOperationShow[1] = crudMS.deleteOperationShow[1] = true;
            }
        }
        
        crudMS.enableEdit = function () {
            isEnableEdit = true;
            $('#dlgActivate').dialog('open');
            $('#Password').textbox('setValue','');           
        }

        // set editEnabled is false when internal 30 * 10 = 300s(5min)
        var tm = window.setInterval("crudMS.editDisabledCount()", 30000);

        // ui
        abp.services.app.combo.getWorkerItems(crudMS.depotId).done(function (data) {
            $('#workerId').combobox({
                data: data,
                valueField: 'id',
                textField: 'displayText'
            })
        });

        $('#dd').datebox({
            onChange: onDateChanged
        })

        $('#dg').datagrid({
            onSelect: function (index, row) {
                crudMS.parentValue = row.id;
                status = row.status;

                $('#dgSon1').datagrid({
                    url: 'WhAffairs/WorkersGridData/' + row.id 
                })
           }
        })

        // Buttons click event
        $('#tb').children('a[name="activate"]').click(function (e) {
            if (crudMS.options.readonly == true) return;
            if (crudMS.authorizeEdit() === false) return;
            isEnableEdit = false;
            $('#dlgActivate').dialog('open');
            $('#Password').textbox('setValue','');
        });

        $('#tb').children('a[name="createFrom"]').click(function (e) {
            if (crudMS.options.readonly == true) return;
            if (crudMS.authorizeEdit() === false) return;
            createFrom();
        });

        $('#dlgActivate-buttons').children('a[name="validFinger"]').click(function (e) {
            validFinger();
        });

        $('#dlgActivate-buttons').children('a[name="validPassword"]').click(function (e) {
            validPassword();
        });

        onDateChanged();
    });

    // contentChange
    function onDateChanged()
    {
        crudMS.masterValue = $('#dd').datebox('getValue');
        // alert(crudMS.masterValue);
        var params = [];
        params.push({name: 'depotId', value: crudMS.depotId});        
        params.push({name: 'carryoutDate', value: crudMS.masterValue});        
        var qs = abp.utils.buildQueryString(params);

        $('#dg').datagrid({
            url: 'WhAffairs/GridData' + qs
        });

        abp.services.app.settle.isSettled(crudMS.depotId, crudMS.masterValue).done(function (data) {
            if (data == true) {
                crudMS.options.readonly = true;
                $('#tb').children('a[name="add"]').linkbutton("disable");
                $('#tb').children('a[name="activate"]').linkbutton("disable");
                $('#tb').children('a[name="createFrom"]').linkbutton("disable");
                $('#tbSon1').children('a[name="add"]').linkbutton("disable");
            }
            else {
                crudMS.options.readonly = false;
                $('#tb').children('a[name="add"]').linkbutton("enable");
                $('#tb').children('a[name="activate"]').linkbutton("enable");
                $('#tb').children('a[name="createFrom"]').linkbutton("enable");
                $('#tbSon1').children('a[name="add"]').linkbutton("enable");
            }
        })
        crudMS.selectRow();
    };

    function createFrom()
    {
        var from = $('#fromDate').datebox('getValue');
        if (from == '') {
            abp.notify.error("请先设置来源日期!");
            return;
        }
        if (crudMS.masterValue < from){
            abp.notify.error("任务执行日期必须大于来源日期!");
            return;
        }

        abp.message.confirm('确认要生成吗?', '确认', function (r) {
            if (r) {
                abp.ui.setBusy($('#tb'));
                crudMS.options.service.createFrom(crudMS.depotId, crudMS.masterValue, from).done(function (data) {
                    abp.notify.info('已生成'+data+'个任务');
                    if (data > 0) crudMS.reload();
                }).always(function () {
                    abp.ui.clearBusy($('#tb'));
                });
            }
        })
    }

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

        var dateStr = crudMS.masterValue + ' ' + new Date().Format('hh');
        // alert(dateStr);
        // alert(deadline);
        if ( operatePassword != $('#Password').val() || dateStr >  deadline)  {
            abp.notify.error("密码错误或过期");
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
            abp.notify.info('已激活'+data+'个任务');
            $('#dlgActivate').dialog('close');            
            if (data > 0) crudMS.reload();
        });
    }

})();