import {Chart, ChartConfiguration} from 'chart.js/auto';
import "../../scss/statistics.scss"
import {getIdOfCurrentHtml} from "../getIdOfCurrentHtml";
import {Modal} from 'bootstrap';

const colorPalette = [
    'rgba(255, 99, 132, 0.5)',
    'rgba(54, 162, 235, 0.5)',
    'rgba(75, 192, 192, 0.5)',
    'rgba(255, 159, 64, 0.5)',
    'rgba(153, 102, 255, 0.5)',
    'rgba(255, 205, 86, 0.5)',
    'rgba(201, 203, 207, 0.5)',
    'rgba(255, 204, 255, 0.5)',
    'rgba(0, 128, 128, 0.5)',
    'rgba(0, 255, 255, 0.5)'
];

document.addEventListener("DOMContentLoaded", AddChartsToPage)
document.addEventListener("DOMContentLoaded", updateMostPopularAnswerPerQuestionEventListener)
document.addEventListener("DOMContentLoaded", addButtonEventListener)


function AddChartsToPage() {
    const canvas = document.getElementById('myChart') as HTMLCanvasElement;
    const ctx = canvas.getContext('2d') as CanvasRenderingContext2D;

    const canvasFlow = document.getElementById('runningFlow-data') as HTMLCanvasElement;
    const ctxFlow = canvasFlow.getContext('2d') as CanvasRenderingContext2D;


    if (ctx) {
        gatherDataParticipants(ctx)
    }

    if (ctxFlow) {
        gatherDataRunningFlows(ctxFlow)
    }

    addThemesAndStepChartToThePage()

    addMostPopularAnswerTable()
}


function updateMostPopularAnswerPerQuestionEventListener() {
    const selectElement = document.getElementById('flow-id') as HTMLSelectElement;

    selectElement.addEventListener("change", addMostPopularAnswerTable)
}

function getFlowId(): string {
    const selectElement = document.getElementById('flow-id') as HTMLSelectElement;
    return selectElement.value;
}

function addMostPopularAnswerTable() {
    const flowId = getFlowId();

    if (flowId) {
        fetch("/api/Statistics/MostPopularAnswerPerQuestionPerFlow/" + flowId, {

            method: "GET"
        })
            .then(response => {
                if (!response.ok) {
                    alert("Failed to get data!");
                }

                if (response.status === 200) {
                    return response.json() as Promise<QuestionWithMostChosenAnswer[]>;
                }

            })
            .then(dataList => {
                if (dataList) {
                    createQuestionWithAnswerRows(dataList)
                }
            })
            .catch(error => {
                console.error("Error:", error);
            });
    }
}

function createQuestionWithAnswerRows(questionWithMostChosenAnswerList: QuestionWithMostChosenAnswer[]) {
    (document.querySelector("#popular-answer-table tbody") as HTMLElement).innerHTML = "";
    questionWithMostChosenAnswerList
        .forEach(questionWithMostChosenAnswer => addTheaterToSelection(questionWithMostChosenAnswer));
}

function addTheaterToSelection(questionWithMostChosenAnswer: QuestionWithMostChosenAnswer) {
    const theaterTableBody = document.querySelector("#popular-answer-table tbody") as HTMLElement;

    theaterTableBody.insertAdjacentHTML('beforeend', `<tr>
    <td>${questionWithMostChosenAnswer.question}</td>
    <td>${questionWithMostChosenAnswer.mostPopularAnswer ?? "No one has answered this question"}</td>
      </tr>`);
}


function addThemesAndStepChartToThePage() {

    const canvasTheme = document.getElementById('step-data') as HTMLCanvasElement;
    const ctxTheme = canvasTheme.getContext('2d') as CanvasRenderingContext2D;
    if (ctxTheme) {
        gatherDataThemesAndStep(ctxTheme)
    }
}

function updateThemesAndStepChartEventListener(chart: Chart) {
    const selectElement = document.getElementById('project-id') as HTMLSelectElement;

    selectElement.addEventListener("change", () => updateThemeAndStepChart(chart))
}

function updateThemeAndStepChart(chart: Chart) {
    chart.destroy();
    addThemesAndStepChartToThePage()
}

function getProjectId(): string {
    const selectElement = document.getElementById('project-id') as HTMLSelectElement;
    const projectId = selectElement.value;

    console.log(projectId);

    return projectId;
}

function gatherDataThemesAndStep(ctxTheme: CanvasRenderingContext2D) {

    const projectId = getProjectId();

    if (projectId) {
        fetch("/api/Statistics/ThemesAndAmountOfStepsAttachedToIt/" + projectId, {

            method: "GET"
        })
            .then(response => {
                if (!response.ok) {
                    alert("Failed to get data!");
                }

                if (response.status === 200) {
                    return response.json() as Promise<ThemeAndAmountOfStepsAttached[]>;
                }

            })
            .then(dataList => {
                const themeNames: string[] = [];
                const amountOfStepsPerTheme: number[] = [];

                if (dataList) {
                    dataList.forEach(data => {
                        themeNames.push(data.themeName);
                        amountOfStepsPerTheme.push(data.amountOfStepsAttached)
                    })
                }

                makeThemeChart(ctxTheme, themeNames, amountOfStepsPerTheme);
            })
            .catch(error => {
                console.error("Error:", error);
            });
    }
}

