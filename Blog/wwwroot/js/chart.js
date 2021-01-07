function DrawChart(Title, xValue, yValue) {

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
    //Title in this method call is the id of div where chart renders.
    Plotly.newPlot(Title, data, layout);
}
async function getStatstics() {
    let response = await fetch('https://localhost:5001/Admin/GetStatistics');
    let data;
    if (response.ok) {
        data = await response.json();
    } 
    else {
        data = [];
        console.log("Error HTTP: " + response.status);
    }
    return data;
}
async function showStatistics(elementId) {
    let element = document.getElementById(elementId);
    let data = await getStatstics();
    if(data !== undefined){
        for(let i = 0; i < data.length;i++){
            let chartElement =  document.createElement('div');
            chartElement.setAttribute('id', data[i].title);
            element.appendChild(chartElement);
            let title = data[i].title;
            let xValues = data[i].xValues;
            let yValues = data[i].yValues;
            DrawChart(title, xValues, yValues);
        }
    }
    removeAfterClick();
}
function removeAfterClick() {
    let elements = document.getElementsByClassName('remove-after-click');
    for (let i = 0; i < elements.length; i ++){
        elements[i].remove();
    }
}