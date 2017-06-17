
$(function () {
    focusBlur('username');
    focusBlur('loginCode1');
    toggleTextPassword('password');
});
  function checkBtn() {

      if ($('#username').val() == '手机/邮箱/用户名' && $('#password').val() == '密码') {
            $('.msg-error').css('display', 'block');
            $('.msg-error').find('label').text('请输入用户名和密码！');
            $('.line').find('input').css('borderColor', '#e12f32');
            $('.user_ico').css({ 'backgroundPosition': '0 -23px', 'borderColor': '#e12f32' });
            $('.user_pw').css({ 'backgroundPosition': '0 -68px', 'borderColor': '#e12f32' });
          return false;
      } else if ($('#username').val() == '手机/邮箱/用户名' && $('#password').val() != '密码') {
            $('.msg-error').css('display', 'block');
            $('.msg-error').find('label').text('请输入用户名！');
            $('#username').css('borderColor', '#e12f32');
            $('.user_ico').css({ 'backgroundPosition': '0 -23px', 'borderColor': '#e12f32' });
            return false;
        } else if ($('#username').val() != '手机/邮箱/用户名' && $('#password').val() == '密码') {
            $('.msg-error').css('display', 'block');
            $('.msg-error').find('label').text('请输入密码！');
            $('#password').css('borderColor', '#e12f32');
            $('.user_pw').css({ 'backgroundPosition': '0 -23px', 'borderColor': '#e12f32' });
           return false;
       } else {

           return true;
        }


    }	