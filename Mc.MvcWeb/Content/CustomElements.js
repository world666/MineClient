var imgArrowRight = new Image();
imgArrowRight.src = "../../Images/arrow_right.png";
var imgArrowLeft = new Image();
imgArrowLeft.src = "../../Images/arrow_left.png";
var imgArrowTop = new Image();
imgArrowTop.src = "../../Images/arrow_top.png";
var imgArrowBottom = new Image();
imgArrowBottom.src = "../../Images/arrow_bottom.png";

function DrawHorizontalArrow(id, side) {
    var canvas = document.getElementById(id);
    var obCanvas = canvas.getContext('2d');
    var start;
    var current = 0;
    var end;
    var dx;
    if (side == "right") {
        start = 0;
        end = 0 + canvas.width;
        dx = 1;
    } else if (side == "left") {
        start = canvas.width;
        end = 0;
        dx = -1;
    } else {
        dx = 0;
    }
    current = start;
   setInterval(function () {
        obCanvas.clearRect(0, 0, canvas.width, canvas.height);
        if (side == "right") {
            obCanvas.beginPath();
            obCanvas.lineWidth = 3;
            obCanvas.strokeStyle = 'black';
            obCanvas.moveTo(0, canvas.height / 2);
            obCanvas.lineTo(canvas.width / 5, canvas.height / 2);
            obCanvas.stroke();
        }
        if (side == "left")
        {
            obCanvas.beginPath();
            obCanvas.lineWidth = 5;
            obCanvas.strokeStyle = 'black';
            obCanvas.moveTo(current, canvas.height / 2);
            obCanvas.lineTo(current - 70, canvas.height / 2);//main
            obCanvas.lineTo(current - 70 + 15, canvas.height / 2 - 8);//arrow
            obCanvas.moveTo(current - 73, canvas.height / 2);//main
            obCanvas.lineTo(current - 70 + 15, canvas.height / 2 + 8);//arrow
            obCanvas.stroke();
            current += dx;
        }
        if (current == end)
            current = start;
    }, 10);
   
}

function DrawVerticalArrow(id, side) {
    var canvas = document.getElementById(id);
    var obCanvas = canvas.getContext('2d');
    var start;
    var current = 0;
    var end;
    var dx;
    if (side == "top") {
        start = canvas.height;
        end = 0;
        dx = -1;
    } else if (side == "bottom") {
        start = 0;
        end = canvas.height;
        dx = 1;
    } else {
        dx = 0;
    }
    current = start;
    setInterval(function () {
        obCanvas.clearRect(0, 0, canvas.width, canvas.height);
        if (side == "right") {
            obCanvas.beginPath();
            obCanvas.lineWidth = 3;
            obCanvas.strokeStyle = 'black';
            obCanvas.moveTo(0, canvas.height / 2);
            obCanvas.lineTo(canvas.width / 5, canvas.height / 2);
            obCanvas.stroke();
        }
        if (side == "top") {
            obCanvas.beginPath();
            obCanvas.lineWidth = 7;
            obCanvas.strokeStyle = 'black';
            obCanvas.moveTo(canvas.width / 2,current);
            obCanvas.lineTo(canvas.width / 2, current - 24);//main
            obCanvas.lineTo(canvas.width / 2 + 10, current - 24 + 5);//arrow
            obCanvas.moveTo(canvas.width / 2, current - 24);//main
            obCanvas.lineTo(canvas.width / 2 - 10, current - 24 + 5);//arrow*/
            obCanvas.stroke();
            current += dx;
        }
        if (current == end)
            current = start;
    }, 20);

}
function DrawVerticalArrow1(id, side) {
    var c = document.getElementById(id);
    var ctx = c.getContext("2d");
    var x = 0;
    var y = 0;
    var width = 300;
    var height = 300;
    var start;
    var current = 0;
    var end;
    var dy;
    if (side == "bottom") {
        start = y - 30;
        end = y + height;
        dy = 1;
    } else if (side == "top") {
        start = y + height-20;
        end = y-20;
        dy = -1;
    } else {
        dy = 0;
    }
    current = start;
    setInterval(function() {
        ctx.clearRect(x, y, width, height);
        if (side=="bottom")
            ctx.drawImage(imgArrowBottom, x + width / 2 - 8, current += dy); // Рисуем изображение от точки с координатами 0, 0
        else
            ctx.drawImage(imgArrowTop, x + width / 2 - 8, current += dy);
        if (current == end)
            current = start;
    }, 10);
}


function LadaSetState(name, state) {
    var c = document.getElementById(name);
    var ctx = c.getContext("2d");
    if (c.className == "lada_horizontal") {
        if (state == "Opened") {
            ctx.fillStyle = "#90EE90";
            if (name == "la")
                ctx.fillStyle = "#FF0000";
            ctx.fillRect(0, 0, 70, 200);
        } else if (state == "Closed") {
            ctx.fillStyle = "#FF0000";
            if (name == "la")
                ctx.fillStyle = "#90EE90";
            ctx.fillRect(0, 0, 400, 30);
        }
    } else {
        if (state == "Closed") {
            ctx.fillStyle = "#FF0000";
            ctx.fillRect(0, 0, 70, 200);
        } else if (state == "Opened") {
            ctx.fillStyle = "#90EE90";
            ctx.fillRect(0, 0, 400, 30);
        }
    }
}

function SetRevers() {
    
}

function SetFan1Work() {
    DrawVerticalArrow("fan1WorkTop", "top");
    DrawHorizontalArrow("fan1WorkLeft", "left");
    DrawHorizontalArrow("fan1WorkLeft2", "left");
}