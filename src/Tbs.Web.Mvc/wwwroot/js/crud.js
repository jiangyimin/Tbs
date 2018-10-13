var crud = crud || {};
(function () {
    var _$dg = $('#datagrid');
    var _$dlg = $('#dlg');
    var _$fm = $('#fm');
    var _action;

    //定义及初始化变量
    crud.options = {
        filter: '',
        readonly: false,
        children: false,
        parentId: 0,
        name: '',
        title: '',
        toolbarItem: 'reload,add,delete',
        sortName: 'id',
        sortOrder: 'asc', 
        pageSize: 20,
        pageList: [15, 20, 25, 50, 100, 200],
        columns: [[]]
    }

    //前置逻辑，将在构造datagrid之前执行
    crud.startfunction = function() { };
    //后置逻辑，将在构造datagrid之后执行
    crud.endfunction = function() { }; 

    crud.reload = function () {
        _$dg.datagrid('reload');
    }

    crud.addNew = function () {
        if (crud.options.children && crud.options.parentId === 0) {
            abp.notify.error("请先选择上级记录");
            return;
        }

        _$dlg.dialog('open').dialog('setTitle', '增加');
        _$fm.form('clear');
        $("#id").attr("value", '0');    // 将Id值置为0
        if (crud.options.parentId > 0) {
            $('#'+crud.options.parentField).combobox('setValue', crud.options.parentId);
        }
        _action = '/CreateEntity';
    };

    crud.deletes = function () {
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
                _action = '/DeleteEntities';
                crud.sendAjax({ids: ids});
            };
        });
    };
        
    crud.edit = function (index) {
        var row = _$dg.datagrid('getRows')[index];
        _$dlg.dialog('open').dialog('setTitle', '编辑');
        _$fm.form('load', row);
        _action = '/UpdateEntity';
    };
        
    crud.delete = function (index) {
        var row = _$dg.datagrid('getRows')[index];
        if (!row) return;
        abp.message.confirm('确认要删除此记录吗?', '请确认', function (r) {
            if (r) {
                _action = '/DeleteEntity';
                crud.sendAjax({id: row.id});
            };
        });
    };
    
    crud.cancel = function () {
        _$dlg.dialog('close');
    };
        
    crud.save = function () {
        if (!_$fm.form('validate'))
            return;
        var fd = new FormData(document.getElementById('fm'));         
        abp.ui.setBusy( 
            _$dlg, crud.sendFdAjax(fd)
        );
    }

    crud.sendAjax = function (data) {
        abp.ajax({
            contentType: 'application/x-www-form-urlencoded',
            url: crud.options.name + _action,
            data: data,
            success: function (data) {
                abp.notify.info(data.content);
                _$dlg.dialog('close');
                _$dg.datagrid('reload');
            }
        });
    }

    crud.sendFdAjax = function (data) {
        return abp.ajax({
            contentType: false,
            processData: false,
            url: crud.options.name + _action,
            data: data,
            success: function (data) {
                abp.notify.info(data.content);
                _$dlg.dialog('close');
                _$dg.datagrid('reload');
            }
        });
    }

    crud.operator = function (val, row, index) {
        var htmlTag = '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crud.edit(' + index + ')">编辑</a>';
        htmlTag = htmlTag + '<span>&nbsp;&nbsp;&nbsp;&nbsp;</span>';
        htmlTag = htmlTag + '<a href="javascript:void(0)" class="easyui-linkbutton" onclick="crud.delete(' + index + ')">删除</a>';
        return htmlTag;
    }

    // document ready
    $(function () {
        // get toolbardata
        crud.toolbarData=[];        
        if (crud.options.toolbarItem.indexOf('reload') >= 0) {
            crud.toolbarData.push({ text: "刷新", iconCls: "icon-reload", handler: crud.reload} );
        }

        if (crud.options.readonly === false && crud.options.toolbarItem.indexOf('add') >= 0) {
            crud.toolbarData.push({ text: "增加", iconCls: "icon-add", handler: crud.addNew });
        }
        if (crud.options.readonly === false && crud.options.toolbarItem.indexOf('delete') >= 0) {
            crud.toolbarData.push("-");
            crud.toolbarData.push({ text: "批量删除", iconCls: "icon-remove", handler: crud.deletes });
        }

        crud.startfunction();

        // splice columns
        if (crud.options.readonly === false)
        {
            crud.options.columns[0].unshift({ field: "ck", checkbox: true });
            crud.options.columns[0].push({ field: "operator", title: "操作", width: 80, align: "center", formatter: crud.operator });
        }

        // set url and grid
        var url = '';
        if (crud.options.children === false)
            url = crud.options.name + '/GridPagedData/' + crud.options.filter;

        _$dg.datagrid({
            url: url, 
            title: crud.options.title + '列表',
            toolbar: crud.toolbarData,
            fit: true,
            fitColumns: true,
            columns: crud.options.columns,
            striped: true,
            rownumbers: true,
            checkOnSelect: false,
            sortName: crud.options.sortName,
            sortOrder: crud.options.sortOrder,
            pagination: true,
            pageSize: crud.options.pageSize,
            pageList: crud.options.pageList, 
            showFooter: false
        });

        _$dlg.dialog({
            buttons:[
                { text:'保存', iconCls: 'icon-save', handler: crud.save },
			    { text:'取消', iconCls: 'icon-cancel', handler: crud.cancel }
			]
        });

        crud.endfunction();
    });

})();