function Draw(Title, xValue, yValue) {

    var trace1 = {
        x: xValue,
        y: yValue,
        type: 'bar',
        text: yValue.map(String),
        textposition: 'auto',
        hoverinfo: 'none',
        marker: {
            color: 'rgb(0,0,0)',
            opacity: 0.8,
            line: {
                color: 'rgb(0,0,0)',
                width: 1.5
            }
        }
    };
    var data = [trace1];
    var layout = {
        title: Title,
        barmode: 'stack'
    };

    Plotly.newPlot(Title, data, layout);
}