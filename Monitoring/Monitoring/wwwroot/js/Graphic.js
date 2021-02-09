function getData(id) {
    var timeFormat = 'mm/DD/yyyy HH:mm';
    $.ajax({
        type: 'GET',
        url: "http://localhost:3000/Graphic/GetDataForGraphic/" + id,
        timeout: 10000,
        success: function (data) {
            console.log(data);
            var ctx = document.getElementById('myChart').getContext('2d');
            var myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: data.labels,
                    datasets: [{
                        label: data.name,
                        data: data.values,
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255, 99, 132, 1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        fill: false,
                    }],
                },
                options: {
                    legend: {
                        labels: {
                            fontColor: "black",
                            fontSize: 18
                        }
                    },
                    scales: {
                        xAxes: [{
                            type: 'time',
                            time: {
                                parser: 'MM/DD/YYYY HH:mm',
                                tooltipFormat: 'll HH:mm',
                                unit: 'minute',
                                unitStepSize: 1,
                            },
                            scaleLabel: {
                                display: true,
                                labelString: 'Date'
                            },
                            gridLines: {
                                display: true,
                                color: "#000000"
                            },
                            ticks: {
                                fontColor: "black",
                                fontSize: 18,
                                beginAtZero: true
                            }
                        }],
                        yAxes: [{
                            scaleLabel: {
                                display: true,
                                labelString: 'Value'
                            },
                            gridLines: {
                                display: true,
                                color: "#000000"
                            },
                            ticks: {
                                fontColor: "black",
                                fontSize: 18,
                                beginAtZero: true
                            }
                        }],
                    },
                },
            });
        }
    })
};