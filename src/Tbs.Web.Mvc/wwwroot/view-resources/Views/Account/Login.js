(function () {
    $(function () {
        $('#TenantName').textbox('setValue', localStorage.getItem("tenantName"));

        $('#LoginButton').click(function (e) {
            // storage
            localStorage.setItem("tenantName", $('#TenantName').textbox('getValue'));

            e.preventDefault();
            var $loginForm = $('#LoginForm');
            
            if (!$loginForm.form('validate'))
                return;

            abp.services.app.session.getTenantId(localStorage.getItem("tenantName")).done(function (tenantId) {
                if (tenantId == 0)
                    abp.multiTenancy.setTenantIdCookie(null);
                else
                    abp.multiTenancy.setTenantIdCookie(tenantId);

                abp.ui.setBusy(
                    $('#LoginArea'),
                    abp.ajax({
                        contentType: 'application/x-www-form-urlencoded',
                        url: $loginForm.attr('action'),
                        data: $loginForm.serialize(),
                    })              
                );

                $('#UserName').next('span').find('input').focus();
            });
        });

        $('#ReturnUrlHash').val(location.hash);

        $('#UserName').next('span').find('input').focus();
    });

})();