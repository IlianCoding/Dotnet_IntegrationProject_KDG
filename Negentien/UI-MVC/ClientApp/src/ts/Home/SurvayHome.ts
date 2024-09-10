import {getIdOfCurrentHtml} from "../getIdOfCurrentHtml";
import {getIdPartFromUrl} from "../getIdPartFromUrl";

document.addEventListener('keydown', function(event) {
    if (['KeyF'].includes(event.code)){
        console.log("You pressed F!");
        console.log(getFlowIdFromUrl())
        window.location.href=`/Step/FirstStep?runningFlowId=${getIdPartFromUrl("runningFlowID")}&flowId=${getFlowIdFromUrl()}`;
    }

});
function getFlowIdFromUrl(): number | null {
    const params = new URLSearchParams(window.location.search)
    const flowIdStr = params.get('flowId');

    const flowId = flowIdStr ? parseInt(flowIdStr, 10) : null;

    return flowId;
}