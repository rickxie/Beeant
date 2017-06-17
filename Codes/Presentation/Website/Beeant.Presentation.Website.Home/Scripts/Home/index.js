$(function(){
    $('#flip').fullpage({
        sectionsColor: ['#f7f7f7', '#fff', '#f7f7f7', '#fff'],
        'navigation': true,
        afterLoad: function(anchorLink, index){
            switch (index){
                case 1:
                    $('.floor1 p').addClass("animated rollIn");
                    break;
                case 2:
                    $('.floor2 h3 div').addClass("animated pulse");
                    $('.floor2 .profile dd').addClass("animated swing");
                    break;
                case 3:
                    $('.floor3 .profile3-img').addClass("animated rubberBand");
                    break;
                case 4:
                    $('.floor4 .profile3-img').addClass("animated shake");
                    $('.floor4 .profile3-r p').addClass("animated bounceInLeft");
                    break;
            }
        },
        onLeave: function(index, direction){
            switch (index){
                case 1:
                    $('.floor1 p').removeClass("animated rollIn");
                    break;
                case 2:
                    $('.floor2 h3 div').removeClass("animated pulse");
                    $('.floor2 .profile dd').removeClass("animated swing");
                    break;
                case 3:
                    $('.floor3 .profile3-img').removeClass("animated rubberBand");
                    break;
                case 4:
                    $('.floor4 .profile3-img').removeClass("animated shake");
                    $('.floor4 .profile3-r p').removeClass("animated bounceInLeft");
                    break;
            }
        }
    });
});