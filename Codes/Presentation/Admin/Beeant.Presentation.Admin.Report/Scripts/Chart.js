(function() {
    require.config({
        paths: {
            echarts: '/Scripts/Plug/Echarts/build/dist'
        }
    });
    require(
        [
            'echarts',
            'echarts/chart/bar',
            'echarts/chart/line',
            'echarts/chart/pie'
        ], function (ec) {
            var myChart = ec.init(document.getElementById('divChart'));
            myChart.setOption(option);
        }
    );
})()

 
  