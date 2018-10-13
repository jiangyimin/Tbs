(function ($) {
    //监听加载状态改变
    document.onreadystatechange = completeLoading;

    //加载状态为complete时移除loading效果
    function completeLoading () {
        if (document.readyState === "complete") {
            var loadingMask = document.getElementById('loadingDiv');
            loadingMask.parentNode.removeChild(loadingMask);
        }
    }
    
    //Notification handler
    abp.event.on('abp.notifications.received', function (userNotification) {
        abp.notifications.showUiNotifyForUserNotification(userNotification);

        //Desktop notification
        Push.create("AbpZeroTemplate", {
            body: userNotification.notification.data.message,
            icon: abp.appPath + 'images/app-logo-small.png',
            timeout: 6000,
            onClick: function () {
                window.focus();
                this.close();
            }
        });
    });

    // plugins for jQuery: etComboItems setImagePreview
    $.extend({
        getComboItems : function (domid, name, key, value, appdix) {
            return abp.services.app.combo.getComboItems(name, key, value, appdix).done(function (data) { 
                if (domid !== '') {
                    $(domid).combobox({
                        data: data,
                        valueField: 'value',
                        textField: 'displayText'
                    });
                }
            });
        },

        setImagePreview: function (domid, obj) {
            var file = obj.files[0];
            if (obj.files && file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $(domid).attr("src", e.target.result);
                }
                reader.readAsDataURL(file);
            }
            //$("#PhotoImage").val('');
        },

        // formatter
        displayDepotText: function (val) {
            var depots = $('#depotId').combobox('getData');
            for (var i = 0; i < depots.length; i++) {
                if (val === parseInt(depots[i].value)) 
                    return depots[i].displayText;
            };
            return val;
        },

        // formatters
        dateFormatter: function (val) {
            if (val) return val.substr(0, 10);
        },

        timeFormatter: function (val) {
            if (val) return val.substr(11, 5);
        },

        datetimeFormatter: function (val) {
            if (val) return val.substr(0, 10) + ' ' + val.substr(11, 5);
        },

        toExcel: function (tbl, title) {
            try {
                var rows = $(tbl).datagrid('getRows');
                var columns = $(tbl).datagrid("options").columns[0];
                var oXL = new ActiveXObject("Excel.Application"); //创建AX对象excel 
                var oWB = oXL.Workbooks.Add();
                var oSheet = oWB.ActiveSheet;

                oSheet.Name = title;
                //设置表头
                for (var i = 0; i < columns.length; i++) {
                    oSheet.Cells(1, i + 1).value = columns[i].title;
                }
                //设置内容部分
                for (var i = 0; i < rows.length; i++) {
                    //动态获取每一行每一列的数据值
                    for (var j = 0; j < columns.length; j++) {
                        oSheet.Cells(i + 2, j + 1).value = rows[i][columns[j].field];
                    }
                }
                oXL.Visible = true;
            } catch (e) {
                alert("无法启动Excel!\n\n如果您确信您的电脑中已经安装了Excel");
            }
        }
    });

    Date.prototype.Format = function (fmt) { //author: meizz 
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) 
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }

    $.fn.serializeFormToObject = function () {
        //serialize to array
        var data = $(this).serializeArray();

        //add also disabled items
        $(':disabled[name]', this).each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

        //map to object
        var obj = {};
        data.map(function (x) { obj[x.name] = x.value; });

        return obj;
    };

    //Configure blockUI
    if ($.blockUI) {
        $.blockUI.defaults.baseZ = 2000;
    }

})(jQuery);