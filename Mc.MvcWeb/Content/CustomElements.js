var imgArrowRight = new Image();
imgArrowRight.src = "../../Images/arrow_right.png";
var imgArrowLeft = new Image();
imgArrowLeft.src = "../../Images/arrow_left.png";
var imgArrowTop = new Image();
imgArrowTop.src = "../../Images/arrow_top.png";
var imgArrowBottom = new Image();
imgArrowBottom.src = "../../Images/arrow_bottom.png";

function DrawHorizontalArrow(id, side) {
    var x = 0;
    var y = 0;
    var width = 300;
    var height = 150;
    var c = document.getElementById(id);
    var ctx = c.getContext("2d");
    var start;
    var current = 0;
    var end;
    var dx;
    if (side == "right") {
        start = x - 20;
        end = x + width;
        dx = 1;
    } else if (side == "left") {
        start = x + width-10;
        end = x - 30;
        dx = -1;
    } else {
        dx = 0;
    }
    current = start;
    setInterval(function() {
        ctx.clearRect(x, y, width, height);
        if (side == "right") 
            ctx.drawImage(imgArrowRight, current += dx, y + height / 2 - 8);
        else 
            ctx.drawImage(imgArrowLeft, current += dx, y + height / 2 - 8);
        if (current == end)
            current = start;
    }, 10);
}

function DrawVerticalArrow(id, side) {
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
}