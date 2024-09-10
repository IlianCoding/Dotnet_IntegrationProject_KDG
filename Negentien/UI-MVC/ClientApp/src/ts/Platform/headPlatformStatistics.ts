import {Chart, ChartConfiguration} from 'chart.js/auto';
import "../../scss/statistics.scss"
import {ProjectDataPerSharingPlatform} from "../models/ProjectDataPerSharingPlatform.interface";


document.addEventListener("DOMContentLoaded", AddChartsToPage)
function AddChartsToPage() {
    const canvas = document.getElementById('myChart') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d') as CanvasRenderingContext2D;

    const canvasFlow = document.getElementById('avg-flow') as HTMLCanvasElement;
    const ctxFlow = canvasFlow.getContext('2d') as CanvasRenderingContext2D;
    
    if (ctx) {
        gatherDataProject(ctx)
    }
    
    if (ctxFlow){
        gatherDataFlows(ctxFlow)
    }
}

function gatherDataProject(ctx: CanvasRenderingContext2D) {

    fetch("/api/Statistics/ProjectDataPerSharingPlatform", {

        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                alert("Failed to get data!");
            }

            if (response.status === 200) {
                return response.json() as Promise<ProjectDataPerSharingPlatform[]>;
            }

        })
        .then(dataList => {
            const sharingPlatformNames: string[] = [];
            const activeProjectCounts: number[] = [];
            const totalProjectCounts: number[] = [];

            if (dataList) {
                dataList.forEach((item) => {
                    sharingPlatformNames.push(item.sharingPlatformName);
                    activeProjectCounts.push(item.activeProjectCount);
                    totalProjectCounts.push(item.totalProjectCount);
                });
            }

            makeChartProject(ctx,sharingPlatformNames,activeProjectCounts,totalProjectCounts)
        })
        .catch(error => {
            console.error("Error:", error);
        });
}


function gatherDataFlows(ctx: CanvasRenderingContext2D) {

    fetch("/api/Statistics/AvgFlowPerProjectPerSharingPlatform", {

        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                alert("Failed to get data!");
            }

            if (response.status === 200) {
                return response.json() as Promise<AvgFlowPerProjectPerSharingPlatformData[]>;
            }

        })
        .then(dataList => {
            const sharingPlatformNames: string[] = [];
            const AvgFlowPerProjectPerPlatform: number[] = [];
         

            if (dataList) {
                dataList.forEach((item) => {
                    sharingPlatformNames.push(item.sharingPlatformName);
                    AvgFlowPerProjectPerPlatform.push(item.avgFlowsPerProject.valueOf());
                });
            }

            makeChartFlow(ctx,sharingPlatformNames,AvgFlowPerProjectPerPlatform)
        })
        .catch(error => {
            console.error("Error:", error);
        });
}

function makeChartProject(ctx: CanvasRenderingContext2D, sharingPlatformNames: string[],
                   activeProjectCounts: number[], totalProjectCounts: number[]) {
    const config: ChartConfiguration = {
        type: 'bar',
        data: {
            labels:sharingPlatformNames,
            datasets: [
                {
                    label: 'Total Projects',
                    data: totalProjectCounts,
                    borderWidth: 1,
                    backgroundColor: 'rgba(54, 162, 235, 0.5)'
                }, {
                    label: '# Active Projects',
                    data: activeProjectCounts,
                    borderWidth: 1,
                    backgroundColor: 'rgba(255, 206, 86, 0.5)'
                }
            ],
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                },
            },
            responsive: true
        },
    };

    new Chart(ctx, config);
}

function makeChartFlow(ctx: CanvasRenderingContext2D, sharingPlatformNames: string[],
                       AvgFlowPerProjectPerPlatform: number[], ) {
    const config: ChartConfiguration = {
        type: 'bar',
        data: {
            labels:sharingPlatformNames,
            datasets: [
                {
                    label: 'Avg Flows Per Project Per SharingPlatform',
                    data: AvgFlowPerProjectPerPlatform,
                    borderWidth: 1,
                    backgroundColor: 'rgba(153, 102, 255, 0.5)'
                }
            ],
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                },
            },
            responsive: true
        },
    };

    new Chart(ctx, config);
}

//
//
// const timePeriods = ['Jan', 'Feb', 'Mar', 'Apr', 'May']; // Time intervals (e.g., months)
// const averages = [1.5, 1.8, 1.2, 2.0, 1.7]; // Average number of new running flows per flow per time interval
//
// // Chart data
// const data = {
//     labels: timePeriods,
//     datasets: [{
//         label: 'Average New Running Flows per Flow per Time Interval',
//         data: averages,
//         backgroundColor: 'rgba(255, 99, 132, 0.5)', // Red color with transparency
//         borderWidth: 1
//     }]
// };
//
// // Chart options
// const options = {
//     scales: {
//         x: {
//             title: {
//                 display: true,
//                 text: 'Time'
//             }
//         },
//         y: {
//             title: {
//                 display: true,
//                 text: 'Average New Running Flows per Flow'
//             }
//         }
//     },
//     plugins: {
//         title: {
//             display: true,
//             text: 'Average New Running Flows per Flow over Time'
//         }
//     }, responsive: true
// };
//
// // Create the chart
// const ctxRun = document.getElementById('runningFlows-flow') as HTMLCanvasElement;
// const myChart = new Chart(ctxRun, {
//     type: 'line', // Use 'bar' for a bar chart
//     data: data,
//     options: options
// });


// Define the data for your chart
// const data = {
//     labels: [
//         'January',
//         'February',
//         'March',
//         'April'
//     ],
//     datasets: [{
//         type: 'bar',
//         label: 'Bar Dataset',
//         data: [10, 20, 30, 40],
//         borderColor: 'rgb(255, 99, 132)',
//         backgroundColor: 'rgba(255, 99, 132, 0.2)'
//     }, {
//         type: 'line',
//         label: 'Line Dataset',
//         data: [50, 50, 50, 50],
//         fill: false,
//         borderColor: 'rgb(54, 162, 235)'
//     }]
// };
//
//
// // Create the bar chart
// const config = {
//     type: 'scatter',
//     data: data,
//     options: {
//         scales: {
//             y: {
//                 beginAtZero: true
//             }
//         }
//     }
// };