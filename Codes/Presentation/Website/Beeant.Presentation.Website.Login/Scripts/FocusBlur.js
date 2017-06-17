//普通框获取焦点、失去焦点
		function focusBlur(id) {
            var val = $('#' + id).val();
            var obj = $('#' + id);
            obj.focus(function () {
                if ($(this).val() == val) {
                    $(this).val('');
                }
//                if ($(this).val()!= '' && $(this).val() != val) {
//                    $(this).val();
//                }
                $(this).keydown(function () {
                    $(this).next().css('display', 'block').click(function () {
                        obj.val('');
                        $(this).css('display', 'none');
                        obj.focus();
                    });
                });
            });
            obj.blur(function () {
                if (obj.val() == '') {

                    obj.val(val);
                }
            }); 

        }
	 //文本框与密码框转换，获取焦点失去焦点
        function toggleTextPassword(id) {
            var obj = $('#' + id);
            var val = obj.val();
            obj.focus(function () {
                if ($(this).val() == '密码') {
                    $(this).val('');
                }
                $(this).get(0).type = 'password';
                
                //                if ($(this).val() && $(this).val()!="密码") {
                //                    $(this).val('');
                //                }else if ($(this).val() != '' && $(this).val() != val) {
                //                    $(this).val('');
                //                }
                //                $(this).get(0).type = 'password';
                $(this).keydown(function () {
                    $(this).next().css('display', 'block').click(function () {
                        obj.val('');
                        $(this).css('display', 'none');
                        obj.focus();
                    });
                });
            });
            obj.blur(function () {
                if (obj.val() == '') {
                    $(this).get(0).type = 'text';
                    obj.val(val);
                } else if (obj.val() && obj.val() != val) {
                    $(this).get(0).type = 'password';
                }
            });
        }