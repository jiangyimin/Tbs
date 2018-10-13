(function() {
    $(function() {
        var _editionService = abp.services.app.edition;
        var _$dialog = $('#dlg');
        var _$form = _$dialog.find('form');
        var editionId;

        // submit Feature property changes
        $('#tbFeature').children('a[name="submit"]').click(function (e) {
            var rows = $('#dgFeature').propertygrid("getChanges", "updated");
            if (rows.length == 0) return;

            _editionService.saveFeatureChanges(editionId, rows).done(function () {
                abp.notify.info("成功提交更改!")
            });
        });

        // trigger for children table
        $('#dg').datagrid({
            url: 'Editions/GridData',
            onSelect: function (index, row) {
                editionId = row.id;
                $('#dgFeature').datagrid({
                    url: 'Editions/GetFeatures/' + row.id
                })
            }
        });

        // Buttons click event
        $('#tb').children('a[name="reload"]').click(function (e) {
            $('#dg').datagrid('reload'); 
        });

        $('#tb').children('a[name="add"]').click(function (e) {
            _$dialog.dialog('open');
            $('#Name').next('span').find('input').focus();
        });

        $('#tb').children('a[name="remove"]').click(function (e) {
            let row = $('#dg').datagrid('getSelected');
            if (!row) {
                abp.notify.error("选择要删除的行", "", { positionClass : 'toast-top-center'} );
                return;
            }
            abp.message.confirm('确定删除这一行吗？', '请确定', function (isConfirmed) {
                if (isConfirmed) {
                    _editionService.removeEdition(row.Name).done(function () {
                        _$dialog.dialog('close');
                        $('#dg').datagrid('reload'); 
                    });
                }
            });
        });

        $('#dlg-tb').children('a[name="save"]').click(function (e) {
            e.preventDefault();

            if (!_$form.form('validate')) {
                return;
            }

            var edition = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js

            abp.ui.setBusy(_$dialog);
            _editionService.createEdition(edition).done(function () {
                _$dialog.dialog('close');
                $('#dg').datagrid('reload'); 
            }).always(function() {
                abp.ui.clearBusy(_$dialog);
            });
        });

        $('#dlg-tb').children('a[name="cancel"]').click(function (e) {
            _$dialog.dialog('close');
        });

    });
})();