(function () {
    $(function () {
        $.getJSON("/Home/GetUserMenu", function (data) {
            if (data.result.length === 0) {
                return;
            }
            //第一层生成手风琴的项
            $.each(data.result, function (index, item) {
                if (item.items.length > 0) {
                    var selected = false;
                    if (index === 0) {
                        selected = true;
                    }
                    // Accordion 折叠面板
                    $('#menu-accordion').accordion('add', {
                        title: item.displayName,
                        content: "<ul id='menu_tree_" + item.name + "'></ul>",
                        iconCls: item.icon,
                        selected: selected,
                    });

                    // 树形菜单
                    var treeData = transToTreeData(item.items);
                    $('#menu_tree_' + item.name).tree({
                        data: treeData,
                        onClick: function (node) {
                            // 添加选项卡
                            addTab(node.text, node.url, node.iconCls);
                        }
                    });
                }
            });
        });

        $("#main-tab").tabs({
            onContextMenu: function (e, title) {
                e.preventDefault();
                $("#tab-menu").menu("show", { left: e.pageX, top: e.pageY })
                    .data("tabTitle", title); //将点击的Tab标题加到菜单数据中
            }
        });

        $("#tab-menu").menu({
            onClick: function (item) {
                tabHandle(this, item.id);
            }
        });

        $('#changePassword').click(function (e) {
            $('#dlg').dialog('open');
            $('#fm').form('clear');
        });

        $('#submit').click(function (e) {
            e.preventDefault();
            if (!$('#fm').form('validate')) {
                return;
            }
            if ($('#NewPassword').val() != $('#NewPasswordAgain').val()) {
                abp.notify.warn("请核对两次密码输入正确!");
                return;
            }

            var $dialog = $('#dlg');
            abp.ui.setBusy($dialog);
            abp.services.app.user.changePassword($('#NewPassword').val()).done(function () {
                abp.notify.info("密码修改成功");
                $dialog.dialog('close');
            }).always(function() {
                abp.ui.clearBusy($dialog);
            });
        });
    });

    function addTab(title, url, icon) {
        var $mainTabs = $("#main-tab");
        if ($mainTabs.tabs("exists", title)) {
            $mainTabs.tabs("select", title);
        } else {
            $mainTabs.tabs("add", {
                title: title,
                closable: true,
                icon: icon,
                content: createFrame(url)
            });
        }
    }

    function createFrame(url) {
        var html = '<iframe scrolling="auto" frameborder="0" src="' + url + '" style="width:100%; height:99%"></iframe>';
        return html;
    }

    // utils for sub
    function closeTab(title) {
        $("#main-tab").tabs('close', title);
    }

    function tabHandle(menu, type) {
        var title = $(menu).data("tabTitle");
        var $tab = $("#main-tab");
        var tabs = $tab.tabs("tabs");
        var index = $tab.tabs("getTabIndex", $tab.tabs("getTab", title));
        var closeTitles = [];
        switch (type) {
            case "tab-menu-refresh":
                var iframe = $(".tabs-panels .panel").eq(index).find("iframe");
                if (iframe) {
                    var url = iframe.attr("src");
                    iframe.attr("src", url);
                }
                break;
            case "tab-menu-openFrame":
                var iframe = $(".tabs-panels .panel").eq(index).find("iframe");
                if (iframe) {
                    window.open(iframe.attr("src"));
                }
                break;
            case "tab-menu-close":
                closeTitles.push(title);
                break;
            case "tab-menu-closeleft":
                if (index == 0) {
                    mfx.notify.warn("左边没有可关闭标签。");
                    return;
                }
                for (var i = 0; i < index; i++) {
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
            case "tab-menu-closeright":
                if (index == tabs.length - 1) {
                    mfx.notify.warn("右边没有可关闭标签。");
                    return;
                }
                for (var i = index + 1; i < tabs.length; i++) {
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
            case "tab-menu-closeother":
                for (var i = 0; i < tabs.length; i++) {
                    if (i == index) {
                        continue;
                    }
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
            case "tab-menu-closeall":
                for (var i = 0; i < tabs.length; i++) {
                    var opt = $(tabs[i]).panel("options");
                    if (opt.closable) {
                        closeTitles.push(opt.title);
                    }
                }
                break;
        }
        for (var i = 0; i < closeTitles.length; i++) {
            $tab.tabs("close", closeTitles[i]);
        }
    }

    function transToTreeData(data) {
        var treeData = [];
        $.each(data, function (index, item) {
            var obj = {};
            obj.id = item.name;
            obj.text = item.displayName;
            obj.iconCls = item.icon;
            obj.url = item.url;
            treeData.push(obj);
        });
        return treeData;
    }

})();