function makeThemeChart(ctxTheme: CanvasRenderingContext2D, themeNames: string[], amountOfStepsPerTheme: number[]) {
    const sourceColors: string[] = [];

    for (let i = 0; i < amountOfStepsPerTheme.length; i++) {

        const colorIndex = i % colorPalette.length;
        sourceColors.push(colorPalette[colorIndex]);
    }

    const data = {
        labels: themeNames,
        datasets: [{
            data: amountOfStepsPerTheme,
            backgroundColor: sourceColors,
            borderWidth: 1
        }]
    };


    const config: ChartConfiguration = {
        type: 'polarArea',
        data: data,
        options: {
            scales: {},
            responsive: true
        }
    };


    const chart = new Chart(ctxTheme, config);

    updateThemesAndStepChartEventListener(chart)
}

function getYear() {
    const selectElement = document.getElementById('year-runningFlows') as HTMLSelectElement;
    return selectElement.value;
}


function changeTitleRunningFlows(year: string) {
    const selectElement =
        document.getElementById('running-flows-title') as HTMLElement;

    selectElement.innerHTML = `Amount of Flows Initiated the Year ${year}`
}

function updateRunningFlowDataEventListener(chart: Chart) {
    const selectElement =
        document.getElementById('year-runningFlows') as HTMLSelectElement;

    selectElement.addEventListener("change", () => updateRunningFlowDataChart(chart))
}

function updateRunningFlowDataChart(chart: Chart) {
    chart.destroy();
    addRunningFlowDataChartToThePage()
}

function addRunningFlowDataChartToThePage() {

    const canvasTheme = document.getElementById('runningFlow-data') as HTMLCanvasElement;
    const ctxRunningFlow = canvasTheme.getContext('2d') as CanvasRenderingContext2D;
    if (ctxRunningFlow) {
        gatherDataRunningFlows(ctxRunningFlow)
    }
}

function gatherDataRunningFlows(ctx: CanvasRenderingContext2D) {
    const id: number = getIdOfCurrentHtml()
    const year = getYear()
    changeTitleRunningFlows(year)

    fetch("/api/Statistics/RunningFlowDataOfPlatform/" + id + "?year=" + year, {

        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                alert("Failed to get data!");
            }

            if (response.status === 200) {
                return response.json() as Promise<RunningFlowsPerFlow[]>;
            }

        })
        .then(dataList => {
            const labels: string[] = [];
            let datasets: any[] = [];

            if (dataList && dataList[0]) {
                dataList[0].runningFlowsPerFlowPerMonth
                    .forEach(rf => labels.push(rf.month));
                datasets = makeDataSets(dataList)
                makeFlowAreaChart(ctx, datasets, labels)
            } 


        })
        .catch(error => {
            console.error("Error:", error);
        });
}

function makeDataSets(dynamicData: RunningFlowsPerFlow[]): any {
    let counterColor = 0;
    const datasets = dynamicData.map(data => (
        {
            label: data.flowName,
            data: data.runningFlowsPerFlowPerMonth.map(rf => rf.amountOfRunningFlowsCreated),
            backgroundColor: colorPalette[counterColor++ % colorPalette.length],
            borderWidth: 1,
            fill: true,
        }
    ));

    return datasets

}

function makeFlowAreaChart(ctx: CanvasRenderingContext2D, datasets: any, labels: string[]) {

    const config = {
        type: 'line',
        data: {
            labels: labels,
            datasets: datasets,
        },
        options: {
            scales: {},
            responsive: true
        },
    };

    const myChart = new Chart(ctx, config);

    updateRunningFlowDataEventListener(myChart)
}

function gatherDataParticipants(ctx: CanvasRenderingContext2D) {
    const id: number = getIdOfCurrentHtml()

    fetch("/api/Statistics/ParticipantsPerFlow/" + id, {

        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                alert("Failed to get data!");
            }

            if (response.status === 200) {
                return response.json() as Promise<ParticipantsPerFlow[]>;
            }

        })
        .then(dataList => {
            const flowNames: string[] = [];
            const participantsCounts: number[] = [];

            if (dataList) {
                dataList.forEach((item) => {
                    flowNames.push(item.flowName);
                    participantsCounts.push(item.participants);
                });
            } else {
                showWarningNoData()
            }

            makeChartParticipants(ctx, flowNames, participantsCounts)
        })
        .catch(error => {
            console.error("Error:", error);
        });
}


function makeChartParticipants(ctx: CanvasRenderingContext2D, flowNames: string[],
                               participantsCounts: number[]) {
    const config: ChartConfiguration = {
        type: 'bar',
        data: {
            labels: flowNames,
            datasets: [
                {
                    label: 'Participants Count',
                    data: participantsCounts,
                    borderWidth: 1,
                    backgroundColor: 'rgba(0, 255, 0, 0.5)'
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


function addButtonEventListener() {
    (document.getElementById('download-all-answers') as HTMLButtonElement)
        .addEventListener('click', getAllAnswersData)
}

function getFormattedDate(): string {
    const date = new Date();
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based, so add 1
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

function getAllAnswersData() {
    const flowId = getFlowId()
    const fileName = `flow-${flowId}-all-answers-` + getFormattedDate() + ".csv"


    fetch(" /api/Statistics/AllAnswersInCsvFormat/" + flowId, {

        method: "GET"
    })
        .then(response => {
            if (!response.ok) {
                alert("Failed to get data!");
            }

            if (response.status === 200) {
                return response.json() as Promise<any>;
            } else if (response.status === 204) {
                showWarningNoData()
            }

        })
        .then(data => {
            if (data) {
                downloadCsvFileOfData(data.data, fileName)
            }

        })
        .catch(error => {
            console.error("Error:", error);
        });

}

function showWarningNoData() {
    const modal = document.getElementById('exampleModal');
    if (modal) {
        const modalInstance = new Modal(modal);
        modalInstance.show();
    }
}

function downloadCsvFileOfData(data: string, fileName: string) {

    const blob = new Blob([data], {type: 'text/csv'});
    const url = URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

    URL.revokeObjectURL(url);
}