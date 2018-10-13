(function() {
    $(function() {
        var _tenantService = abp.services.app.tenant;
        var _$dialog = $('#dlg');
        var _$form = _$dialog.find('form');

        $('#tb').children('a[name="add"]').click(function (e) {
            _$dialog.dialog('open');
            $('#TenantName').next('span').find('input').focus();
        });

        $('#dlg-tb').children('a[name="save"]').click(function (e) {
            e.preventDefault();

            if (!_$form.form('validate')) {
                return;
            }

            var tenant = _$form.serializeFormToObject(); //serializeFormToObject is defined in main.js

            abp.ui.setBusy(_$dialog);
            _tenantService.createTenant(tenant).done(function () {
                _$dialog.dialog('close');
                location.reload(true); //reload page to see new tenant!
            }).always(function() {
                abp.ui.clearBusy(_$dialog);
            });
        });
        
        $('#dlg-tb').children('a[name="cancel"]').click(function (e) {
            _$dialog.dialog('close');
        });
    });
})();