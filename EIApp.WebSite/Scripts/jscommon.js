/*
公共js代码
by wangwd
2015-9-15
*/
(function ($) {
    $.extend($.fn, {
        /*#region 自定义datagrid插件*/
        ///<summary>
        /// Hitech DIMS datagrid
        ///</summary>
        DimsDG: function (options) {
            var $this = this; //缓存当前作用域的this
            var _initings = {
                "url": "url",
                "title": "标题",
                "idField": "id",
                "columns": [[{
                    title: '编号',
                    field: 'id',
                    width: 80
                }]],
                "AddFuntion": function (rows) {
                    $.messager.alert('警告', '未实现的函数', "error");
                },
                "RemoveFuntion": function (rows) {
                    $.messager.alert('警告', '未实现的函数', "error");
                },
                "EditFuntion": function (rows) {
                    $.messager.alert('警告', '未实现的函数', "error");
                }
            };
            //
            var defaults = {
                url: _initings.url,
                title: _initings.title,
                method: 'Post',
                pagination: true,
                //是否分页
                pageSize: 20,
                pageList: [10, 15, 20, 25, 30],
                singleSelect: true,
                //是否只能选择一行
                fit: true,
                //自适应父容器
                fitColumns: true,
                //自动展开/收缩列的大小
                rownumbers: true,
                //行号
                nowarp: true,
                //如果为true，则在同一行中显示数据。设置为true可以提高加载性能。
                border: true,
                //是否显示边框
                idField: _initings.idField,
                //指明哪一个字段是标识字段。
                columns: _initings.columns,
                //data:mydata,//需要显示的json数据
                toolbar: [{
                    text: '增加',
                    iconCls: 'icon-add',
                    handler: function () {
                        var rows = $this.datagrid('getSelections');
                        if (rows.length != 1) {
                            rows = [];
                        }
                        if (options.AddFuntion) {
                            options.AddFuntion(rows[0]);
                        } else {
                            _initings.AddFuntion(rows[0]);
                        }
                        //$this.datagrid('reload');//刷新当前页
                    }
                },
				'-', {
				    text: '修改',
				    iconCls: 'icon-edit',
				    handler: function () {
				        var rows = $this.datagrid('getSelections');

				        if (rows.length != 1) {
				            $.messager.alert('警告', '抱歉,请先选择一行', "warning");
				            return;
				        }
				        if (options.EditFuntion) {
				            options.EditFuntion(rows[0]);
				        } else {
				            _initings.EditFuntion(rows[0]);
				        }
				    }
				},
				'-', {
				    text: '删除',
				    iconCls: 'icon-remove',
				    handler: function () {
				        var rows = $this.datagrid('getSelections');
				        if (rows.length != 1) {
				            $.messager.alert('警告', '抱歉,请先选择一行', "warning");
				            return;
				        }
				        $.messager.confirm('确认对话框', '您想要删除该行吗？',
						function (r) {
						    if (r) {
						        if (options.RemoveFuntion) {
						            options.RemoveFuntion(rows[0]);
						        } else {
						            _initings.RemoveFuntion(rows[0]);
						        }
						    }
						});
				    }
				}],
                onDblClickCell: function (index, field, value) {
                    $this.datagrid('uncheckRow', index);
                },
                onLoadSuccess: function (data) {
                },
                onLoadError: function (data) {
                    $.messager.alert('警告', "数据加载失败!<br/>错误原因:" + data.statusText + "!<br/>请联系系统管理员!", "error");
                },
                //数据筛选
                //loadFilter: function (data) {
                //    if (data.IsSuccess) {
                //        return data.Data;
                //    }
                //    else {
                //        data = { "total": 0, "rows": [] };
                //        return data;
                //    }
                //}
            };
            var settings = {};

            $.extend(settings, defaults, options);
            return $this.datagrid(settings);
        },

        /*#endregion*/

        /*#region 自定义dialog插件*/
        ///<summary>
        /// Hitech DIMS dialog
        ///</summary>
        DimsDialog: function (options) {
            var $this = this; //缓存当前作用域的this
            var settings = {};
            var _initings = {
                "title": "模态窗口",
                "width": 400,
                "height": 300,
                "onOpenDialog": function () {
                    //$.messager.alert('警告', '未实现的函数', "error");
                },
                "OKFuntion": function () {
                    //$.messager.alert('警告', '未实现的函数', "error");
                },
                IsValidation: true//默认开启校验模式
            };
            var defaults = {
                title: _initings.title,
                width: _initings.width,
                height: _initings.height,
                //closed: false,
                cache: false,
                resizable: true,
                //href: 'Test.html',
                modal: true,
                buttons: [{
                    text: '确定',
                    iconCls: 'icon-ok',
                    handler: function () {
                        //是否定义属性IsValidation
                        var IsValidation = options.IsValidation !== undefined ? options.IsValidation : _initings.IsValidation;
                        //页面检验
                        if (IsValidation) {
                            if (!$this.form('enableValidation').form('validate')) {
                                return;
                            }
                        }
                        var serializeData = $this.find("*").serializeArray(); //序列化数据
                        if (options.OKFuntion) {
                            options.OKFuntion(serializeData);
                        } else {
                            _initings.OKFuntion(serializeData);
                        }
                    }
                },
				{
				    text: '取消',
				    iconCls: 'icon-cancel',
				    handler: function () {
				        $this.dialog("close");
				    }
				}],
                onBeforeOpen: function () {
                    $this.form('clear');
                    if (options.onOpenDialog) {
                        options.onOpenDialog();
                    } else {
                        _initings.onOpenDialog();
                    }
                }
            }

            $.extend(settings, defaults, options);
            $this.dialog(settings);
        },
        /*#endregion*/

        /*#region 自定义Combobox插件*/
        DimsCombobox: function (options) {
            var $this = this; //缓存当前作用域的this
            var settings = {};
            var defaults = {
                //是否可以输入
                editable: false,
                //数据筛选
                loadFilter: function (data) {
                    if (data.IsSuccess) {
                        return data.Data.rows;
                    } else {
                        data = [];
                        return data;
                    }
                },
                onLoadError: function (data) {
                    $.messager.alert('警告', "Combobox数据加载!<br/>错误原因:" + data.statusText + "!<br/>请联系系统管理员!", "error");
                }
            };
            $.extend(settings, defaults, options);
            $this.combobox(settings);
        },
        /*#endregion*/


    });
})(jQuery);

$(function () {
    /*#region Ajax 全局事件*/
    $.ajaxSetup({
        type: "POST",
        dataType: "json",
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('警告', "ajax出错!<br/>错误原因:" + errorThrown.toString() + "!<br/>请联系系统管理员!", "error");
        }
    });
    /*#endregion*/
});