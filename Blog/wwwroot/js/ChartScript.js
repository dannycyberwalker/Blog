function Draw(LineName,XName, YName, DivId , Data) {
    google.charts.load('current',
        { packages: ['corechart', 'line'] });
    google.charts.setOnLoadCallback(drawBasic);
    function drawBasic() {
        var data = new google.visualization.DataTable();
        data.addColumn('number', 'X');
        data.addColumn('number', LineName);

        data.addRows(Data);

        var options = {
            hAxis: {
                title: XName
            },
            vAxis: {
                title: YName
            },
            colors: ['#a52714'],
            crosshair: {
                color: '#000',
                trigger: 'selection'
            }
        };

        var chart = new google.visualization.LineChart(document.getElementById(DivId));

        chart.draw(data, options);

    }
}
