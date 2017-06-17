


$(function() {
	// ie 9 以下CSS3	 start   
    if (window.PIE) {
        $('.rounded').each(function() {PIE.attach(this);});
		
		$("a").each(function() {PIE.attach(this);});
		$('img').each(function() {PIE.attach(this);});
	
    }
	// ie 9 以下CSS3 end	
	
	 //在线客服下拉
   $(".qqline").each(function(){
	   var timoutid						   
	   $(this).hover(function(){
		  var $liNode = $(this);
		  timoutid = setTimeout(function(){
			  $liNode.addClass("hover");
			  $liNode.children(".qqlist").slideDown(200);
		  },300);
		},function(){
			$(this).removeClass("hover");
			$(this).children(".qqlist").slideUp(0);
			clearTimeout(timoutid);
		});						   
	
	});
   
    //弹窗
   $(".newcp_2,.newAdd").click(function(evt) {
       var evt = evt || window.event;
		evt.preventDefault();
       $(".win_popup").fadeIn();
   });
   
   //关闭弹窗
   function colsePopup() {
       $(".win_popup").fadeOut();
   }
   $(".close,.btn_clear,*[name=close]").click(function() {
       colsePopup();
   });
	
    //是否有滚动条
	var windowH=$(window).height();
	var docH = $(document.body).height();
    if(windowH > docH){
		  $("#footer").addClass("fixted");		
	}

	// 返回顶部
    $(".backTop").click(function(evt) {
        var evt = evt || window.event;
        evt.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, 120);
    });

    /*
	function getBrowserDim(){
	   var windowobj = $(window);
       var browserwidth = windowobj.width()/2;
       var scrollLeft = windowobj.scrollLeft();
	   var b=$(".w1000").width()/2;
	   var Left = scrollLeft+browserwidth+b;
	   return Left;
	}

	//$(".backToTop").css("left",getBrowserDim());
	$(".smBtn").css("left",getBrowserDim())
	$(".backToTop").hover(function(){$(this).css("background-position","0px 0px")},function(){$(this).css("background-position","-45px 0px")});
	
*/
});
