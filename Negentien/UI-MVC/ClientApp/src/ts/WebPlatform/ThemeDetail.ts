import {Url} from "../models/Url.interface";
import {getUrl} from "../files/fileManaging";

const commentsDiv = document.getElementById('comments');
const themeId: HTMLInputElement | null = document.querySelector(`.theme-id`) as HTMLInputElement;
const submitCommentButton = document.getElementById('submitComment');
const commentText: HTMLInputElement | null = document.getElementById(`newComment`) as HTMLInputElement;

let likedComments: number[] = JSON.parse(localStorage.getItem('likedComments') || '[]');

submitCommentButton?.addEventListener('click', () => {
    submitComment();
});

document.addEventListener('DOMContentLoaded', () => {
    loadComments();
});

function addEventListenerSubmitReply() {
    const replyButtons = document.querySelectorAll('[id^="replyButton_"]');

    replyButtons.forEach((button) => {
        button.addEventListener('click', () => {
            const commentId = parseInt(button.id.split('replyButton_')[1]);
            const replyText = document.getElementById(`replyText_${commentId}`) as HTMLInputElement;
            submitReply(commentId, replyText.value);
        });
    });
}

function likeHandler(this: HTMLButtonElement) {
    const button = this as HTMLButtonElement;
    const commentId = parseInt(button.id.split('likeButton_')[1]);
    button.innerText = '♥';
    button.id = `unlikeButton_${commentId}`;
    button.removeEventListener("click", likeHandler);
    button.addEventListener("click", unlikeHandler);
    likeComment(commentId);
}

function unlikeHandler(this: HTMLButtonElement) {
    const button = this as HTMLButtonElement;
    const commentId = parseInt(button.id.split('unlikeButton_')[1]);
    button.innerText = '♡';
    button.id = `likeButton_${commentId}`;
    button.removeEventListener("click", unlikeHandler);
    button.addEventListener("click", likeHandler);
    unlikeComment(commentId);
}

function addEventListenerLike() {
    const likeButtons = document.querySelectorAll('[id^="likeButton_"]') as NodeListOf<HTMLButtonElement>;
    likeButtons.forEach((button) => {
        button.addEventListener('click', likeHandler);
    });
}

function addEventListenerUnlike() {
    const unlikeButtons = document.querySelectorAll('[id^="unlikeButton_"]') as NodeListOf<HTMLButtonElement>;
    unlikeButtons.forEach((button) => {
        button.addEventListener('click', unlikeHandler);
    });
}

function likeComment(commentId: number) {
    fetch(`/api/Comments/LikeComment/${commentId}`, {
        method: 'PUT',
        headers: {
            Accept: 'application/json'
        }
    }).then(r => {
        if (!r.ok) {
            return r.json().then(error => {
                let errorMessage = "";
                error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                alert(errorMessage);
            });
        }
        r.json().then(data => {
            const likeCounter = document.getElementById(`likeCounter_${commentId}`) as HTMLParagraphElement;
            likedComments.push(commentId);
            localStorage.setItem('likedComments', JSON.stringify(likedComments));
            likeCounter.innerText = data.likes;
            
        });
    }).catch(error => {
        console.log(error.message)
    });
}

function unlikeComment(commentId: number) {
    fetch(`/api/Comments/UnlikeComment/${commentId}`, {
        method: 'PUT',
        headers: {
            Accept: 'application/json'
        }
    }).then(r => {
        if (!r.ok) {
            return r.json().then(error => {
                let errorMessage = "";
                error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                alert(errorMessage);
            });
        }
        r.json().then(data => {
            const likeCounter = document.getElementById(`likeCounter_${commentId}`) as HTMLParagraphElement;
            likedComments = likedComments.filter((id: number) => id !== commentId);
            localStorage.setItem('likedComments', JSON.stringify(likedComments));
            likeCounter.innerText = data.likes;
        });
    }).catch(error => {
        console.log(error.message)
    });
}

