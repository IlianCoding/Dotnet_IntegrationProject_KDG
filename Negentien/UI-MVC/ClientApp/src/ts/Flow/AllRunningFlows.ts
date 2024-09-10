const attendantNameSelect = document.getElementById('attendantNameSelect') as HTMLSelectElement
const flowSelect = document.getElementById('flowSelect') as HTMLSelectElement
const runningFlowTBody = document.getElementById('runningFlowBody') as HTMLElement


class AttendantFlowsDto {
    public attendantName: string;
    public currentFlowsIds: number[];

    constructor(attendantName: string, currentFlowsIds: number[]) {
        this.attendantName = attendantName;
        this.currentFlowsIds = currentFlowsIds;
    }
}

class RunningFlow {
    public Id: number;
    public RunningFlowTime: Date;
    public CreatedAttendantName: string;
    public CurrentFlowId: number;
    public IsKiosk: boolean;

    constructor(id: number, runningFlowTime: Date, createdAttendantName: string, currentFlowId: number, isKiosk: boolean) {
        this.Id = id;
        this.RunningFlowTime = runningFlowTime;
        this.CreatedAttendantName = createdAttendantName;
        this.CurrentFlowId = currentFlowId;
        this.IsKiosk = isKiosk;
    }
}


let attendantFlowsList: AttendantFlowsDto[] = [];
let RunningFlowList: RunningFlow[] = [];

function loadAllAttendantWithCurrentFlows() {
    fetch('../../api/RunningFlows/AttendantNamesAndCurrentFlow', {
        headers: {
            "Accept": "application/json"
        }
    }).then(response => {
        if (!response.ok) {
            throw new Error("Load all attendantnames failed: " + response.status)
        }
        return response.json();
    }).then(attendants => {
        attendants.forEach((item: { attendantName: string, currentFlowsIds: number[] }) => {
            const attendantName = item.attendantName;
            const currentFlowsIds = item.currentFlowsIds;

            const attendantFlowsDto = new AttendantFlowsDto(attendantName, currentFlowsIds);
            attendantFlowsList.push(attendantFlowsDto);
        })
    }).then(() => {
        fillattendantNameSelect();
    }).catch(error => {
        console.error("loading attendantsNames with flow failed:", error);
    });
}

function fillattendantNameSelect() {
    if (attendantFlowsList != null) {
        if (attendantNameSelect != null) {
            attendantNameSelect.innerHTML = `
                <option value=""></option>`
            for (const attendant of attendantFlowsList) {
                attendantNameSelect.innerHTML += `
                    <option value="${attendant.attendantName}">${attendant.attendantName}</option>`
            }
        }
    }
}

function loadAllRunnningFlowWithNotCloseStatus() {
    fetch('../../api/RunningFlows/RunningFlows', {
        headers: {
            "Accept": "application/json"
        }
    }).then(response => {
        if (!response.ok) {
            throw new Error("Load all attendantnames failed: " + response.status)
        }
        return response.json();
    }).then(runningFlow => {
        runningFlow.forEach((item: {
            id: number,
            runningFlowTime: Date,
            createdAttendantName: string,
            currentFlowId: number;
            isKiosk: boolean
        }) => {
            RunningFlowList.push(new RunningFlow(item.id, item.runningFlowTime, item.createdAttendantName, item.currentFlowId, item.isKiosk))
        })
    })
}

function runningFlowLayout(attendantName?: string, flowId?: number) {

    if (!runningFlowTBody) {
        return;  // Stop de functie als tbody niet bestaat
    }

    runningFlowTBody.innerHTML = '';
    let index = 1;
    for (const item of RunningFlowList) {
        const matchesAttendantName = !attendantName || item.CreatedAttendantName === attendantName;
        const matchesFlowId = !flowId || item.CurrentFlowId === flowId;

        if (matchesAttendantName && matchesFlowId) {
            runningFlowTBody.innerHTML += `
                <tr>
                    <td>${index}</td>
                    <td>${item.RunningFlowTime}</td>
                    <td>${item.CreatedAttendantName}</td>
                    <td>${item.IsKiosk ? "Kiosk mode" : "Ai mode"}</td>
                    <td><a class="nav-link" href="/SurveyHome/IndexSurveyHome?runningFlowId=${item.Id}&flowId=${item.CurrentFlowId}">Survey</a></td>
                </tr>`;
            index++;
        }
    }
}


window.addEventListener("DOMContentLoaded", () => {
    loadAllAttendantWithCurrentFlows();
    loadAllRunnningFlowWithNotCloseStatus();
    if (attendantNameSelect) {
        attendantNameSelect.addEventListener("input", () => {
            for (const item of attendantFlowsList) {
                if (attendantNameSelect.value == item.attendantName) {
                    runningFlowLayout(attendantNameSelect.value);
                    flowSelect.innerHTML = `
                <option value=""></option>`
                    for (const id of item.currentFlowsIds) {
                        GetFlowName(id)
                            .then(flowName => {
                                console.log("Flow name:", flowName);

                                flowSelect.innerHTML += `
                <option value="${id}">${flowName}</option>`
                            })
                            .catch(error => {
                                console.error("Error:", error);
                            });
                    }
                }
            }
        })
    }
    if (flowSelect) {
        flowSelect.addEventListener("click", () => {
            runningFlowLayout(attendantNameSelect.value, Number(flowSelect.value));
        })
    }
})

function GetFlowName(flowId: Number): Promise<string> {
    return new Promise((resolve, reject) => {
        fetch(`/api/Flows/${flowId}`, {
            method: "GET"
        }).then(response => {
            if (!response.ok) {
                console.error("GetFlowName with id: " + flowId + " doesn't exist.");
                reject("Flow not found");
            }
            return response.json();
        }).then(flowDto => {
            resolve(flowDto.flowName);
        }).catch(error => {
            console.error(error);
            reject(error);
        });
    });
}



