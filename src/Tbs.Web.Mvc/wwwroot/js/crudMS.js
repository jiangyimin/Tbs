var crudMS = crudMS || {};
(function () {
    //定义及初始化变量
    crudMS.options = {
        dgParent: '#dg',
        dlgParent: '#dlg',
        fmParent: '#fm',
        dgSon1: '#dgSon1',
        dlgSon1: '#dlgSon1',
        fmSon1: '#fmSon1',
        dgSon2: '#dgSon2',
        dlgSon2: '#dlgSon2',
        fmSon2: '#fmSon2',
        readonly: false,
        editEnabled: false,
        tmCount: 0
    }

    crudMS.editOperationShow = [true, true, true];
    crudMS.deleteOperationShow = [true, true, true];

    var _$dg = $(crudMS.options.dgParent);
    var _$dlg = $(crudMS.options.dlgParent);
    var _$fm = $(crudMS.options.fmParent);


    // can reset in outer
    crudMS.beforeFormatOperation = function(val, row, index) { }
    crudMS.beforeFormatOperationSon1 = function(val, row, index) { }
    crudMS.beforeFormatOperationSon2 = function(val, row, index) { }
    crudMS.enableEdit = function () {}

    crudMS.authorizeEdit = function () {
        if (crudMS.options.editEnabled == false) {
            abp.notify.warn("此操作需要授权");
            crudMS.enableEdit();
        }
        return crudMS.options.editEnabled;
    }

    crudMS.editDisabledCount = function () {
        crudMS.options.tmCount = crudMS.options.tmCount + 1;
        if (crudMS.options.tmCount >= 10) {
            crudMS.options.tmCount = 0;
            crudMS.options.editEnabled = false;
            crudMS.reload();
        }
    }

    crudMS.reload = function () {
        _$dg.datagrid('reload');
        crudMS.selectRow();
    }

    crudMS.addNew = function () {
        if (crudMS.authorizeEdit() === false) return;
        _$dlg.dialog('open').dialog('setTitle', '增加');
        _$fm.form('clear');
        $("#id").attr("value", '0');    // 将Id值置为0
        $("#depotId").attr("value", crudMS.depotId);
        if (crudMS.masterField)
            $(crudMS.masterField).attr('value', crudMS.masterValue);
    };

    crudMS.deletes = function () {
        if (crudMS.authorizeEdit() === false) return;
        var checkedRows = _$dg.datagrid("getChecked");
        if (checkedRows.length === 0) {
            abp.notify.warn("请先选中要删除的行。");
            return;
        }
        var ids = [];
        checkedRows.forEach( function(val,index,arr) {
            ids.push(val.id);
        });

        abp.message.confirm('确认要删除所有选中的行?', '请确认', function (r) {
            if (r) {
                crudMS.options.service.deletes(ids).done(function () {
                    abp.notify.info('批量删除操作成功')
                    _$dg.datagrid('reload');
                    crudMS.selectRow();
                });
            };
        });
    };
        
    crudMS.edit = function (index) {
        if (crudMS.authorizeEdit() === false) return;
         var row = _$dg.datagrid('getRows')[index];
         _$dlg.dialog('open').dialog('setTitle', '编辑');
         _$fm.form('load', row);
    };
        
    crudMS.delete = function (index) {
        // if (crudMS.beforeChangeParent(index) == false) return;
        
        if (crudMS.authorizeEdit() === false) return;
        var row = _$dg.datagrid('getRows')[index];
        if (!row) return;
        abp.message.confirm('确认要删除此记录吗?', '请确认', function (r) {
            if (r) {
                crudMS.options.service.delete(row.id).done(function () {
                    abp.notify.info('删除操作成功')
                    _$dg.datagrid('reload');
                    crudMS.selectRow();
                });
            }
        });
    };
            
    crudMS.save = function () {
        if (!_$fm.form('validate'))
            return;

        var entity = _$fm.serializeFormToObject(); //serializeFormToObject is defined in main.js
        abp.ui.setBusy(_$dlg);
        var _defer;
        if (_$dlg.panel('options').title === "增加")
            _defer = crudMS.options.service.insert(entity); 
        else
            _defer = crudMS.options.service.update(entity);

        _defer.done(function () {
            abp.notify.info(_$dlg.panel('options').title+'操作成功')
            _$dlg.dialog('close');
            _$dg.datagrid('reload');
            crudMS.selectRow();
        }).always(function() {
            abp.ui.clearBusy(_$dlg);
        });    
    }

    crudMS.operator = function (val, row, index) {
        if (crudMS.options.readonly === true) return;

        crudMS.beforeFormatOperation(val, row, index);
        var htmlTag = '';
        if (crudMS.editOperationShow[0] == true)
            htmlTag = '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crudMS.edit(' + index + ')">编辑</a>';
        htmlTag = htmlTag + '<span>&nbsp;&nbsp;&nbsp;&nbsp;</span>';
        if (crudMS.deleteOperationShow[0] == true)
            htmlTag = htmlTag + '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crudMS.delete(' + index + ')">删除</a>';
        return htmlTag;
    }

    crudMS.selectRow = function () {
        //alert(_$dg.datagrid('getRows').length);
        if (_$dg.datagrid('getRows').length > 0)
            _$dg.datagrid('selectRow', 0);
        
        crudMS.parentValue = 0;       
    }

    // for son1
    var _$dgSon1 = $(crudMS.options.dgSon1);
    var _$dlgSon1 = $(crudMS.options.dlgSon1);
    var _$fmSon1 = $(crudMS.options.fmSon1);


    crudMS.operatorSon1 = function (val, row, index) {
        if (crudMS.options.readonly === true) return;

        crudMS.beforeFormatOperationSon1(val, row, index);
        var htmlTag='';
        if (crudMS.editOperationShow[1] == true)
            htmlTag = '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crudMS.editSon1(' + index + ')">编辑</a>';
        htmlTag = htmlTag + '<span>&nbsp;&nbsp;&nbsp;&nbsp;</span>';
        if (crudMS.deleteOperationShow[1] == true)
            htmlTag = htmlTag + '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crudMS.deleteSon1(' + index + ')">删除</a>';
        return htmlTag;
    }

    crudMS.addNewSon1 = function () {
        if (crudMS.authorizeEdit() == false) return;
        if (crudMS.parentValue === 0)
        {
            abp.notify.error('请先选择上级记录');
            return;
        }
        _$dlgSon1.dialog('open').dialog('setTitle', '增加');
        _$fmSon1.form('clear');
        $("#idSon1").attr("value", '0');    // 将Id值置为0

        if (crudMS.parentField)
            $(crudMS.parentField).attr('value', crudMS.parentValue);
    };

    crudMS.deletesSon1 = function () {
        // if (crudMS.beforeChangeSon1(index) == false) return;
        
        if (crudMS.authorizeEdit() == false) return;
        var checkedRows = _$dgSon1.datagrid("getChecked");
        if (checkedRows.length === 0) {
            abp.notify.warn("请先选中要删除的行。");
            return;
        }
        var ids = [];
        checkedRows.forEach( function(val,index,arr) {
            ids.push(val.id);
        });

        abp.message.confirm('确认要删除所有选中的行?', '请确认', function (r) {
            if (r) {
                crudMS.options.service.deletesSon(ids).done(function () {
                    abp.notify.info('批量删除操作成功')
                    _$dgSon1.datagrid('reload');
                });
            };
        });
    };
        
    crudMS.editSon1 = function (index) {
        // if (crudMS.beforeChangeSon1(index) == false) return;

        if (crudMS.authorizeEdit() == false) return;
        var row = _$dgSon1.datagrid('getRows')[index];
        _$dlgSon1.dialog('open').dialog('setTitle', '编辑');
        _$fmSon1.form('load', row);
    };
        
    crudMS.deleteSon1 = function (index) {
        // if (crudMS.beforeChangeSon1(index) == false) return;

        if (crudMS.authorizeEdit() == false) return;
        var row = _$dgSon1.datagrid('getRows')[index];
        if (!row) return;
        abp.message.confirm('确认要删除此记录吗?', '请确认', function (r) {
            if (r) {
                crudMS.options.service.deleteSon(row.id).done(function () {
                    abp.notify.info('删除操作成功')
                    _$dgSon1.datagrid('reload');
                });
            }
        });
    };
            
    crudMS.saveSon1 = function () {
        if (!_$fmSon1.form('validate'))
            return;

        var entity = _$fmSon1.serializeFormToObject(); //serializeFormToObject is defined in main.js
        abp.ui.setBusy(_$dlgSon1);
        var _defer;
        if (_$dlgSon1.panel('options').title === "增加")
            _defer = crudMS.options.service.insertSon(entity); 
        else
            _defer = crudMS.options.service.updateSon(entity);

        _defer.done(function () {
            abp.notify.info(_$dlgSon1.panel('options').title+'操作成功')
            _$dlgSon1.dialog('close');
            _$dgSon1.datagrid('reload');
        }).always(function() {
            abp.ui.clearBusy(_$dlgSon1);
        });    
    }

    // for son2
    var _$dgSon2 = $(crudMS.options.dgSon2);
    var _$dlgSon2 = $(crudMS.options.dlgSon2);
    var _$fmSon2 = $(crudMS.options.fmSon2);


    crudMS.operatorSon2 = function (val, row, index) {
        if (crudMS.options.readonly === true) return;
    
        crudMS.beforeFormatOperationSon2(val, row, index)
        var htmlTag='';
        if (crudMS.editOperationShow[2] == true)
            htmlTag = '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crudMS.editSon2(' + index + ')">编辑</a>';
        htmlTag = htmlTag + '<span>&nbsp;&nbsp;&nbsp;&nbsp;</span>';
        if (crudMS.deleteOperationShow[2] == true)
            htmlTag = htmlTag + '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crudMS.deleteSon2(' + index + ')">删除</a>';
        return htmlTag;
    }

    crudMS.addNewSon2 = function () {
        if (crudMS.authorizeEdit() == false) return;
        if (crudMS.parentValue === 0)
        {
            abp.notify.error('请先选择上级记录');
            return;
        }
        _$dlgSon2.dialog('open').dialog('setTitle', '增加');
        _$fmSon2.form('clear');
        $("#idSon2").attr("value", '0');    // 将Id值置为0

        if (crudMS.parentField)
            $(crudMS.parentField+'2').attr('value', crudMS.parentValue);
    };

    crudMS.deletesSon2 = function () {
        if (crudMS.authorizeEdit() == false) return;
        var checkedRows = _$dgSon2.datagrid("getChecked");
        if (checkedRows.length === 0) {
            abp.notify.warn("请先选中要删除的行。");
            return;
        }
        var ids = [];
        checkedRows.forEach( function(val,index,arr) {
            ids.push(val.id);
        });

        abp.message.confirm('确认要删除所有选中的行?', '请确认', function (r) {
            if (r) {
                crudMS.options.service.deletesSon2(ids).done(function () {
                    abp.notify.info('批量删除操作成功')
                    _$dgSon2.datagrid('reload');
                });
            };
        });
    };
        
    crudMS.editSon2 = function (index) {
        // if (crudMS.beforeFormatOperationSon2() == false) return;

        if (crudMS.authorizeEdit() == false) return;
        var row = _$dgSon2.datagrid('getRows')[index];
        _$dlgSon2.dialog('open').dialog('setTitle', '编辑');
        _$fmSon2.form('load', row);
    };
        
    crudMS.deleteSon2 = function (index) {
        // if (crudMS.beforeChangeSon2(index) == false) return;
        
        if (crudMS.authorizeEdit() == false) return;
        var row = _$dgSon2.datagrid('getRows')[index];
        if (!row) return;
        abp.message.confirm('确认要删除此记录吗?', '请确认', function (r) {
            if (r) {
                crudMS.options.service.deleteSon2(row.id).done(function () {
                    abp.notify.info('删除操作成功')
                    _$dgSon2.datagrid('reload');
                });
            }
        });
    };
            
    crudMS.saveSon2 = function () {
        if (!_$fmSon2.form('validate'))
            return;

        var entity = _$fmSon2.serializeFormToObject(); //serializeFormToObject is defined in main.js
        abp.ui.setBusy(_$dlgSon2);
        var _defer;
        if (_$dlgSon2.panel('options').title === "增加")
            _defer = crudMS.options.service.insertSon2(entity); 
        else
            _defer = crudMS.options.service.updateSon2(entity);

        _defer.done(function () {
            abp.notify.info(_$dlgSon1.panel('options').title+'操作成功')
            _$dlgSon2.dialog('close');
            _$dgSon2.datagrid('reload');
        }).always(function() {
            abp.ui.clearBusy(_$dlgSon2);
        });    
    }

})();