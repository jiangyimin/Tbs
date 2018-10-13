(function() {
    $(function() {
        var _userService = abp.services.app.user;
        var _$dialog = $('#dlg');
        var _$dg = $('#dg');
        var _$form = _$dialog.find('form');
        var _row;

        // role combobox
        _userService.getRoles().done(function(roles) {
            $('#roleName').combobox({
                data: roles,
                valueField: 'name',
                textField: 'displayName'
            });
        });

        $('#tb').children('a[name="add"]').click(function (e) {
            _$dialog.dialog('open').dialog('setTitle', '增加');
            _$form.form('clear');
            $('#UserName').next('span').find('input').focus();
        });

        $('#tb').children('a[name="edit"]').click(function (e) {
            _row = $('#dg').datagrid('getSelected');
            if (!_row) {
                abp.notify.error("选择要修改的用户", "", { positionClass : 'toast-top-center'} );
                return;
            }
            _$dialog.dialog('open').dialog('setTitle', '修改');
            _$form.form('load', _row);
            $('#UserName').attr('readonly', 'readonly');
        });

        $('#tb').children('a[name="remove"]').click(function (e) {
            _row = _$dg.datagrid('getSelected');
            if (!_row) {
                abp.notify.error("选择要删除的行", "", { positionClass : 'toast-top-center'} );
                return;
            }
            abp.message.confirm('确定删除这一行吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    _userService.deleteUser(_row.id).done(function () {
                        abp.notify.info(_row.name + " 被删除！");
                        _$dialog.dialog('close');
                        _$dg.datagrid('reload'); 
                   });
                }
            });
        });

        $('#dlg-tb').children('a[name="save"]').click(function (e) {
            e.preventDefault();

            if (!_$form.form('validate')) {
                return;
            }

            var user = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js

            abp.ui.setBusy(_$dialog);
            var _defer;
            if (_$dialog.panel('options').title === "增加")
                _defer = _userService.createUser(user);
            else
                _defer = _userService.updateUser(_row.id, user);

            _defer.done(function () {
                abp.notify.info(_$dialog.panel('options').title+'操作成功')
                _$dialog.dialog('close');
                _$dg.datagrid('reload');
            }).always(function() {
                abp.ui.clearBusy(_$dialog);
            });
        });
        
        $('#dlg-tb').children('a[name="cancel"]').click(function (e) {
            _$dialog.dialog('close');
        });
    });
})();