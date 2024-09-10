import {types} from "sass";
import Boolean = types.Boolean;

export function addRunningFlow(flowId: number, attendantName: string, isKiosk:boolean) {
    console.log(attendantName.length)
    if (attendantName.length == 0) {
        return;
    }
    console.log("attendantName = " + attendantName)
    let currentdateTime = new Date();
    fetch('/api/RunningFlows', {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            "RunningFlowTime": currentdateTime.toISOString(),
            "State": 0,
            "CurrentFlowId": flowId,
            "CreatedAttendantName":attendantName,
            "IsKiosk": isKiosk
        })
    }).then(function (response) {
        if (!response.ok) {
            throw new Error("Add RunningFlow failed: " + response.status)
        }
        return response.json();
    }).then(function (runningFlowDto) {
        window.location.reload();
    }).catch(function (error) {
        console.error(error)
    })
}