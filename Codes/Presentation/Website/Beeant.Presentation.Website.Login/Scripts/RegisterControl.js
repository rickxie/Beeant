
$(function(){
	(function(){
		setCookie('username','nancy',3);
		setCookie('age','23',3);
		
		
		function setCookie(key, value, t) {
			var oDate = new Date();
			oDate.setDate( oDate.getDate() + t );
			document.cookie = key + '='+ value +';expires=' + oDate.toGMTString();
		}
		function getCookie(key) {
			var arr1 = document.cookie.split('; ');
			//['username=leo','age=32']
			for (var i=0; i<arr1.length; i++) {
				
				var arr2 = arr1[i].split('=');
				//['username','leo']
				//['age',32]
				if (arr2[0] == key) {
					return arr2[1];
				}
			}	
		}
		function delCookie(key) {
			setCookie(key, '', -1);
		}
		
	})();
	(function(){
		var user = $('#user').val();;
		$('#user').focus(function(){
			
			$('#user').val(''); 
		});
		$('#user').blur(function(){
			if($('#user').val()==''){
				$('#user').val(user);	
			}else{
				var pattern = /^[a-zA-Z][a-zA-Z0-9_]{5,15}$/;
				if(pattern.test($(this).val())){
					$(this).closest('.line').find('.warn').css('display','none');
					$(this).next().removeClass('em_reg_error').addClass('em_reg_right');
				}else{
					$(this).next().removeClass('em_reg_right').addClass('em_reg_error');
					$(this).closest('.line').find('.warn').css('display','block').text('6-16位字符，支持数字、英文，下划线，不能以数字开头！');
				}
			}
		});
		var phone = '';
		$('#phone').focus(function(){
			phone = $('#phone').val();
			$('#phone').val(''); 
		});
		$('#phone').blur(function(){
			if($('#phone').val()==''){
				$('#phone').val(phone);	
			}else{
				var patrn = /(^1[3|4|5|7|8][0-9]{9}$)/;
				if(patrn.test($('#phone').val())){
					$(this).closest('.line').find('.warn').css('display','none');
					$(this).next().removeClass('em_reg_error').addClass('em_reg_right');
				}else{
					$(this).next().removeClass('em_reg_right').addClass('em_reg_error');
					$(this).closest('.line').find('.warn').css('display','block').text('手机号码格式不对，请输入有效的手机号码！');	
				}
			}
			$.ajax({
				url : 'user.php',
				type : 'GET',
				data : {name : $(this).val()},
				success : function(data){
					//data : 1 或 0
					if(data == 1){
						$('#div1').html('可以注册');
					}
					else if(data == 0){
						$('#div1').html('不可以注册,已经有人注册过了');
					}
					$(this).next().removeClass('em_reg_error').addClass('em_reg_right');
				},
				error : function(err){
					/*if(err.status == 404){
						404.html
					}*/
					$(this).next().removeClass('em_reg_right').addClass('em_reg_error');
				}
			});
			
		});
		var bflag = true;
//		$('.get_mes_verifi').click(function(){
//			if(!bflag) return; 
//			bflag = false;
//			$(this).css('background','#d1d1d1');
//			$(this).html('<strong>60</strong>秒后可重新操作');
//			$('.yanzheng_state').css('display','block');
//			var sec = 60;
//			var This = this;
//			var sTimer = null;
//			var $strong = $(this).find('strong');
//			clearInterval(sTimer);
//			sTimer = setInterval(function(){
//				sec --;
//				$strong.text(sec);
//				if(sec==0){
//					 clearInterval(sTimer);
//					 bflag = true;
//					 $(This).html('获取短信校验码');
//					 $('.yanzheng_state').css('display','none');
//					 $(This).css('background','#f5f5f5');
//					 
//				}
//			},1000);	
//		});		
	})();
	(function(){
		var $val = $('#reg_pw').val();
		$('#reg_pw').focus(function(){
			$('#reg_pw').val('');
			$(this).get(0).type = 'password';		
			$(this).keydown(function(){
				$('.hint').find('p').css('display','none');
				$('.hint').find('.txt_hint').css('display','block');	
			});
			$('.hint').find('.txt_hint').css('display','block').text('请输入密码，长度只能在 6-16 个字符之间！');
			$(this).next().removeClass('em_reg_right em_reg_error')
		});
		$('#reg_pw').blur(function(){
			if($('#reg_pw').val() == ''){
				$(this).get(0).type = 'text';	
				$(this).val($val);
				$('.hint').find('p').css('display','none');
				
			}else{
				if($(this).val().length>16||$(this).val().length<6){
					$('.hint').find('p').css('display','none');
					$('.hint').find('.txt_hint').css('display','block').text('密码格式不正确，长度只能在 6-16 个字符之间！');
					$(this).next().removeClass('em_reg_right').addClass('em_reg_error');
				}else{
					$('.hint').find('p').css('display','none');
					$(this).next().removeClass('em_reg_error').addClass('em_reg_right');
					pwStrength($('#reg_pw').val());
				}
			}
		});
		function pwStrength(pwd){    
			
				S_level=checkStrong(pwd);    
				switch(S_level) {    
					case 1:    
						$('.hint').find('.state_hint').css('display','block').find('strong').removeClass('st').eq(0).addClass('st');
						break;    
					case 2:    
						$('.hint').find('.state_hint').css('display','block').find('strong').removeClass('st').eq(1).addClass('st');   
						break;  
					case 3:    
						$('.hint').find('.state_hint').css('display','block').find('strong').removeClass('st').eq(1).addClass('st');  
						break;    
					case 4:    
						$('.hint').find('.state_hint').css('display','block').find('strong').removeClass('st').eq(2).addClass('st');  
						break;     
				}    
		}    
		   //判断输入密码的类型    
		function CharMode(iN){    
			if (iN>=48 && iN <=57) //数字    
				return 1;    
			if (iN>=65 && iN <=90) //大写    
				return 2;    
			if (iN>=97 && iN <=122) //小写    
				return 4;    
			else    
				return 8;     
		}  
		//bitTotal函数    
		//计算密码模式    
		function bitTotal(num){    
			var modes=0;    
			for (i=0;i<4;i++){    
				if (num & 1) modes++;    
				num>>>=1;    
			}  
			return modes;    
		}  
		//返回强度级别    
		function checkStrong(sPW){
			var Modes = 0;
			for (i=0;i<sPW.length;i++){    
				//密码模式    
				Modes|=CharMode(sPW.charCodeAt(i));    
			}  
			return bitTotal(Modes);    
		};  
	
		var $confirmVal = '';
		var arr = ['请再次输入密码！', '请输入确认密码，长度只能在 6-16 个字符之间！', '2次密码输入不一致，请重新输入！'];
		$('#reg_pw_confirm').focus(function(){
			$val = $('#reg_pw_confirm').val();
			if($('#reg_pw_confirm').val() == $val){
				$('#reg_pw_confirm').val('');
				$(this).get(0).type = 'password';	
			}
			
			$('.confirm_hint').css('display','block').text(arr[0]);
			
		});
		$('#reg_pw_confirm').blur(function(){
			
			if($('#reg_pw_confirm').val() == ''){
				$(this).get(0).type = 'text';	
				$(this).val($val);
				$('.confirm_hint').css('display','none')
			}else{
				if($(this).val() != $('#reg_pw').val()){	
					$('.confirm_hint').css('display','block').text(arr[2]);
					$(this).next().removeClass('em_reg_right').addClass('em_reg_error');
				}else{
					$('.confirm_hint').css('display','none');
					$(this).next().removeClass('em_reg_error').addClass('em_reg_right');	
				}
			}
	
		});
		
	})();
	(function(){
		/*$(window).resize(function(){
			$('#mask').height($(document).height());
			$('#mask').width($(document).width());
			$('.reg_suc').css('left',($(window).width()-$('.reg_suc').width())/2+$(document).scrollLeft());
			$('.reg_suc').css('top',($(window).height()-$('.reg_suc').height())/2+$(document).scrollTop());
			$('.reg_suc').show();
			$('.reg_suc').find('.close').click(function(){
				$('#mask').hide();
				$('.reg_suc').hide();	
			});
		});*/
		
			
			/*$('#mask').height($(document).height());
			$('#mask').width($(document).width());
			if(isIe6()){
				$('.reg_suc').css('left',($(window).width()-$('.reg_suc').width())/2+$(document).scrollLeft());
				$('.reg_suc').css('top',($(window).height()-$('.reg_suc').height())/2+$(document).scrollTop());
			}else{
				$('.reg_suc').css('left',($(window).width()-$('.reg_suc').width())/2);
				$('.reg_suc').css('top',($(window).height()-$('.reg_suc').height())/2);
			}
			$('.reg_suc').show();
			$('.reg_suc').find('.close').click(function(){
				$('#mask').hide();
				$('.reg_suc').hide();	
			});
			*/
		
		function isIe6(){
				var str = window.navigator.userAgent.toLowerCase();
				if(str.indexOf('msie 6')!=-1) return true;
				return false;
		}
	})();
});
