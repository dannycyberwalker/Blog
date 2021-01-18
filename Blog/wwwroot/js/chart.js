function DrawChart(Title, xValue, yValue) {

    var trace1 = {
        x: xValue,
        y: yValue,
        type: 'bar',
        text: yValue.map(String),
        textposition: 'auto',
        hoverinfo: 'none',
        marker: {
            color: 'rgb(252,255,7)',
            opacity: 0.8,
            line: {
                color: 'rgb(246,0,43)',
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
//Controller signature: GetStatistics(string tableName,string from, string to , int daysInOneStep)
async function getStatstics(tableName, from, to, daysInOneStep) {
    let requestUrl = `https://localhost:5001/Admin/GetStatistics?tableName=${tableName}&from=${from}&to=${to}&daysInOneStep=${daysInOneStep}`;
    let response = await fetch(requestUrl);
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
async function showStatistics(outputElementId, classNameRemoveElement, tableName, fromId, toId, daysInOneStepId) {
    let from = document.getElementById(fromId).value;
    let to = document.getElementById(toId).value;
    let daysInOneStep = document.getElementById(daysInOneStepId).value;
    let outputElement = document.getElementById(outputElementId);
    
    let data = await getStatstics(tableName, from, to, daysInOneStep);
    
    if(data !== undefined){
       let chartElement =  document.createElement('div');
       chartElement.setAttribute('id', data.title);
       outputElement.appendChild(chartElement);
       let title = data.title;
       let xValues = data.xValues;
       let yValues = data.yValues;
       DrawChart(title, xValues, yValues);

    }
    removeAfterClick(classNameRemoveElement);
}
function removeAfterClick(className) {
    let elements = document.getElementsByClassName(className);
    for (let i = 0; i < elements.length; i ++){
        elements[i].remove();
    }
}