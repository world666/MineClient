var imgArrowRight = new Image();
imgArrowRight.src = "../../Images/arrow_right.png";
var imgArrowLeft = new Image();
imgArrowLeft.src = "../../Images/arrow_left.png";
var imgArrowTop = new Image();
imgArrowTop.src = "../../Images/arrow_top.png";
var imgArrowBottom = new Image();
imgArrowBottom.src = "../../Images/arrow_bottom.png";

function DrawHorizontalArrow(id, side, length, size) {
    var canvas = document.getElementById(id);
    var obCanvas = canvas.getContext('2d');
    var start;
    var current = 0;
    var arrowLength = canvas.width * 0.1 + length;
    var arrowSize = canvas.width * 0.03 + size;
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
            obCanvas.lineWidth = 6;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(current, canvas.height / 2);
            obCanvas.lineTo(current + arrowLength, canvas.height / 2);//main
            obCanvas.stroke();
            obCanvas.lineWidth = 4;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(current + arrowLength, canvas.height / 2);
            obCanvas.lineTo(current + arrowLength - arrowSize, canvas.height / 2 - 8);//arrow
            obCanvas.moveTo(current + arrowLength + 3, canvas.height / 2);//main
            obCanvas.lineTo(current + arrowLength - arrowSize, canvas.height / 2 + 8);//arrow
            obCanvas.stroke();
            current += dx;
        }
        if (side == "left")
        {
            obCanvas.lineWidth = 6;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(current, canvas.height / 2);
            obCanvas.lineTo(current - arrowLength, canvas.height / 2);//main
            obCanvas.stroke();
            obCanvas.lineWidth = 4;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(current - arrowLength, canvas.height / 2);
            obCanvas.lineTo(current - arrowLength + arrowSize, canvas.height / 2 - 8);//arrow
            obCanvas.moveTo(current - arrowLength, canvas.height / 2);//main
            obCanvas.lineTo(current - arrowLength + arrowSize, canvas.height / 2 + 8);//arrow
            obCanvas.stroke();
            current += dx;
        }
        if (current == end)
            current = start;
    }, 10);
   
}

function DrawVerticalArrow(id, side, length, size) {
    var canvas = document.getElementById(id);
    var obCanvas = canvas.getContext('2d');
    var start;
    var current = 0;
    var end;
    var dx;
    var arrowLength = canvas.height * 0.15 + length;
    var arrowSize = canvas.height * 0.05 + size;
    if (side == "top") {
        start = canvas.height;
        end = 0;
        dx = -1;
    } else if (side == "down") {
        start = 0;
        end = canvas.height;
        dx = 1;
    } else {
        dx = 0;
    }
    current = start;
    setInterval(function () {
        obCanvas.clearRect(0, 0, canvas.width, canvas.height);
        if (side == "down") {
            obCanvas.lineWidth = 10;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(canvas.width / 2, current);
            obCanvas.lineTo(canvas.width / 2, current + arrowLength);//main
            obCanvas.stroke();
            obCanvas.lineWidth = 3;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(canvas.width / 2, current + arrowLength);
            obCanvas.lineTo(canvas.width / 2 + 15, current + arrowLength - arrowSize);//arrow
            obCanvas.moveTo(canvas.width / 2, current + arrowLength);//main
            obCanvas.lineTo(canvas.width / 2 - 15, current + arrowLength - arrowSize);//arrow*/
            obCanvas.stroke();
            current += dx;
        }
        if (side == "top") {
            
            obCanvas.lineWidth = 10;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(canvas.width / 2,current);
            obCanvas.lineTo(canvas.width / 2, current - arrowLength);//main
            obCanvas.stroke();
            obCanvas.lineWidth = 3;
            obCanvas.strokeStyle = 'black';
            obCanvas.beginPath();
            obCanvas.moveTo(canvas.width / 2, current - arrowLength);
            obCanvas.lineTo(canvas.width / 2 + 15, current - arrowLength + arrowSize);//arrow
            obCanvas.moveTo(canvas.width / 2, current - arrowLength);//main
            obCanvas.lineTo(canvas.width / 2 - 15, current - arrowLength + arrowSize);//arrow*/
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
            if (name == "ld")
                ctx.fillRect(c.width - 70, 0, c.width, c.height);
            else
            ctx.fillRect(0, 0, 70, 200);      
        } else if (state == "Closed") {
            ctx.fillStyle = "#FF0000";
            ctx.fillRect(0, 0, 400, 30);
        }
    } else {
        if (state == "Closed") {
            ctx.fillStyle = "#FF0000";
            if (name == "la")
                ctx.fillStyle = "#90EE90";
            ctx.fillRect(0, 0, 70, 200);
        } else if (state == "Opened") {
            ctx.fillStyle = "#90EE90";
            if (name == "la")
                ctx.fillStyle = "#FF0000";
            ctx.fillRect(0, 0, 400, 30);
        }
    }
}

function SetRevers() {
    
}

function SetFan1Work() {
    DrawVerticalArrow("fan1WorkTop", "top",8,1);
    DrawHorizontalArrow("fan1WorkLeft", "left",50,15);
    DrawHorizontalArrow("fan1WorkLeft2", "left",50,15);
    DrawHorizontalArrow("fan1WorkRight", "right", 0,0);
    DrawVerticalArrow("fan1WorkTop2", "top", 30, 4);
    DrawVerticalArrow("fan1WorkDown", "down", 30, 4);
    DrawHorizontalArrow("fan1WorkLeft3","left",80,15);
}
function SetFan2Work() {
    DrawVerticalArrow("fan2WorkDown2", "down", 8, 1);
    DrawHorizontalArrow("fan2WorkLeft", "left", 50, 15);
    DrawHorizontalArrow("fan2WorkLeft2", "left", 50, 15);
    DrawHorizontalArrow("fan2WorkRight", "right", 0, 0);
    DrawVerticalArrow("fan2WorkTop2", "top", 30, 4);
    DrawVerticalArrow("fan2WorkDown", "down", 30, 4);
    DrawHorizontalArrow("fan2WorkLeft3", "left", 80, 15);
    DrawVerticalArrow("fan2WorkTop3", "top", 0, -2);
}
function Revers() {
    DrawVerticalArrow("reversDown", "down", 30, 4);
    DrawHorizontalArrow("reversRightNormaLeft", "right", 50, 15);
    DrawVerticalArrow("reversDownNormaTop", "down", 30, 4);
    DrawHorizontalArrow("reversLeftNormaRight", "left", 50, 15);
    DrawHorizontalArrow("reversLeft", "left", 50, 15);
    DrawVerticalArrow("reversDown2", "down", 10, 4);
    DrawHorizontalArrow("reversNormaRight", "right", 0, 0);
    DrawHorizontalArrow("reversNormaLeft", "left", 14, 2);
}
function Norma() {
    DrawHorizontalArrow("reversRightNormaLeft", "left", 50, 15);
    DrawVerticalArrow("reversDownNormaTop", "top", 30, 4);
    DrawHorizontalArrow("reversLeftNormaRight", "right", 50, 15);
    DrawHorizontalArrow("reversNormaRight", "right", 0, 0);
    DrawHorizontalArrow("reversNormaLeft", "left", 14, 2);
    DrawHorizontalArrow("normaLeft", "left", 50, 15);
}