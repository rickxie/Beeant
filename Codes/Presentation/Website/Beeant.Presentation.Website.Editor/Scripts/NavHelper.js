/*
 * NavHelper 0.1
 * Copyright (c) 2015 yvh302@163.com
 * Date 2015-08-10
*/
(function($){ 
   $.fn.extend({
	NavHelper: function (opt, callback) { 
     if (!opt) var opt = {}; 
	 var id;
     var $this =[];
     var slideBox = "";
	 var NavClass = opt.NavClass ? $(opt.NavClass) : $(".NavClass");//滚动列表集合，默认NavClass;
	 var TopFixtedH = opt.TopFixtedH ? parseInt(opt.TopFixtedH, 10) : 0;//距离头部固定条高度；如果没有固定条，默认为0；
	 var LiH = opt.LiH ? parseInt(opt.LiH, 10) : 0;//距离li高度；如果没有固定条，默认为0；
	 var speed = opt.speed ? parseInt(opt.speed, 10) : 400;// 滚动动画速度；
	 var aClass = opt.aClass ? opt.aClass : "aClass";//指定a class；
	 var SeletedClass = opt.SeletedClass ? opt.SeletedClass : "Seleted";//滚动到当前显示class
     var num = NavClass.length; //滚动列表集合个数
	 var fisrt = true;
	 var Noscroll = false;
	// console.log(NavClass)

    //锚点函数
     function Scrolla(ID){
	     var top = $(ID).offset().top-TopFixtedH;
         $("html,body").animate({scrollTop:top},speed,function(){fisrt = true;});
     }
	 
     NavClass.each(function(index){		
	     id = $(this).attr("id");
	     $this[index] =$(this).offset().top;
	     slideBox += "<li><a class='"+aClass+"' href='#"+id+"'>"+$(this).text()+"</a></li>";
		// console.log("我是ID"+index+"的高"+$this[index])
     });
     $(".rightBar").append("<ul class='letter'>"+slideBox+"</ul>");
     //console.log(num)	
	  var scroll_top = $(document).scrollTop(); 
	  if(scroll_top==0){$(".letter li").eq(0).addClass(SeletedClass).siblings().removeClass(SeletedClass);}
     $(document).scroll(function(){	
		Noscroll = true;						 
	    var sH=	$(window).scrollTop()+TopFixtedH;
	    for (var i=0; i<num; i++){
			 ///console.log("我是ID："+i+"高"+sH)
		    if($this[i]<=sH && sH<$this[i+1] && fisrt){
				$(".letter li").eq(i).addClass(SeletedClass).siblings().removeClass(SeletedClass);
			}else if($this[i]<=sH && fisrt){
				$(".letter li").eq(i).addClass(SeletedClass).siblings().removeClass(SeletedClass);
			}
	    }
		
		
	//console.log(sH)		
    //console.log($("#pd003").offset().top-$(".iscoll2").height());
      });
      var hash;   
      hash=window.location.hash;
      for (var i=0; i<num; i++){
	      if(hash==$(".letter li").eq(i).children("a").attr("href")){
		    Scrolla(hash);
			
          }
      }
      $("."+aClass).on("click",function(){
		   fisrt=false;
		   var ID = $(this).attr("href");	
		   $(this).parent().addClass(SeletedClass).siblings().removeClass(SeletedClass);
		   Scrolla(ID);
		   
      });
	}
  }); 
})(jQuery);

//调用方法
//$("#ass").NavHelper({NavClass:".NavClass",TopFixtedH:0,SeletedClass:"hover",aClass:"aclick",speed:300});