function showComment(comment: {
    id: number;
    user: { firstName: any; lastName: any; } | null;
    text: any;
    subComments: any[];
    likes: number;
    isLikedByCurrentUser: boolean;
    objectName: string;
}) {
    
    let commentDiv = document.createElement('div');
    commentDiv.className = 'comment align-items-center row mb-3 d-flex align-items-center p-1';
    if (comment.user == null) {
        commentDiv.innerHTML += ` <h5>Anonymous</h5>`;
    } else {
        commentDiv.innerHTML += `<h5>${comment.user.firstName} ${comment.user.lastName}</h5>`;
    }
    commentDiv.innerHTML += `<p class="col-9">${comment.text}</p>`;
    
    if (comment.objectName && comment.objectName !== "") {
        commentDiv.innerHTML += `<img class="image-comment-fill" data-comment-objectName="${comment.objectName}" alt="Couldn't load comment image">`;
    }

    if (comment.isLikedByCurrentUser || likedComments.includes(comment.id)) {
        commentDiv.innerHTML += `<button id="unlikeButton_${comment.id}" type="submit" class="btn-like h4 col-2">♥</button><p id="likeCounter_${comment.id}" class="col-1">${comment.likes}</p>`;
    } else {
        commentDiv.innerHTML += `<button id="likeButton_${comment.id}" type="submit" class="btn-like h4 col-2">♡</button><p id="likeCounter_${comment.id}" class="col-1">${comment.likes}</p>`;
    }
    
    comment.subComments.forEach((subComment: any) => {
        let subCommentDiv = document.createElement('div');
        subCommentDiv.className = 'subComment row d-flex align-items-center';
        if (subComment.user == null) {
            subCommentDiv.innerHTML += `<h6>Anonymous</h6>`;
        } else {
            subCommentDiv.innerHTML += `<h6>${subComment.user.firstName} ${subComment.user.lastName}</h6>`;
        }
        subCommentDiv.innerHTML += `<p class="col-9">${subComment.text}</p>`;

        if (subComment.isLikedByCurrentUser || likedComments.includes(subComment.id)) {
            subCommentDiv.innerHTML += `<button id="unlikeButton_${subComment.id}" type="submit" class="btn-like h4 col-2">♥</button><p id="likeCounter_${subComment.id}" class="col-1">${subComment.likes}</p>`;
        } else {
            subCommentDiv.innerHTML += `<button id="likeButton_${subComment.id}" type="submit" class="btn-like h4 col-2">♡</button><p id="likeCounter_${subComment.id}" class="col-1">${subComment.likes}</p>`;
        }

        commentDiv.appendChild(subCommentDiv);
    });

    commentDiv.innerHTML += `
                    <div class="reply-section">
                    <textarea class="form-control" rows="2" id="replyText_${comment.id}" placeholder="Write your reply here..."></textarea>
                    <button id="replyButton_${comment.id}" type="submit" class="btn btn-primary btn-reply">Reply </button>
                    </div>
                `;
    commentsDiv!.appendChild(commentDiv);
}

function loadComments() {
    commentsDiv!.innerText = "";
    fetch(`/api/Comments/GetComments/${themeId?.innerText}`, {
        headers: {
            Accept: 'application/json'
        }
    }).then(r => {
        if (!r.ok) {
            return r.json().then(error => {
                let errorMessage = "";
                error.forEach((e: any) => errorMessage += e.errorMessage + "\n");
                alert(errorMessage);
            });
        }
        r.json().then(data => {
            data.forEach((comment: any) => {
                showComment(comment);
            });
            addImagesToComments();
            addEventListenerSubmitReply();
            addEventListenerLike();
            addEventListenerUnlike();
        });
    }).catch(error => {
        console.log(error.message)
    });
}

function addImagesToComments(){
    const elements = document.querySelectorAll('.image-comment-fill');

    elements.forEach(function(element) {
        insertSourceIntoImage(element as HTMLImageElement)
    });
}

function insertSourceIntoImage(element: HTMLImageElement){
    const objectName = element.getAttribute('data-comment-objectName') ;
    
    console.log(objectName); 
    if (objectName){
        getUrl(element,objectName)
    }
}

function submitComment() {
    const formData = new FormData(document.querySelector('#upload-form') as HTMLFormElement);
    const fileInput = document.querySelector('#upload-input') as HTMLInputElement;

    if (fileInput.files?.length === 0) {
        //todo hier ook aanpassingen
        const newCommentDto = {
            Text: commentText?.value,
            ThemeId: themeId?.innerText,
            Url: "",
            contentType: ""
        }
        
        fetch(`/api/Comments/AddComment`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(newCommentDto)
        }).then(r => {
            if (!r.ok) {
                throw new Error("Error submitting comment " + r.statusText);
            }
            return r.json();
        }).then(r => {
            loadComments();
        }).catch(function (error) {
            console.log(error);
        });
    } else {

        fetch("/api/files", {
            method: "POST",
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    alert("Upload failed!");
                }

                if (response.status === 200) {
                    return response.json() as Promise<Url>;
                } else {
                    throw new Error("Unexpected response status: " + response.status);
                }   
            })
            .then(responseObject => {
//todo aanpassing van var naar const
                const newCommentDto = {
                    Text: commentText?.value,
                    ThemeId: themeId?.innerText,
                    Url: responseObject.objectName,
                    contentType: responseObject.contentType
                }

                console.log(newCommentDto.Url);

                fetch(`/api/Comments/AddComment`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(newCommentDto)
                }).then(r => {
                    if (!r.ok) {
                        throw new Error("Error submitting comment " + r.statusText);
                    }
                    return r.json();
                }).then(r => {
                    loadComments();
                }).catch(function (error) {
                    console.log(error);
                });
                console.log(`Here is the image in the cloud: ${responseObject.objectName}`);
            })
            .catch(error => {
                console.error("Error:", error);
            });
    }
}

function submitReply(commentId: number, replyText: string) {
    const newReplyDto = {
        Text: replyText,
        CommentId: commentId
    };
    console.log(newReplyDto);

    fetch(`/api/Comments/AddReply`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(newReplyDto)
    }).then(r => {
        if (!r.ok) {
            throw new Error("Error submitting reply " + r.statusText);
        }
    }).then(r => {
        loadComments();
    }).catch(function (error) {
        console.log(error);
    })
}

